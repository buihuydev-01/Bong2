@echo off
echo ========================================
echo Sports Odds Viewer - Starting...
echo ========================================
echo.

dotnet run

if %errorlevel% neq 0 (
    echo.
    echo Error: Failed to start application
    echo Please make sure you have built the project first (run build.bat)
    pause
)
