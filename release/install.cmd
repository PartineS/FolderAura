@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo.
echo ╔══════════════════════════════════════════════════════════════╗
echo ║                    FolderAura Installer                     ║
echo ║                 Masaüstü Klasör Özelleştirme                ║
echo ╚══════════════════════════════════════════════════════════════╝
echo.

:: Yönetici kontrolü
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Bu script yönetici olarak çalıştırılmalıdır!
    echo    Sağ tık → "Yönetici olarak çalıştır" seçin.
    echo.
    pause
    exit /b 1
)

echo ✅ Yönetici izinleri doğrulandı.

:: Kurulum dizini
set "INSTALL_DIR=%ProgramFiles%\FolderAura"
set "START_MENU=%ProgramData%\Microsoft\Windows\Start Menu\Programs"
set "DESKTOP=%PUBLIC%\Desktop"

echo.
echo 📁 Kurulum dizini: %INSTALL_DIR%
echo.

:: Dizin oluştur
if not exist "%INSTALL_DIR%" (
    mkdir "%INSTALL_DIR%" 2>nul
    if !errorlevel! neq 0 (
        echo ❌ Kurulum dizini oluşturulamadı!
        pause
        exit /b 1
    )
)

:: Dosyaları kopyala
echo 📋 Dosyalar kopyalanıyor...
copy /Y "FolderAura.exe" "%INSTALL_DIR%\" >nul
copy /Y "README_TR.md" "%INSTALL_DIR%\" >nul
copy /Y "README_EN.md" "%INSTALL_DIR%\" >nul

if not exist "%INSTALL_DIR%\FolderAura.exe" (
    echo ❌ Ana dosya kopyalanamadı!
    pause
    exit /b 1
)

:: Başlat menüsü kısayolu
echo 🔗 Başlat menüsü kısayolu oluşturuluyor...
set "SHORTCUT_VBS=%TEMP%\create_shortcut.vbs"
(
echo Set objShell = CreateObject("WScript.Shell"^)
echo Set objShortcut = objShell.CreateShortcut("%START_MENU%\FolderAura.lnk"^)
echo objShortcut.TargetPath = "%INSTALL_DIR%\FolderAura.exe"
echo objShortcut.WorkingDirectory = "%INSTALL_DIR%"
echo objShortcut.Description = "Masaüstü Klasör Özelleştirme Uygulaması"
echo objShortcut.Save
) > "%SHORTCUT_VBS%"
cscript //nologo "%SHORTCUT_VBS%" >nul
del "%SHORTCUT_VBS%" >nul 2>&1

:: Masaüstü kısayolu (opsiyonel)
echo.
set /p "CREATE_DESKTOP=🖥️  Masaüstünde kısayol oluşturulsun mu? (E/H): "
if /i "!CREATE_DESKTOP!"=="E" (
    set "DESKTOP_VBS=%TEMP%\create_desktop_shortcut.vbs"
    (
    echo Set objShell = CreateObject("WScript.Shell"^)
    echo Set objShortcut = objShell.CreateShortcut("%DESKTOP%\FolderAura.lnk"^)
    echo objShortcut.TargetPath = "%INSTALL_DIR%\FolderAura.exe"
    echo objShortcut.WorkingDirectory = "%INSTALL_DIR%"
    echo objShortcut.Description = "Masaüstü Klasör Özelleştirme Uygulaması"
    echo objShortcut.Save
    ) > "!DESKTOP_VBS!"
    cscript //nologo "!DESKTOP_VBS!" >nul
    del "!DESKTOP_VBS!" >nul 2>&1
    echo ✅ Masaüstü kısayolu oluşturuldu.
)

:: Kayıt defteri girişleri (Kaldır/Değiştir için)
echo 📝 Sistem kaydı yapılıyor...
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /v "DisplayName" /t REG_SZ /d "FolderAura" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /v "DisplayVersion" /t REG_SZ /d "1.0.0" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /v "Publisher" /t REG_SZ /d "FolderAura Team" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /v "InstallLocation" /t REG_SZ /d "%INSTALL_DIR%" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /v "UninstallString" /t REG_SZ /d "%INSTALL_DIR%\uninstall.cmd" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /v "DisplayIcon" /t REG_SZ /d "%INSTALL_DIR%\FolderAura.exe" /f >nul

:: Kaldırma scripti oluştur
echo 📄 Kaldırma scripti oluşturuluyor...
(
echo @echo off
echo chcp 65001 ^>nul
echo setlocal enabledelayedexpansion
echo.
echo echo.
echo echo ╔══════════════════════════════════════════════════════════════╗
echo echo ║                   FolderAura Uninstaller                    ║
echo echo ║                 Masaüstü Klasör Özelleştirme                ║
echo echo ╚══════════════════════════════════════════════════════════════╝
echo echo.
echo.
echo :: Yönetici kontrolü
echo net session ^>nul 2^>^&1
echo if %%errorlevel%% neq 0 ^(
echo     echo ❌ Bu script yönetici olarak çalıştırılmalıdır!
echo     echo    Sağ tık → "Yönetici olarak çalıştır" seçin.
echo     echo.
echo     pause
echo     exit /b 1
echo ^)
echo.
echo echo ⚠️  FolderAura uygulaması kaldırılacak!
echo echo.
echo set /p "CONFIRM=Devam etmek istiyor musunuz? (E/H): "
echo if /i "!CONFIRM!" neq "E" ^(
echo     echo ❌ İşlem iptal edildi.
echo     pause
echo     exit /b 0
echo ^)
echo.
echo echo 🗑️  Dosyalar siliniyor...
echo.
echo :: Başlat menüsü kısayolu
echo del "%START_MENU%\FolderAura.lnk" ^>nul 2^>^&1
echo.
echo :: Masaüstü kısayolu
echo del "%PUBLIC%\Desktop\FolderAura.lnk" ^>nul 2^>^&1
echo.
echo :: Kayıt defteri
echo reg delete "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FolderAura" /f ^>nul 2^>^&1
echo.
echo :: Kullanıcı ayarları ^(opsiyonel^)
echo set /p "DELETE_SETTINGS=🔧 Kullanıcı ayarları da silinsin mi? (E/H): "
echo if /i "!DELETE_SETTINGS!"=="E" ^(
echo     rmdir /s /q "%%LOCALAPPDATA%%\FolderAura" ^>nul 2^>^&1
echo     echo ✅ Kullanıcı ayarları silindi.
echo ^) else ^(
echo     echo ℹ️  Kullanıcı ayarları korundu: %%LOCALAPPDATA%%\FolderAura
echo ^)
echo.
echo :: Ana dosyalar ^(kendini sil^)
echo cd /d "%%TEMP%%"
echo timeout /t 2 /nobreak ^>nul
echo rmdir /s /q "%INSTALL_DIR%" ^>nul 2^>^&1
echo.
echo echo ✅ FolderAura başarıyla kaldırıldı!
echo echo.
echo pause
) > "%INSTALL_DIR%\uninstall.cmd"

echo.
echo ✅ Kurulum tamamlandı!
echo.
echo 📍 Program konumu: %INSTALL_DIR%
echo 🚀 Başlat menüsünden "FolderAura" arayarak başlatabilirsiniz
echo 💡 İlk kullanımda yönetici izni istenecektir
echo.
echo Kurulum başarılı! 🎉
echo.
pause
