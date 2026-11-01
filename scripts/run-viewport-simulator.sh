#!/bin/bash
set -euo pipefail
FAMILY=${1:?supply ipad or iphone}
case "$FAMILY" in ipad|iphone);; *) echo "Unsupported simulator family" >&2; exit 2;; esac
OUT="artifacts/viewport-$FAMILY"
mkdir -p "$OUT" "$OUT/capture-attempts"
for phase in original orbit edited empty resized final; do for attempt in 1 2 3 4 5 6; do rm -f "$OUT/capture-attempts/$phase-$attempt.png" "$OUT/capture-attempts/$phase-$attempt.json"; done; done
rm -f "$OUT/capture-timing.txt" "$OUT/app-capture-timing.txt"
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
rm -f "$DATA/Documents/viewport-capture-timing.txt" "$DATA/Documents/viewport-result.txt" "$DATA/Documents/viewport-result.txt.family" "$DATA/Documents/viewport-result.txt.started" "$DATA/Documents/viewport-report.json" "$DATA/Documents/viewport-stage.txt" "$DATA/Documents/viewport-ack.txt"
rm -f "$DATA/Documents/native-metal-original.png" "$DATA/Documents/native-metal-edited.png" "$DATA/Documents/native-metal-orbit.png" "$DATA/Documents/native-metal-empty.png" "$DATA/Documents/native-metal-report.json"
xcrun simctl launch --terminate-running-process "$UDID" com.jeweldesign.viewport | tee "$OUT/launch.txt"
last=''
for i in $(seq 1 600); do
 if test -f "$DATA/Documents/viewport-stage.txt"; then
  payload=$(cat "$DATA/Documents/viewport-stage.txt")
  if test "$payload" != "$last"; then
   capture_token=${payload%%|*}
   phase=${payload#*|}
   if test "$capture_token" = "$payload" || test "${#capture_token}" -ne 32; then echo "Invalid viewport capture token" >&2; exit 2; fi
   case "$capture_token" in *[!0123456789abcdef]*) echo "Invalid viewport capture token" >&2; exit 2;; esac
   case "$phase" in original|orbit|edited|empty|resized|final) ;; *) echo "Invalid viewport capture phase" >&2; exit 2;; esac
   capture_ready=0
   for attempt in 1 2 3 4 5 6; do
    candidate="$OUT/capture-attempts/$phase-$attempt.png"
    printf 'begin|%s|%s\n' "$payload" "$(date -u '+%Y-%m-%dT%H:%M:%SZ')" >> "$OUT/capture-timing.txt"
    xcrun simctl io "$UDID" screenshot --type=png "$candidate"
    printf 'captured|%s|%s\n' "$payload" "$(date -u '+%Y-%m-%dT%H:%M:%SZ')" >> "$OUT/capture-timing.txt"
    if node scripts/check-viewport-capture-surface.cjs "$candidate" > "$OUT/capture-attempts/$phase-$attempt.json"; then
     cp "$candidate" "$OUT/$phase.png"
     capture_ready=1
     break
    else
     capture_status=$?
     if test "$capture_status" -ne 2; then exit "$capture_status"; fi
    fi
    sleep 1
   done
   if test "$capture_ready" -ne 1; then echo "Viewport capture remained obstructed for $phase" >&2; exit 1; fi
   printf '%s' "$payload" > "$DATA/Documents/viewport-ack.pending"
   mv "$DATA/Documents/viewport-ack.pending" "$DATA/Documents/viewport-ack.txt"
   printf 'acknowledged|%s|%s\n' "$payload" "$(date -u '+%Y-%m-%dT%H:%M:%SZ')" >> "$OUT/capture-timing.txt"
   last="$payload"
  fi
 fi
 if test -f "$DATA/Documents/viewport-result.txt"; then
  cp "$DATA/Documents/viewport-result.txt" "$OUT/result.txt"
  cp "$DATA/Documents/viewport-result.txt.family" "$OUT/runtime-family.txt"
  if test -f "$DATA/Documents/viewport-capture-timing.txt"; then cp "$DATA/Documents/viewport-capture-timing.txt" "$OUT/app-capture-timing.txt"; fi
  cat "$OUT/result.txt"
  test "$(cat "$OUT/runtime-family.txt")" = "$FAMILY"
  grep -q '^PASS visible Metal viewport:6 captured stages;' "$OUT/result.txt"
  grep -q 'PASS viewport frame ownership:116 resource-only checks;' "$OUT/result.txt"
  grep -q 'PASS source camera presets:120 unchanged-wrapper comparisons' "$OUT/result.txt"
  grep -q 'PASS published frustum/lens:1245 independently compiled-source cases' "$OUT/result.txt"
  grep -q 'PASS published camera matrices:118 compiled-source cases and16 independent Metal clip endpoints;' "$OUT/result.txt"
  grep -q 'PASS published camera frames:730 sequences/2978 source steps;' "$OUT/result.txt"
  grep -q 'PASS published viewport transitions:961 sequences/6675 source steps;' "$OUT/result.txt"
  grep -q 'PASS original Rhino5 scalar validity:18 native-branch boundary cases/196608 IEEE high-word patterns;' "$OUT/result.txt"
  grep -q 'PASS original Rhino5 camera frame:32 native-derived state/boundary checks and4 analytical poses;' "$OUT/result.txt"
  grep -q 'PASS original Rhino5 viewport:26 native-derived perspective/frustum/lens/target checks;' "$OUT/result.txt"
  grep -q '^PASS original-derived Rhino5 vector rotation:' "$OUT/result.txt"
  grep -q '^PASS original-derived Rhino5 camera locations:' "$OUT/result.txt"
  grep -q '^PASS original-derived Rhino5 viewport cache:' "$OUT/result.txt"
  grep -q '^PASS original-derived Rhino5 numeric viewport commit:' "$OUT/result.txt"
  grep -q '^PASS viewport screenshot acknowledgement:' "$OUT/result.txt"
  grep -q 'PASS Metal offscreen viewport:actual-CAD pixels=' "$OUT/result.txt"
  cp "$DATA/Documents/viewport-report.json" "$OUT/report.json"
  for phase in original orbit edited empty resized final; do test -s "$OUT/$phase.png"; done
  node scripts/check-viewport-report.cjs "$OUT/report.json" > "$OUT/report-verification.json"
  node scripts/verify-viewport-screenshots.cjs "$OUT" > "$OUT/screen-pixel-verification.json"
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
