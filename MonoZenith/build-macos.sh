#!/bin/bash

# Configuratie
APP_NAME="EchoesOfTheShattering"
VERSION="1.0.0"
IDENTIFIER="com.nickjordan.echoesoftheshattering"
BUILD_DIR="$APP_NAME.app"
ICON_PATH="AppIcon.icns"

echo "Building the game..."

# Stap 1: Build de game
dotnet publish -c Release -r osx-arm64 --self-contained -o "$BUILD_DIR/Contents/MacOS"

# Stap 2: Maak de binary uitvoerbaar
chmod +x "$BUILD_DIR/Contents/MacOS/MonoZenith"

echo "Finished building the game."

# Stap 3: Maak de Info.plist
echo "Creating the Info.plist file..."

mkdir -p "$BUILD_DIR/Contents"
cat <<EOF > "$BUILD_DIR/Contents/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
    <dict>
        <key>CFBundleExecutable</key>
        <string>EchoesOfTheShattering</string>
        <key>CFBundleIdentifier</key>
        <string>$IDENTIFIER</string>
        <key>CFBundleName</key>
        <string>$APP_NAME</string>
        <key>CFBundleVersion</key>
        <string>$VERSION</string>
        <key>CFBundleShortVersionString</key>
        <string>$VERSION</string>
        <key>CFBundleIconFile</key>
        <string>$ICON_PATH</string>
        <key>NSHighResolutionCapable</key>
        <true/>
    </dict>
</plist>
EOF

echo "Finished creating the Info.plist file."

# Stap 4: Fix permissies en verwijder quarantine flags
echo "Fixing permissions and removing quarantine flags..."

chmod -R 755 "$BUILD_DIR"
xattr -rc "$BUILD_DIR"
xattr -dr com.apple.quarantine "$BUILD_DIR"

echo "Permissions and quarantine flags fixed."

# Stap 5: Sign de app
echo "Signing the app..."

codesign --force --deep --sign - "$BUILD_DIR"

echo "Finished signing the app."

echo "Done building the game."