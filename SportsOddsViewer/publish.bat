@echo off
echo ========================================
echo Sports Odds Viewer - Publish Script
echo ========================================
echo.

echo Publishing application for Windows x64...
echo This will create a standalone executable.
echo.

dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

if %errorlevel% neq 0 (
    echo.
    echo Error: Publish failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Publish completed successfully!
echo ========================================
echo.
echo The executable file is located at:
echo bin\Release\net8.0-windows\win-x64\publish\SportsOddsViewer.exe
echo.
echo You can copy this .exe file to any Windows computer and run it.
echo.
pause
