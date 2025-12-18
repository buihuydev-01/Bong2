@echo off
echo ============================================
echo   BETTING ODDS DISPLAY - BUILD AND RUN
echo ============================================
echo.

echo [1] Cleaning old files...
if exist obj rmdir /s /q obj
if exist bin rmdir /s /q bin
echo Done.
echo.

echo [2] Restoring packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore!
    pause
    exit /b 1
)
echo Done.
echo.

echo [3] Building project...
dotnet build -c Debug
if %errorlevel% neq 0 (
    echo ERROR: Build failed!
    pause
    exit /b 1
)
echo Done.
echo.

echo [4] Running application...
echo.
echo *** WATCH THE CONSOLE OUTPUT BELOW ***
echo.
dotnet run

pause
