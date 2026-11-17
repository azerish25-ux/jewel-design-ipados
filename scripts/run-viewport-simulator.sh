#!/bin/bash
set -euo pipefail
FAMILY=${1:?supply ipad or iphone}
case "$FAMILY" in ipad|iphone);; *) echo "Unsupported simulator family" >&2; exit 2;; esac
OUT="artifacts/viewport-$FAMILY"
mkdir -p "$OUT" "$OUT/capture-attempts"
for phase in original orbit edited empty resized final paper-original paper-green paper-blue paper-clear paper-resized paper-final trace-original trace-image trace-orbit trace-hidden trace-moved trace-final; do for attempt in 1 2 3 4 5 6; do rm -f "$OUT/capture-attempts/$phase-$attempt.png" "$OUT/capture-attempts/$phase-$attempt.json"; done; done
rm -f "$OUT/capture-timing.txt" "$OUT/app-capture-timing.txt"
for phase in original orbit edited empty resized final paper-original paper-green paper-blue paper-clear paper-resized paper-final trace-original trace-image trace-orbit trace-hidden trace-moved trace-final; do rm -f "$OUT/$phase.png"; done
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
   case "$phase" in original|orbit|edited|empty|resized|final|paper-original|paper-green|paper-blue|paper-clear|paper-resized|paper-final|trace-original|trace-image|trace-orbit|trace-hidden|trace-moved|trace-final) ;; *) echo "Invalid viewport capture phase" >&2; exit 2;; esac
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
  grep -q '^PASS legacy native user-string bodies:' "$OUT/result.txt"
  grep -q '^PASS offline encoded image assets:' "$OUT/result.txt"
  grep -q '^PASS GDI enlargement:1408 exact complete native buffers' "$OUT/result.txt"
  grep -q '^PASS context image cache:33 hit/miss/usage/context/failure/lease/budget cases/65536 exact CRT-C UTF16 lower units/1156 native compare pairs;' "$OUT/result.txt"
  grep -q '^PASS Unicode NLS1 lower:1114112 exact native unit/scalar mappings/3329 native contexts/7 null/budget/UTF16 guards;' "$OUT/result.txt"
  grep -q '^PASS NLS1 comparison inputs:65536 exact active native weights/730 exact ordered expansions/2 expansion-index guards;' "$OUT/result.txt"
  grep -q '^PASS source entry-name copy:144 mode/CRT/string cases/444 C-prefix/query/ownership/failure checks;' "$OUT/result.txt"
  grep -q '^PASS native cache ordering:9 actual alias-selection/lease/atomic-failure checks;' "$OUT/result.txt"
  grep -q '^PASS CRT100 portable qsort:10431 exact native orders/1972827 exact callback events/2 dependency guards;' "$OUT/result.txt"
  grep -q '^PASS mutable string runtime:68 source flag transitions/683 literal/mode/cache/guard checks;' "$OUT/result.txt"
  grep -q '^PASS ANSI portable profile:1112064 native scalar byte outputs/65536 native comparison pairs/65536 native lower pairs/65536 native decode pairs/4096 legacy recovery cases/136 boundary/cache checks;' "$OUT/result.txt"
  grep -q '^PASS CRT100 portable profiles:3342336 native lower inputs/3345804 native comparisons/147 whole buffers/257 boundary/cache checks;' "$OUT/result.txt"
  grep -q '^PASS NLS1 filename policy:40663 native wrapper pairs/75 boundary/cache checks;' "$OUT/result.txt"
  grep -q '^PASS direct NLS1 strings:40663 exact native comparisons/213 preserved key-order disagreements;' "$OUT/result.txt"
  grep -q '^PASS direct NLS1 exhaustive:4456448 exact native comparisons/0 mismatches;' "$OUT/result.txt"
  grep -q '^PASS direct NLS1 Hangul:6507500 exact native comparisons/0 mismatches;' "$OUT/result.txt"
  grep -q '^PASS direct NLS1 stress:103280 exact native comparisons/7 null/budget/terminator guards;' "$OUT/result.txt"
  grep -q '^PASS actual source cache GPU:8 exact image readbacks/1 decode-and-upload-free hit/2 retained-image failures/2 invalidated color kinds/1 independent context/1 queued-disposal frame;' "$OUT/result.txt"
  grep -q '^PASS prepared wallpaper:120 exact native stored/RGBA and decoded-adapter cases/9 storage/depth/fit/budget/provenance guards;' "$OUT/result.txt"
  grep -q '^PASS actual prepared wallpaper GPU:20 decoded source/HALFTONE24/post-gray/upload cases/194272 exact opaque pixels/1 retained-image failure guard;' "$OUT/result.txt"
  grep -q '^PASS composed texture preparation:120 exact native stored/RGBA and decoded-adapter cases/17 gray-order distinctions/27 processed-alpha distinctions/10 ownership/storage/depth/budget guards;' "$OUT/result.txt"
  grep -q '^PASS actual composed texture GPU:20 decoded PNG/source preparation/upload/readback cases/584 exact pixels/1 retained-image failure guard;' "$OUT/result.txt"
  grep -q '^PASS managed GDI HALFTONE:3860 exact complete stored-buffer cases/7 explicit storage/depth/domain/budget guards;' "$OUT/result.txt"
  grep -q '^PASS original trace plane:' "$OUT/result.txt"
  grep -q '^PASS visible world trace:' "$OUT/result.txt"
  grep -q '^PASS original texture preparation:' "$OUT/result.txt"
  grep -q '^PASS actual texture preparation GPU:' "$OUT/result.txt"
  grep -q '^PASS original GDI wallpaper placement:' "$OUT/result.txt"
  grep -q '^PASS visible CAD wallpaper:' "$OUT/result.txt"
  grep -q '^PASS lossless PNG samples:' "$OUT/result.txt"
  grep -q '^PASS straight image provider:' "$OUT/result.txt"
  grep -q '^PASS actual straight image decode:' "$OUT/result.txt"
  grep -q '^PASS actual Metal image quad:' "$OUT/result.txt"
  grep -q '^PASS actual offline Apple image decode:' "$OUT/result.txt"
  grep -q '^PASS original-derived wallpaper raster:' "$OUT/result.txt"
  grep -q '^PASS original-derived SetView preflight:' "$OUT/result.txt"
  grep -q '^PASS original-derived Pop merge:' "$OUT/result.txt"
  grep -q '^PASS original-derived history push:' "$OUT/result.txt"
  grep -q '^PASS original-derived runtime view:' "$OUT/result.txt"
  grep -q '^PASS copied user-string provenance:' "$OUT/result.txt"
  grep -q '^PASS actual user-string archive binding:' "$OUT/result.txt"
  grep -q '^PASS original-derived userdata runtime:' "$OUT/result.txt"
  grep -q '^PASS owned original5 view data:' "$OUT/result.txt"
  grep -q '^PASS viewport screenshot acknowledgement:' "$OUT/result.txt"
  grep -q 'PASS Metal offscreen viewport:actual-CAD pixels=' "$OUT/result.txt"
  cp "$DATA/Documents/straight-image-result.json" "$OUT/straight-image-result.json"
  cp "$DATA/Documents/straight-image-gpu.bgra" "$OUT/straight-image-gpu.bgra"
  cp "$DATA/Documents/image-gpu-result.json" "$OUT/image-gpu-result.json"
  cp "$DATA/Documents/image-gpu-bmp.bgra" "$OUT/image-gpu-bmp.bgra"
  cp "$DATA/Documents/source-cache-gpu.json" "$OUT/source-cache-gpu.json"
  for index in $(seq 0 7); do cp "$DATA/Documents/source-cache-$index.bgra" "$OUT/source-cache-$index.bgra"; done
  node scripts/verify-source-cache-gpu.cjs "$OUT" > "$OUT/source-cache-gpu-verification.json"
  cp "$DATA/Documents/prepared-wallpaper-gpu.json" "$OUT/prepared-wallpaper-gpu.json"
  for index in $(seq 0 19); do cp "$DATA/Documents/prepared-wallpaper-$index.bgra" "$OUT/prepared-wallpaper-$index.bgra"; done
  node scripts/verify-prepared-wallpaper-gpu.cjs "$OUT" > "$OUT/prepared-wallpaper-gpu-verification.json"
  cp "$DATA/Documents/composed-texture-gpu.json" "$OUT/composed-texture-gpu.json"
  for index in $(seq 0 19); do cp "$DATA/Documents/composed-texture-$index.bgra" "$OUT/composed-texture-$index.bgra"; done
  node scripts/verify-composed-texture-gpu.cjs "$OUT" > "$OUT/composed-texture-gpu-verification.json"
  cp "$DATA/Documents/texture-preparation-gpu.json" "$OUT/texture-preparation-gpu.json"
  cp "$DATA/Documents/texture-preparation-gpu.bgra" "$OUT/texture-preparation-gpu.bgra"
  cp "$DATA/Documents/trace-report.json" "$OUT/trace-report.json"
  cp "$DATA/Documents/anisotropy-gpu.json" "$OUT/anisotropy-gpu.json"
  for maximum in 1 4 8 16; do cp "$DATA/Documents/anisotropy-$maximum.bgra" "$OUT/anisotropy-$maximum.bgra"; done
  cp "$DATA/Documents/wallpaper-report.json" "$OUT/wallpaper-report.json"
  cp "$DATA/Documents/viewport-report.json" "$OUT/report.json"
  for phase in original orbit edited empty resized final paper-original paper-green paper-blue paper-clear paper-resized paper-final trace-original trace-image trace-orbit trace-hidden trace-moved trace-final; do test -s "$OUT/$phase.png"; done
  node scripts/check-viewport-report.cjs "$OUT/report.json" > "$OUT/report-verification.json"
  node scripts/verify-viewport-screenshots.cjs "$OUT" > "$OUT/screen-pixel-verification.json"
  node scripts/verify-wallpaper-screenshots.cjs "$OUT" > "$OUT/wallpaper-pixel-verification.json"
  node scripts/verify-trace-screenshots.cjs "$OUT" > "$OUT/trace-pixel-verification.json"
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
