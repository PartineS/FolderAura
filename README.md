# FolderAura - Desktop Folder Customization Application

![FolderAura Logo](https://img.shields.io/badge/FolderAura-v1.0.0-blue.svg)
![Windows](https://img.shields.io/badge/Windows-10%2F11-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![License](https://img.shields.io/badge/License-Free-green.svg)

## 🌟 Overview

**FolderAura** is a powerful Windows desktop application that allows you to customize the appearance of your folders with modern Windows 11 effects, custom icons, and color coding.

### ✨ Key Features
- 🎨 **Custom Folder Icons** - Change any folder icon with your preferred images (ICO/PNG/JPEG)
- 🌪️ **Modern Windows 11 Effects** - Apply Mica, Acrylic, and Blur effects to folder windows
- 🎯 **Color Coding System** - Organize folders with color labels for better productivity
- ⚙️ **Auto-Save Settings** - Your customizations are automatically saved and restored
- 🔄 **Easy Undo** - Revert changes anytime with one click
- 🛡️ **Safe & Secure** - Only modifies visual aspects, never touches file contents

## 📥 Quick Start

### Download & Install
1. Download the latest release from [Releases](https://github.com/PartineS/FolderAura/releases)
2. Extract the files to your desired location
3. Run `install.cmd` as **Administrator**
4. Launch from Start Menu → "FolderAura"

### Usage
1. **Select Folder** → Choose the folder you want to customize
2. **Pick Icon** → Select your preferred image file
3. **Choose Effect** → Apply Mica, Acrylic, or Blur effects
4. **Set Color** → Pick a color for organization
5. **Apply** → See changes instantly!

## 🎯 System Requirements
- **OS**: Windows 10 (1809+) or Windows 11
- **Runtime**: .NET 8.0 (auto-installs if missing)
- **Permissions**: Administrator privileges required
- **Storage**: ~10MB disk space

## 📖 Documentation
- 🇹🇷 [Turkish Documentation](README_TR.md) - Detaylı Türkçe kullanım kılavuzu
- 🇺🇸 [English Documentation](README_EN.md) - Comprehensive English guide

## 🛠️ For Developers

### Build from Source
```powershell
git clone https://github.com/PartineS/FolderAura.git
cd FolderAura
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
├── MainWindow.xaml        # UI design
└── App.xaml              # Application entry point
```

### Technologies Used
- **Framework**: .NET 8.0 WPF
- **UI Library**: ModernWPF (Windows 11 styling)
- **Windows APIs**: Vanara.PInvoke (DwmApi, Shell32, User32)
- **Configuration**: System.Text.Json

## 🔒 Security & Privacy
- ✅ No internet connection required
- ✅ No personal data collection
- ✅ Only modifies folder appearance
- ✅ Open source and transparent
- ✅ Virus-free (Windows Defender approved)

## 🐛 Troubleshooting
- **App won't start?** → Run as Administrator
- **Changes not visible?** → Press F5 to refresh Explorer
- **Icons not showing?** → Use .ICO format for best compatibility

For detailed troubleshooting, see the documentation files.

## 📸 Screenshots

*Screenshots will be added in the next release*

## 📜 License
This project is **free for personal use**. See detailed documentation for terms.

## 🤝 Contributing
Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## 📞 Support
- 🐛 **Bug Reports**: [GitHub Issues](https://github.com/PartineS/FolderAura/issues)
- 💡 **Feature Requests**: [GitHub Issues](https://github.com/PartineS/FolderAura/issues)
- ❓ **Questions**: Check documentation first, then create an issue

## 🗂️ Download

### Latest Release (v1.0.0)
- **Size**: ~8MB
- **Format**: Portable application + installer
- **Languages**: Turkish + English

[📥 Download Latest Release](https://github.com/PartineS/FolderAura/releases/latest)

---

**Made with ❤️ for Windows users who love customization**

*FolderAura v1.0.0 - © 2025 FolderAura Team*
