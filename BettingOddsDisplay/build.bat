@echo off
REM Build script cho BettingOddsDisplay

echo ========================================
echo   Betting Odds Display - Build Script
echo ========================================
echo.

REM Check if dotnet is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found!
    echo Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo [1/5] Checking .NET SDK version...
dotnet --version
echo.

echo [2/5] Cleaning previous builds...
dotnet clean
echo.

echo [3/5] Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages!
    pause
    exit /b 1
)
echo.

echo [4/5] Building project (Debug)...
dotnet build -c Debug
if %errorlevel% neq 0 (
    echo ERROR: Build failed!
    pause
    exit /b 1
)
echo.

echo [5/5] Build completed successfully!
echo.
echo Output directory: bin\Debug\net8.0-windows\
echo.
echo To run the application:
echo   dotnet run
echo.
echo To build Release version:
echo   dotnet build -c Release
echo.
echo To publish standalone executable:
echo   dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
echo.

pause
