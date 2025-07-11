# FolderAura

FolderAura is a powerful Windows desktop application that allows you to customize your folders with modern visual effects and custom icons.

## Features

• **Custom Folder Icons** - PNG, ICO, SVG support
• **Modern Windows 11 Effects** - Acrylic, blur, and transparency effects  
• **Easy Installation** - One-click install and uninstall
• **Turkish & English** - Full language support
• **Safe & Secure** - Only visual modifications

## Quick Start

1. Download from [Releases](https://github.com/PartineS/FolderAura/releases)
2. Run `install.cmd` as **Administrator**
3. Launch FolderAura from Start Menu

## Usage

1. Select folder to customize
2. Choose icon (PNG, ICO, SVG)
3. Apply effects and transparency
4. Click "Apply"

## System Requirements

- Windows 10/11 (64-bit)
- .NET 8.0 Runtime
- Administrator privileges

## Documentation

- 🇹🇷 [Turkish Guide](README_TR.md)
- 🇺🇸 [English Guide](README_EN.md)
dotnet restore
dotnet build -c Release
```

### Project Structure
```
FolderAura/
├── Core/                  # Core functionality
│   ├── FolderManager.cs   # Folder operations
│   ├── IconManager.cs     # Icon management
│   ├── TransparencyManager.cs # Windows effects
│   └── SettingsManager.cs # Configuration
├── Models/                # Data models
├── release/               # Ready-to-use binaries
---

© 2025 FolderAura
