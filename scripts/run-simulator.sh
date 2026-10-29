#!/bin/bash
set -euo pipefail
APP=$(find "$PWD/tests/iOS/bin/Debug" -type d -name '*.app' | head -1)
test -n "$APP" && test -f "$APP/Info.plist"
printf 'Installing %s\n' "$APP"
xcrun simctl list devices available -j > artifacts/devices.json
UDID=$(python3 -c 'import json; d=json.load(open("artifacts/devices.json")); print(next(v["udid"] for k,vs in d["devices"].items() if "iOS" in k for v in vs if "iPad" in v["name"]))')
xcrun simctl boot "$UDID" || true
xcrun simctl bootstatus "$UDID" -b
xcrun simctl install "$UDID" "$APP"
xcrun simctl launch --console-pty --terminate-running-process "$UDID" com.jeweldesign.reusesmoke > artifacts/app-console.txt 2>&1 &
LAUNCH_PID=$!
DATA=$(xcrun simctl get_app_container "$UDID" com.jeweldesign.reusesmoke data)
for i in $(seq 1 60); do
  if test -f "$DATA/Documents/reuse-result.txt"; then
    cp "$DATA/Documents/reuse-result.txt" artifacts/ipad-simulator-result.txt
    cat artifacts/ipad-simulator-result.txt
    grep -q '^PASS 16 ' artifacts/ipad-simulator-result.txt
    exit $?
  fi
  sleep 1
done
cat artifacts/app-console.txt || true
find "$DATA" -maxdepth 3 -name 'reuse-result*' -print
xcrun simctl spawn "$UDID" log show --last 4m --predicate 'process == "iOS"' > artifacts/app-system.log || true
echo 'Simulator result missing' >&2
exit 1
