using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;

namespace FolderAura.Core
{
    public class IconManager
    {
        private const string DESKTOP_INI = "desktop.ini";
        private const string FOLDER_TYPE_GENERIC = "Generic";

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private const uint SHCNE_ASSOCCHANGED = 0x08000000;
        private const uint SHCNF_IDLIST = 0x0000;

        public void SetFolderIcon(string folderPath, string iconPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                throw new ArgumentException("Geçersiz klasör yolu", nameof(folderPath));

            if (string.IsNullOrEmpty(iconPath) || !File.Exists(iconPath))
                throw new ArgumentException("Geçersiz ikon dosyası", nameof(iconPath));

            try
            {
                var desktopIniPath = Path.Combine(folderPath, DESKTOP_INI);

                // desktop.ini içeriğini oluştur
                var iniContent = CreateDesktopIniContent(iconPath);

                // desktop.ini dosyasını oluştur/güncelle
                File.WriteAllText(desktopIniPath, iniContent, Encoding.UTF8);

                // Dosya özniteliklerini ayarla
                SetFileAttributes(desktopIniPath, FileAttributes.Hidden | FileAttributes.System);
                SetFileAttributes(folderPath, File.GetAttributes(folderPath) | FileAttributes.ReadOnly | FileAttributes.System);

                // Explorer'ı yenile
                RefreshExplorer();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"İkon uygulanırken hata: {ex.Message}", ex);
            }
        }

        public void RemoveFolderIcon(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                throw new ArgumentException("Geçersiz klasör yolu", nameof(folderPath));

            try
            {
                var desktopIniPath = Path.Combine(folderPath, DESKTOP_INI);

                // desktop.ini dosyasını sil
                if (File.Exists(desktopIniPath))
                {
                    SetFileAttributes(desktopIniPath, FileAttributes.Normal);
                    File.Delete(desktopIniPath);
                }

                // Klasör özniteliklerini sıfırla
                var currentAttributes = File.GetAttributes(folderPath);
                var newAttributes = currentAttributes & ~(FileAttributes.ReadOnly | FileAttributes.System);
                SetFileAttributes(folderPath, newAttributes);

                // Explorer'ı yenile
                RefreshExplorer();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"İkon kaldırılırken hata: {ex.Message}", ex);
            }
        }

        public bool HasCustomIcon(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                return false;

            var desktopIniPath = Path.Combine(folderPath, DESKTOP_INI);
            return File.Exists(desktopIniPath);
        }

        public string? GetCurrentIcon(string folderPath)
        {
            if (!HasCustomIcon(folderPath)) return null;

            try
            {
                var desktopIniPath = Path.Combine(folderPath, DESKTOP_INI);
                var content = File.ReadAllText(desktopIniPath);

                // IconResource satırını bul
                var lines = content.Split('\n');
                foreach (var line in lines)
                {
                    if (line.StartsWith("IconResource=", StringComparison.OrdinalIgnoreCase))
                    {
                        var iconPath = line.Substring("IconResource=".Length).Trim();
                        // Index kısmını kaldır (,0 gibi)
                        var commaIndex = iconPath.IndexOf(',');
                        if (commaIndex > 0)
                        {
                            iconPath = iconPath.Substring(0, commaIndex);
                        }
                        return iconPath;
                    }
                }
            }
            catch
            {
                // Hata durumunda null dön
            }

            return null;
        }

        private string CreateDesktopIniContent(string iconPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[.ShellClassInfo]");
            sb.AppendLine($"IconResource={iconPath},0");
            sb.AppendLine($"[ViewState]");
            sb.AppendLine($"Mode=");
            sb.AppendLine($"Vid=");
            sb.AppendLine($"FolderType={FOLDER_TYPE_GENERIC}");
            return sb.ToString();
        }

        private void SetFileAttributes(string path, FileAttributes attributes)
        {
            try
            {
                File.SetAttributes(path, attributes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Dosya öznitelikleri ayarlanırken hata: {ex.Message}", ex);
            }
        }

        private void RefreshExplorer()
        {
            try
            {
                // Explorer'ı yenile
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
            }
            catch
            {
                // Yenileme hatası kritik değil, sessizce geç
            }
        }

        public bool IsValidIconFile(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath) || !File.Exists(iconPath))
                return false;

            var extension = Path.GetExtension(iconPath).ToLowerInvariant();
            return extension == ".ico";
        }

        public string GetIconInfo(string iconPath)
        {
            if (!IsValidIconFile(iconPath)) return "Geçersiz ikon dosyası";

            try
            {
                var fileInfo = new FileInfo(iconPath);
                return $"Boyut: {fileInfo.Length / 1024} KB, Değiştirilme: {fileInfo.LastWriteTime:dd/MM/yyyy}";
            }
            catch
            {
                return "Bilgi alınamadı";
            }
        }
    }
}
