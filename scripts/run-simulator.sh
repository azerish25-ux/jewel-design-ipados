#!/bin/bash
set -euo pipefail
APP=$(find "$PWD/tests/iOS/bin/Debug" -type d -name '*.app' | head -1)
test -n "$APP" && test -f "$APP/Info.plist"
printf 'Installing %s\n' "$APP"
xcrun simctl list devices available -j > artifacts/devices.json
FAMILY=${SIMULATOR_FAMILY:-ipad}
node scripts/ios-device.cjs artifacts/devices.json "$FAMILY" > artifacts/selected-device.json
UDID=$(node -p 'require("./artifacts/selected-device.json").udid')
xcrun simctl boot "$UDID" || true
xcrun simctl bootstatus "$UDID" -b
xcrun simctl install "$UDID" "$APP"
xcrun simctl launch --console-pty --terminate-running-process "$UDID" com.jeweldesign.reusesmoke > artifacts/app-console.txt 2>&1 &
LAUNCH_PID=$!
DATA=$(xcrun simctl get_app_container "$UDID" com.jeweldesign.reusesmoke data)
for i in $(seq 1 60); do
  if test -f "$DATA/Documents/reuse-result.txt"; then
    cp "$DATA/Documents/reuse-result.txt" artifacts/simulator-result.txt
    cat artifacts/simulator-result.txt
    cp "$DATA/Documents/reuse-result.txt.family" artifacts/runtime-family.txt
    test "$(cat artifacts/runtime-family.txt)" = "$FAMILY"
    grep -q '^PASS 16 ' artifacts/simulator-result.txt
    exit $?
  fi
  sleep 1
done
cat artifacts/app-console.txt || true
find "$DATA" -maxdepth 3 -name 'reuse-result*' -print
xcrun simctl spawn "$UDID" log show --last 4m --predicate 'eventMessage CONTAINS[c] "com.jeweldesign.reusesmoke" OR process == "iOS"' > artifacts/app-system.log || true
mkdir -p artifacts/crash-reports
find "$HOME/Library/Logs/DiagnosticReports" -maxdepth 1 -name 'iOS*.ips' -mmin -15 -exec cp {} artifacts/crash-reports/ \; || true
echo 'Simulator result missing' >&2
exit 1
