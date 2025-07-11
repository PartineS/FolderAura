# FolderAura - Desktop Folder Customization Application

## 📂 Overview
FolderAura is an application that allows you to change the appearance of folders on your Windows desktop. With this program you can:
- Add custom icons to folders
- Apply modern Windows 11 effects (blur, transparency) to folder windows
- Add color labels to folders

**IMPORTANT**: This application only makes visual changes, it does not touch your folder contents.

## ✨ What Can It Do?
- **Change Folder Icons**: You can change the icon of any folder with any image you want
- **Modern Effects**: You can add blur and transparency effects to folder windows
- **Color System**: You can assign color codes to folders (red=important, green=completed, etc.)
- **Save Settings**: Your changes are automatically saved
- **Undo**: You can revert to the original state anytime

## 🔧 System Requirements
- **Windows 10** (version 1809 or higher) or **Windows 11**
- **.NET 8.0 Runtime** (will be installed automatically if missing)
- **At least 1GB free disk space**
- **Administrator privileges** (required for folder modifications)

## 📥 Installation

## � INSTALLATION - STEP BY STEP

### Method 1: Automatic Installation (RECOMMENDED)

1. **Download files**: Copy the `FolderAura` folder to your desktop
2. **Find installation file**: Locate the `install.cmd` file
3. **Run as administrator**:
   - **RIGHT-CLICK** on `install.cmd` file
   - Select **"Run as administrator"** option
   - Click **"Yes"** on Windows UAC warning
4. **Follow installation wizard**:
   - Installation will start automatically
   - Type **"Y"** if asked for desktop shortcut
   - Press **Enter** when installation completes

### Method 2: Manual Installation

1. **Copy application file**: Copy `FolderAura.exe` to your desired folder
2. **Always run as administrator**:
   - **RIGHT-CLICK** on `FolderAura.exe`
   - Select **"Run as administrator"**

## 🚀 HOW TO USE THE APPLICATION?

### First Launch
1. **From Start Menu**: 
   - Press **Windows key** + **S**
   - Type **"FolderAura"**
   - Click on the result

2. **Or From Desktop**:
   - Double-click on **FolderAura** icon on desktop

### Main Usage

#### Changing Folder Icon:
1. Click **"Browse Folder"** button
2. Select the folder you want to change (e.g., "Documents" on Desktop)
3. Click **"Browse Icon"** button
4. Select the image you want to use (.ico, .png, .jpg files)
5. Click **"Apply Changes"** button
6. **Refresh File Explorer** (F5 key)

#### Applying Effects:
1. Select a folder
2. **Choose effect type**:
   - **Mica**: Modern Windows 11 blur effect
   - **Acrylic**: Semi-transparent glass effect
   - **Blur**: Simple blur effect
3. **Adjust transparency level** (0-100)
4. Click **"Apply Changes"** button

#### Adding Color:
1. Select a folder
2. Click **"Color Picker"** button
3. Select a color from the list
4. Click **"Apply Changes"** button

## ⚙️ Settings and Features

### Settings File Location
- Your settings are stored at: `C:\Users\[Username]\AppData\Local\FolderAura\`
- Deleting this folder resets all settings

### Resetting Settings
- Click **"Reset Settings"** button in the application
- Or delete the folder above

### Backup
- You can backup your settings by copying the settings file elsewhere
- `settings.json` file contains all your settings

## 🔧 TROUBLESHOOTING

### "Application won't start" Problem
**Solution 1**: Check Administrator Permissions
- **RIGHT-CLICK** on application → **"Run as administrator"**
- Click **"Yes"** on Windows UAC warning

**Solution 2**: Check .NET Runtime
- Press **Windows + R** keys
- Type **"cmd"** and press **Enter**
- Type **"dotnet --version"**
- If it gives error, [download Microsoft .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

### "Folder changes not visible" Problem
**Solution 1**: Refresh Explorer
- Press **F5** key in folder window
- Or press **Ctrl + F5**

**Solution 2**: Restart Windows Explorer
- Press **Ctrl + Shift + Esc** (Task Manager)
- Find **"Windows Explorer"**
- **Right-click** → **"Restart"**

**Solution 3**: Restart Computer
- Changes sometimes appear after restart

### "Icon not showing" Problem
**Solution 1**: Use ICO Format
- Use **.ico** extension files instead of PNG/JPEG
- Convert your image to .ico using online ICO converters

**Solution 2**: Check Icon Size
- Very large files can cause problems
- Use 256x256 pixels or smaller

### "Access denied" Error
**Solution**: Check Folder Permissions
- You must have **write permission** to the folder you're trying to change
- Don't try to change system folders (Windows, Program Files)

## 🗑️ UNINSTALLATION

### Automatic Uninstallation (If installed)
1. Open **Control Panel**
2. Go to **"Add or remove programs"**
3. Find **"FolderAura"** and click **"Uninstall"**
4. Or run **"uninstall.cmd"** file in installation folder **as administrator**

### Manual Uninstallation
1. Close the application
2. Delete `FolderAura.exe` file
3. To clean settings: **Windows + R** → **`%LOCALAPPDATA%`** → Delete **FolderAura** folder

## 🔒 Security and Privacy
- ✅ Only makes changes to selected folders
- ✅ Does not read or modify your files
- ✅ Does not use internet connection
- ✅ Does not collect personal information
- ✅ Contains no viruses/malware

## � Support and Contact
- **If you have problems**: Use GitHub Issues section
- **For suggestions**: Also write to GitHub Issues
- **Quick help**: Read the troubleshooting section in README file

## 📜 License
This application is **completely free** and for personal use.

---
**FolderAura v1.0.0** - © 2025 FolderAura Team

**Last Update**: July 11, 2025
