#!/bin/bash

# Configuratie
APP_NAME="EchoesOfTheShattering"
VERSION="1.0.0"
IDENTIFIER="com.nickjordan.echoesoftheshattering"
BUILD_DIR="$APP_NAME.app"
ICON_PATH="AppIcon.icns"
PUBLISH_DIR="./publish"
OUTPUT_ZIP="$APP_NAME.zip"

# Foutafhandelingsfunctie
handle_error() {
    echo "❌ $1"
    exit 1
}

echo "🔨 1. Building the game..."

# Stap 1: Build de game voor zowel arm64 als x64
dotnet publish -c Release -r osx-x64 --self-contained -o "$PUBLISH_DIR/x64" || handle_error "Failed to build for osx-x64"
dotnet publish -c Release -r osx-arm64 --self-contained -o "$PUBLISH_DIR/arm64" || handle_error "Failed to build for osx-arm64"

# Stap 2: Maak de binary uitvoerbaar
chmod +x "$PUBLISH_DIR/x64/MonoZenith"
chmod +x "$PUBLISH_DIR/arm64/MonoZenith"

echo "✅ Game successfully built for both osx-x64 and osx-arm64."

# Stap 3: Maak de Info.plist
echo "📄 2. Creating the Info.plist file..."
mkdir -p "$BUILD_DIR/Contents/MacOS"
mkdir -p "$BUILD_DIR/Contents/Resources"
cat <<EOF > "$BUILD_DIR/Contents/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
    <dict>
        <key>CFBundleExecutable</key>
        <string>MonoZenith</string>
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

echo "✅ Info.plist created."

# Stap 5: Verplaats de gepubliceerde bestanden naar de app-structuur
echo "📦 4. Moving published files to app structure..."
mkdir -p "$BUILD_DIR/Contents/MacOS"
cp -R "$PUBLISH_DIR/x64"/* "$BUILD_DIR/Contents/MacOS/" || handle_error "Failed to copy x64 build files."
cp -R "$PUBLISH_DIR/arm64"/* "$BUILD_DIR/Contents/MacOS/" || handle_error "Failed to copy arm64 build files."

# Stap 6: Verwijder quarantine flags en pas permissies aan
echo "🔐 5. Fixing permissions and removing quarantine flags..."
chmod -R 755 "$BUILD_DIR"
xattr -rc "$BUILD_DIR" || handle_error "Failed to remove quarantine flags."

# Stap 7: Code signing (indien je een Developer ID hebt)
#echo "🔏 6. Signing the app..."
#codesign --force --deep --sign "Developer ID Application: YourName (YourID)" "$BUILD_DIR" || handle_error "Failed to sign the app."
#
#echo "✅ App successfully signed."

# Stap 8: Zip de app voor distributie
echo "📦 7. Zipping the app for distribution..."
ditto -c -k --sequesterRsrc --keepParent "$BUILD_DIR" "$OUTPUT_ZIP" || handle_error "Failed to zip the app."

echo "✅ App successfully zipped as $OUTPUT_ZIP."

# Eindigen
echo "🎉 Build process completed successfully! The .app and .zip are ready for distribution."