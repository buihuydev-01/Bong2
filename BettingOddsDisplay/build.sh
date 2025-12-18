#!/bin/bash
# Build script for BettingOddsDisplay (Linux/Mac)

echo "========================================"
echo "  Betting Odds Display - Build Script"
echo "========================================"
echo ""

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found!"
    echo "Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo "[1/5] Checking .NET SDK version..."
dotnet --version
echo ""

echo "[2/5] Cleaning previous builds..."
dotnet clean
echo ""

echo "[3/5] Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore packages!"
    exit 1
fi
echo ""

echo "[4/5] Building project (Debug)..."
dotnet build -c Debug
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed!"
    exit 1
fi
echo ""

echo "[5/5] Build completed successfully!"
echo ""
echo "Output directory: bin/Debug/net8.0-windows/"
echo ""
echo "To run the application:"
echo "  dotnet run"
echo ""
echo "To build Release version:"
echo "  dotnet build -c Release"
echo ""
echo "To publish standalone executable:"
echo "  dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true"
echo ""
