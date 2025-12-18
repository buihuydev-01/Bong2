#!/bin/bash
# Build Release and create standalone executable (Linux/Mac)

echo "========================================"
echo "  Build Release - Standalone EXE"
echo "========================================"
echo ""

# Check dotnet
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found!"
    exit 1
fi

echo "[1/4] Cleaning..."
dotnet clean
echo ""

echo "[2/4] Restoring packages..."
dotnet restore
echo ""

echo "[3/4] Publishing Release (win-x64, self-contained, single file)..."
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if [ $? -ne 0 ]; then
    echo "ERROR: Publish failed!"
    exit 1
fi
echo ""

echo "[4/4] Build completed!"
echo ""
echo "Executable location:"
echo "  bin/Release/net8.0-windows/win-x64/publish/BettingOddsDisplay.exe"
echo ""
ls -lh "bin/Release/net8.0-windows/win-x64/publish/BettingOddsDisplay.exe"
echo ""
echo "You can now distribute this single .exe file!"
echo ""
