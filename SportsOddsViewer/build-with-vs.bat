@echo off
echo ========================================
echo Sports Odds Viewer - Visual Studio Build
echo ========================================
echo.

echo This script will help you build the project using Visual Studio.
echo.

echo [INFO] Solution file: SportsOddsViewer.sln
echo.

echo To build this project:
echo.
echo Option 1: Using Visual Studio
echo   1. Open SportsOddsViewer.sln in Visual Studio
echo   2. Press Ctrl+Shift+B to build
echo   3. Press F5 to run with debugging (or Ctrl+F5 without debugging)
echo.

echo Option 2: Using MSBuild (Command Line)
echo   Run: msbuild SportsOddsViewer.sln /p:Configuration=Release
echo.

echo Option 3: Using dotnet CLI
echo   Run: dotnet build SportsOddsViewer.sln
echo   Run: dotnet run --project SportsOddsViewer.csproj
echo.

echo ========================================
echo.
pause
