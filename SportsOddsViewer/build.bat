@echo off
echo ========================================
echo Sports Odds Viewer - Build Script
echo ========================================
echo.

echo [1/3] Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo Error: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo [2/3] Building project...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo Error: Build failed
    pause
    exit /b 1
)

echo.
echo [3/3] Build completed successfully!
echo.
echo To run the application, execute: run.bat
echo Or run: dotnet run
echo.
pause
