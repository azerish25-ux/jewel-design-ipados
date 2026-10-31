#!/bin/bash
set -euo pipefail
FAMILY=${1:?supply ipad or iphone}
case "$FAMILY" in ipad|iphone);; *) echo "Unsupported simulator family" >&2; exit 2;; esac
OUT="artifacts/viewport-$FAMILY"
mkdir -p "$OUT"
for phase in original orbit edited empty resized final; do rm -f "$OUT/$phase.png"; done
rm -f "$OUT/result.txt" "$OUT/runtime-family.txt" "$OUT/report.json" "$OUT/report-verification.json"
xcrun simctl list devices available -j > "$OUT/devices.json"
node scripts/ios-device.cjs "$OUT/devices.json" "$FAMILY" > "$OUT/selected-device.json"
UDID=$(node -p 'require("./"+process.argv[1]).udid' "$OUT/selected-device.json")
cleanup(){ local result=$?; trap - EXIT; xcrun simctl shutdown "$UDID" > "$OUT/shutdown.txt" 2>&1 || true; exit "$result"; }
trap cleanup EXIT
APP=$(find tests/iOS.Viewport/bin/Debug -type d -name '*.app' | head -1)
test -f "$APP/Info.plist"
xcrun simctl boot "$UDID" > "$OUT/boot.txt" 2>&1 || true
xcrun simctl bootstatus "$UDID" -b | tee "$OUT/bootstatus.txt"
xcrun simctl install "$UDID" "$APP"
DATA=$(xcrun simctl get_app_container "$UDID" com.jeweldesign.viewport data)
[[ "$DATA" == /* && "$DATA" != / ]]
rm -f "$DATA/Documents/viewport-result.txt" "$DATA/Documents/viewport-result.txt.family" "$DATA/Documents/viewport-result.txt.started" "$DATA/Documents/viewport-report.json" "$DATA/Documents/viewport-stage.txt" "$DATA/Documents/viewport-ack.txt"
rm -f "$DATA/Documents/native-metal-original.png" "$DATA/Documents/native-metal-edited.png" "$DATA/Documents/native-metal-orbit.png" "$DATA/Documents/native-metal-empty.png" "$DATA/Documents/native-metal-report.json"
xcrun simctl launch --terminate-running-process "$UDID" com.jeweldesign.viewport | tee "$OUT/launch.txt"
last=''
for i in $(seq 1 600); do
 if test -f "$DATA/Documents/viewport-stage.txt"; then
  payload=$(cat "$DATA/Documents/viewport-stage.txt")
  if test "$payload" != "$last"; then
   [[ "$payload" =~ ^[0-9a-f]{32}\|(original|orbit|edited|empty|resized|final)$ ]]
   phase=${payload#*|}
   xcrun simctl io "$UDID" screenshot --type=png "$OUT/$phase.png"
   printf '%s' "$payload" > "$DATA/Documents/viewport-ack.pending"
   mv "$DATA/Documents/viewport-ack.pending" "$DATA/Documents/viewport-ack.txt"
   last="$payload"
  fi
 fi
 if test -f "$DATA/Documents/viewport-result.txt"; then
  cp "$DATA/Documents/viewport-result.txt" "$OUT/result.txt"
  cp "$DATA/Documents/viewport-result.txt.family" "$OUT/runtime-family.txt"
  cat "$OUT/result.txt"
  test "$(cat "$OUT/runtime-family.txt")" = "$FAMILY"
  grep -q '^PASS visible Metal viewport:6 captured stages;' "$OUT/result.txt"
  grep -q 'PASS viewport frame ownership:116 resource-only checks;' "$OUT/result.txt"
  grep -q 'PASS source camera presets:120 unchanged-wrapper comparisons' "$OUT/result.txt"
  grep -q 'PASS Metal offscreen viewport:actual-CAD pixels=' "$OUT/result.txt"
  cp "$DATA/Documents/viewport-report.json" "$OUT/report.json"
  for phase in original orbit edited empty resized final; do test -s "$OUT/$phase.png"; done
  node scripts/check-viewport-report.cjs "$OUT/report.json" > "$OUT/report-verification.json"
  mkdir -p "$OUT/offscreen"
  for phase in original edited orbit empty; do cp "$DATA/Documents/native-metal-$phase.png" "$OUT/offscreen/$phase.png"; done
  cp "$DATA/Documents/native-metal-report.json" "$OUT/offscreen/report.json"
  node scripts/verify-metal-artifacts.cjs "$OUT/offscreen" > "$OUT/offscreen/pixel-verification.json"
  exit 0
 fi
 sleep 1
done
find "$DATA/Documents" -maxdepth 1 -name 'viewport-*' -type f -exec cp {} "$OUT/" \;
xcrun simctl spawn "$UDID" log show --last 5m --predicate 'eventMessage CONTAINS[c] "com.jeweldesign.viewport"' > "$OUT/system.log" 2>&1 || true
echo 'Visible viewport result missing' >&2
exit 1
