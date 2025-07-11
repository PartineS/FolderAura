@echo off
echo.
echo =================================
echo   FolderAura Kaldirma
echo =================================
echo.

:: Check if running as administrator
openfiles >nul 2>&1
if %errorlevel% neq 0 (
    echo Bu islem yonetici yetkisi gerektirir.
    echo Lutfen "Yonetici olarak calistir" secenegini kullanin.
    pause
    exit /b 1
)

set "INSTALL_DIR=%ProgramFiles%\FolderAura"

echo FolderAura kapatiliyor...
taskkill /f /im FolderAura.exe >nul 2>&1

echo Kisayollar siliniyor...
del /f /q "%ProgramData%\Microsoft\Windows\Start Menu\Programs\FolderAura.lnk" >nul 2>&1
del /f /q "%USERPROFILE%\Desktop\FolderAura.lnk" >nul 2>&1

echo Program dosyalari siliniyor...
if exist "%INSTALL_DIR%" (
    rmdir /s /q "%INSTALL_DIR%"
)

echo.
echo =================================
echo   Kaldirma Tamamlandi!
echo =================================
echo.
pause
