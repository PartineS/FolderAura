using System;

namespace FolderAura.Models
{
    public class FolderSettings
    {
        public string FolderPath { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public string? Label { get; set; }
        public string? BorderColor { get; set; }
        public string EffectType { get; set; } = "None";
        public DateTime LastModified { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }

    public class ApplicationSettings
    {
        public string? LastSelectedFolder { get; set; }
        public string Theme { get; set; } = "Auto";
        public bool AutoSaveSettings { get; set; } = true;
        public bool ShowNotifications { get; set; } = true;
        public string SettingsVersion { get; set; } = "1.0";
    }

    public class EffectResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string EffectType { get; set; } = "None";
    }

    public class EffectSupportInfo
    {
        public bool IsSupported { get; set; }
        public string Message { get; set; } = string.Empty;
        public string RequiredVersion { get; set; } = string.Empty;
    }
}
