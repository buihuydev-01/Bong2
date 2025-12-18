@echo off
REM Build Release và tạo standalone executable

echo ========================================
echo   Build Release - Standalone EXE
echo ========================================
echo.

REM Check dotnet
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found!
    pause
    exit /b 1
)

echo [1/4] Cleaning...
dotnet clean
echo.

echo [2/4] Restoring packages...
dotnet restore
echo.

echo [3/4] Publishing Release (win-x64, self-contained, single file)...
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if %errorlevel% neq 0 (
    echo ERROR: Publish failed!
    pause
    exit /b 1
)
echo.

echo [4/4] Build completed!
echo.
echo Executable location:
echo   bin\Release\net8.0-windows\win-x64\publish\BettingOddsDisplay.exe
echo.
echo File size: 
dir "bin\Release\net8.0-windows\win-x64\publish\BettingOddsDisplay.exe" | find "BettingOddsDisplay.exe"
echo.
echo You can now distribute this single .exe file!
echo.

pause
