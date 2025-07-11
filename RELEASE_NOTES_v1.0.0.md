# FolderAura v1.0.0 Release Notes

## 🎉 Initial Release - July 11, 2025

Welcome to **FolderAura v1.0.0**, the first stable release of our Windows desktop folder customization application!

### ✨ New Features

#### 🎨 Folder Customization
- **Custom Icons**: Change any folder icon with ICO, PNG, or JPEG images
- **Modern Effects**: Apply Windows 11 Mica, Acrylic, and Blur effects to folder windows
- **Color Coding**: Organize folders with predefined color labels
- **Real-time Preview**: See changes instantly before applying

#### ⚙️ Settings & Management
- **Auto-Save Configuration**: Your settings are automatically saved and restored
- **JSON-based Storage**: Lightweight and portable settings management
- **Easy Reset**: One-click reset to default settings
- **Backup Support**: Export/import your customizations

#### 🛡️ Security & Reliability
- **Administrator Privileges**: Secure folder modification with proper permissions
- **Safe Operations**: Only modifies visual aspects, never touches file contents
- **No Network Access**: Completely offline application
- **Memory Optimized**: Minimal resource usage (~50MB RAM)

### 🛠️ Technical Specifications

#### System Requirements
- **Operating System**: Windows 10 (1809+) or Windows 11
- **.NET Runtime**: .NET 8.0 (automatically installed if missing)
- **Permissions**: Administrator privileges required
- **Storage**: ~10MB disk space
- **Memory**: ~50MB RAM usage

#### Technologies Used
- **.NET 8.0 WPF**: Core application framework
- **ModernWPF**: Windows 11-style UI components
- **Vanara.PInvoke**: Windows API integration (DwmApi, Shell32, User32)
- **System.Text.Json**: Configuration management

### 📦 What's Included

#### Download Package Contents
```
FolderAura-v1.0.0-Final.zip (2.5MB)
├── FolderAura.exe         # Main application (8MB)
├── install.cmd            # Automatic installer script
├── uninstall.cmd          # Uninstaller script
├── README.md              # Quick start guide
├── README_TR.md           # Turkish documentation
└── README_EN.md           # English documentation
```

### 🚀 Installation Options

#### Option 1: Automatic Installation (Recommended)
1. Extract the ZIP file
2. Right-click `install.cmd` → "Run as administrator"
3. Follow the installation wizard
4. Launch from Start Menu → "FolderAura"

#### Option 2: Portable Usage
1. Extract the ZIP file to your preferred location
2. Right-click `FolderAura.exe` → "Run as administrator"
3. Use directly without installation

### 🎯 Key Improvements

#### Performance Optimizations
- **Size Reduction**: 88% smaller than initial builds (8MB vs 70MB)
- **Memory Efficiency**: Aggressive garbage collection and resource management
- **Fast Startup**: Optimized initialization and loading
- **No Dependencies**: Self-contained with framework-dependent deployment

#### User Experience
- **Intuitive Interface**: Modern Windows 11-style UI
- **Step-by-step Documentation**: Comprehensive guides in Turkish and English
- **Error Handling**: Detailed troubleshooting guides
- **Accessibility**: Clear visual feedback and status messages

### 🐛 Known Issues & Limitations

#### Current Limitations
- **Icon Formats**: ICO format recommended for best compatibility
- **System Folders**: Cannot modify Windows system directories (by design)
- **Explorer Refresh**: Manual refresh (F5) may be needed to see changes
- **Windows 10**: Some effects may have limited compatibility on older Windows 10 versions

#### Workarounds
- **PNG/JPEG Icons**: Convert to ICO format using online converters
- **Changes Not Visible**: Press F5 or restart Windows Explorer
- **Permission Errors**: Ensure running as Administrator

### 🔮 Future Roadmap

#### Planned Features (v1.1.0)
- **Drag & Drop**: Direct icon assignment via drag and drop
- **Batch Operations**: Apply settings to multiple folders at once
- **Custom Effects**: User-defined transparency and blur levels
- **Icon Library**: Built-in icon collection
- **Themes**: Predefined customization themes

#### Long-term Goals
- **Context Menu Integration**: Right-click folder customization
- **Live Wallpaper Support**: Dynamic folder backgrounds
- **Cloud Sync**: Sync settings across devices
- **Plugin System**: Third-party extensions support

### 📞 Support & Community

#### Getting Help
- **Documentation**: Check README files for detailed guides
- **GitHub Issues**: Report bugs and request features
- **Troubleshooting**: Comprehensive problem-solving guides included

#### Contributing
- **Source Code**: Available on GitHub for contributors
- **Feature Requests**: Submit ideas via GitHub Issues
- **Bug Reports**: Help us improve with detailed reports

### 🏆 Special Thanks

Special thanks to the Windows developer community and all the beta testers who helped make this release possible!

---

**Download FolderAura v1.0.0**: [Release Assets Below]

**Full Changelog**: Initial release

**Minimum Requirements**: Windows 10 1809+ | .NET 8.0 | Administrator privileges
