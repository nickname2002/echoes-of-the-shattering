#!/bin/bash

# Configuratie
APP_NAME="EchoesOfTheShattering"
VERSION="1.0.0"
IDENTIFIER="com.nickjordan.echoesoftheshattering"
PLATFORM="osx-arm64"
BUILD_DIR="$APP_NAME.app"
PKG_NAME="$APP_NAME-$VERSION.pkg"
ICON_PATH="AppIcon.icns"

echo "Building the game..."

# Stap 1: Build the game
dotnet publish -c Release -r osx-arm64 --self-contained -o ./EchoesOfTheShattering.app/Contents/MacOS

# Stap 2: Maak de binary uitvoerbaar
chmod +x EchoesOfTheShattering.app/Contents/MacOS/MonoZenith

echo "Finished building the game."

# Stap 3: Maak de Info.plist
echo "Creating the Info.plist file..."

mkdir -p "$BUILD_DIR/Contents"
cat <<EOF > "$BUILD_DIR/Contents/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
    <dict>
        <!-- Verwijst naar de uitvoerbare binary -->
        <key>CFBundleExecutable</key>
        <string>MonoZenith</string>

        <!-- Unieke identifier voor de app -->
        <key>CFBundleIdentifier</key>
        <string>com.nickjordan.EchoesOfTheShattering</string>

        <!-- App-naam -->
        <key>CFBundleName</key>
        <string>Echoes of the Shattering</string>

        <!-- App-versie -->
        <key>CFBundleVersion</key>
        <string>1.0.0</string>

        <key>CFBundleShortVersionString</key>
        <string>1.0.0</string>

        <!-- Verwijzing naar het icoon -->
        <key>CFBundleIconFile</key>
        <string>AppIcon.icns</string>

        <!-- Retina-ondersteuning -->
        <key>NSHighResolutionCapable</key>
        <true/>
    </dict>
</plist>
EOF

echo "Finished creating the Info.plist file."

# TODO: Package the standalone app into a .pkg file

echo "Finished the packaging process."
