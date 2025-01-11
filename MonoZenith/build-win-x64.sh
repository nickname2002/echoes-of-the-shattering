#!/bin/bash

# Configuratie
APP_NAME="Echoes of the Shattering"
VERSION="1.0.0"
PUBLISH_DIR="./publish"
INSTALLER_SCRIPT="EchoesOfTheShatteringInstaller.iss"
ICON_FILE="Icon.ico"

echo "🔨 1. Bouwen van de Windows-versie..."

# Stap 1: Build de game voor Windows
dotnet publish -c Release -r win-x64 --self-contained -o "$PUBLISH_DIR"

if [ $? -ne 0 ]; then
    echo "❌ Build mislukt."
    exit 1
fi

echo "✅ Build succesvol."

# Stap 2: Controleer of Inno Setup geïnstalleerd is
echo "🔍 2. Controleren of Inno Setup is geïnstalleerd..."

if ! command -v iscc &> /dev/null; then
    echo "❌ Inno Setup Compiler (ISCC) is niet gevonden."
    echo "➡️  Download en installeer Inno Setup via https://jrsoftware.org/isinfo.php"
    exit 1
fi

echo "✅ Inno Setup is beschikbaar."

# Stap 3: Installer script aanmaken
echo "📝 3. Installer script genereren..."

cat <<EOF > "$INSTALLER_SCRIPT"
; Installer script voor $APP_NAME

[Setup]
AppName=$APP_NAME
AppVersion=$VERSION
DefaultDirName={autopf}\\$APP_NAME
DefaultGroupName=$APP_NAME
OutputBaseFilename=${APP_NAME// /}Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=$ICON_FILE

[Files]
Source: "$PUBLISH_DIR\\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\\$APP_NAME"; Filename: "{app}\\MonoZenith.exe"
Name: "{commondesktop}\\$APP_NAME"; Filename: "{app}\\MonoZenith.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"

[Run]
Filename: "{app}\\MonoZenith.exe"; Description: "Start $APP_NAME"; Flags: nowait postinstall skipifsilent
EOF

echo "✅ Installer script aangemaakt."

# Stap 4: Installer genereren met Inno Setup
echo "📦 4. Bouwen van de installer..."

iscc "$INSTALLER_SCRIPT"

if [ $? -ne 0 ]; then
    echo "❌ Installer build mislukt."
    exit 1
fi

echo "✅ Installer succesvol aangemaakt."

# Stap 5: Opruimen
echo "🧹 5. Opruimen van tijdelijke bestanden..."

rm "$INSTALLER_SCRIPT"

echo "🎉 Klaar! De installer staat in de Output-map."