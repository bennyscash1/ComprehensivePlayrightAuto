#!/bin/bash

echo "no" | avdmanager create avd -n test -k "system-images;android-30;google_apis;arm64-v8a" --device "pixel"

# Start VNC server
Xvfb :0 -screen 0 1080x1920x24 &> /dev/null &
export DISPLAY=:0
fluxbox &> /dev/null &
x11vnc -display :0 -nopw -forever -shared -bg -rfbport 5901

# Start ADB server
adb start-server

# Start emulator
emulator -avd test -no-audio -no-boot-anim -gpu swiftshader_indirect -no-snapshot -no-window -port 5555 -qemu -dns-server 8.8.8.8 > /var/log/emulator.log 2>&1 &

# Wait for device to be ready
echo "Waiting for emulator to boot..."
adb wait-for-device
BOOT_COMPLETED=0
while [ $BOOT_COMPLETED -eq 0 ]; do
    BOOT_COMPLETED=$(adb shell getprop sys.boot_completed 2>/dev/null | tr -d '\r')
    sleep 2
done
echo "Emulator booted successfully!"

# Keep container running
tail -f /var/log/emulator.log