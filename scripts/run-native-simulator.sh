#!/bin/bash
set -euo pipefail
FAMILY=${1:?supply ipad or iphone}
OUT="artifacts/native-$FAMILY"
mkdir -p "$OUT"
xcrun simctl list devices available -j > "$OUT/devices.json"
node scripts/ios-device.cjs "$OUT/devices.json" "$FAMILY" > "$OUT/selected-device.json"
UDID=$(node -p 'require("./"+process.argv[1]).udid' "$OUT/selected-device.json")
APP=$(find tests/iOS.Native/bin/Debug -type d -name '*.app' | head -1)
test -f "$APP/Info.plist"
xcrun simctl boot "$UDID" > "$OUT/boot.txt" 2>&1 || true
xcrun simctl bootstatus "$UDID" -b | tee "$OUT/bootstatus.txt"
xcrun simctl install "$UDID" "$APP"
xcrun simctl launch --terminate-running-process "$UDID" com.jeweldesign.nativegeometry | tee "$OUT/launch.txt"
DATA=$(xcrun simctl get_app_container "$UDID" com.jeweldesign.nativegeometry data)
for i in $(seq 1 120); do
 if test -f "$DATA/Documents/native-result.txt"; then
  cp "$DATA/Documents/native-result.txt" "$OUT/result.txt"
  cp "$DATA/Documents/native-result.txt.family" "$OUT/runtime-family.txt"
  cat "$OUT/result.txt"
  test "$(cat "$OUT/runtime-family.txt")" = "$FAMILY"
  grep -q '^PASS OCCT ' "$OUT/result.txt"
  exit 0
 fi
 sleep 1
done
find "$DATA/Documents" -maxdepth 1 -name 'native-result*' -print > "$OUT/result-files.txt"
xcrun simctl spawn "$UDID" log show --last 3m --predicate 'eventMessage CONTAINS[c] "com.jeweldesign.nativegeometry"' > "$OUT/system.log" 2>&1 || true
echo 'Native candidate result missing' >&2
exit 1
