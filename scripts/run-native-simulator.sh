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
DATA=$(xcrun simctl get_app_container "$UDID" com.jeweldesign.nativegeometry data)
# A result from an earlier launch cannot prove this invocation ran.
rm -f "$DATA/Documents/native-result.txt" "$DATA/Documents/native-result.txt.family" "$DATA/Documents/native-result.txt.started"
xcrun simctl launch --terminate-running-process "$UDID" com.jeweldesign.nativegeometry | tee "$OUT/launch.txt"
for i in $(seq 1 120); do
 if test -f "$DATA/Documents/native-result.txt"; then
  cp "$DATA/Documents/native-result.txt" "$OUT/result.txt"
  cp "$DATA/Documents/native-result.txt.family" "$OUT/runtime-family.txt"
  cat "$OUT/result.txt"
  test "$(cat "$OUT/runtime-family.txt")" = "$FAMILY"
  grep -q '^PASS OCCT ' "$OUT/result.txt"
  grep -q '^PASS 16 original-binary golden Bernstein cases;' "$OUT/result.txt"
  grep -q '^PASS managed candidate ABI:' "$OUT/result.txt"
  grep -q 'PASS candidate native curves:' "$OUT/result.txt"
  grep -q 'PASS ring rail candidate:' "$OUT/result.txt"
  grep -q 'native-curve-line-events=pass;' "$OUT/result.txt"
  grep -q 'PASS native finite curve-line events:' "$OUT/result.txt"
  grep -q 'native-closed-seam=pass;' "$OUT/result.txt"
  grep -q 'PASS native closed-curve seam:' "$OUT/result.txt"
  grep -q 'native-curve-bounds=pass;' "$OUT/result.txt"
  grep -q 'PASS native curve bounds:' "$OUT/result.txt"
  grep -q 'native-uniform-affine-and-closure=pass;' "$OUT/result.txt"
  grep -q 'PASS outside rail candidate:' "$OUT/result.txt"
  grep -q 'native-nurbs-construction=pass;' "$OUT/result.txt"
  grep -q 'PASS stored native profiles:' "$OUT/result.txt"
  grep -q 'PASS value graph channels:28160 source differential cases;' "$OUT/result.txt"
  grep -q 'PASS evaluated value-channel geometry:' "$OUT/result.txt"
  grep -q 'PASS legacy rail block bindings:' "$OUT/result.txt"
  grep -q 'PASS record-bound rail builders:' "$OUT/result.txt"
  grep -q 'PASS graph-managed rail outputs:' "$OUT/result.txt"
  grep -q 'PASS select ring rail:150 exact-source forwarding cases,' "$OUT/result.txt"
  grep -q 'PASS saved transform producers:16093 exact-source' "$OUT/result.txt"
  grep -q 'PASS two-rail profile placement:4496 extracted frame/seam cases;' "$OUT/result.txt"
  grep -q 'published-rotation-reference=pass;' "$OUT/result.txt"
  grep -q 'PASS saved rotation producers:44217 recovered-wrapper/published-native cases,' "$OUT/result.txt"
  grep -q 'PASS expandable transform consumer:12285 exact source socket cases,' "$OUT/result.txt"
  grep -q 'PASS legacy array values:289 exact-source arithmetic pairs/33 null-item failures;789 source flatten/copy trees,' "$OUT/result.txt"
  grep -q 'PASS collection socket constructors:12285 exact-source zero-fixed-input cases;' "$OUT/result.txt"
  grep -q 'PASS owned native collections:18 source output comparisons,' "$OUT/result.txt"
  grep -q 'native-sweep-feasibility=observed;' "$OUT/result.txt"
  grep -q 'tangent-plane-three-section-shell=pass;' "$OUT/result.txt"
  grep -q 'contact-on-border-retained-profiles=1/3;' "$OUT/result.txt"
  grep -q 'arbitrary-two-rail-correspondence=unverified;' "$OUT/result.txt"
  grep -q 'PASS explicit sweep2 preparation:3906 exact stable-order cases;' "$OUT/result.txt"
  grep -q 'PASS native array consumers:49 source first-item cases;' "$OUT/result.txt"
  exit 0
 fi
 sleep 1
done
find "$DATA/Documents" -maxdepth 1 -name 'native-result*' -print > "$OUT/result-files.txt"
xcrun simctl spawn "$UDID" log show --last 3m --predicate 'eventMessage CONTAINS[c] "com.jeweldesign.nativegeometry"' > "$OUT/system.log" 2>&1 || true
echo 'Native candidate result missing' >&2
exit 1
