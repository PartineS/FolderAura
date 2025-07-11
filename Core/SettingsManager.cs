using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FolderAura.Models;

namespace FolderAura.Core
{
    public class SettingsManager
    {
        private readonly string _settingsDirectory;
        private readonly string _appSettingsFile;
        private readonly string _foldersDataFile;

        public SettingsManager()
        {
            _settingsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FolderAura");
            
            _appSettingsFile = Path.Combine(_settingsDirectory, "appsettings.json");
            _foldersDataFile = Path.Combine(_settingsDirectory, "folders.json");

            // Ayarlar dizinini oluştur
            EnsureSettingsDirectory();
        }

        private void EnsureSettingsDirectory()
        {
            try
            {
                if (!Directory.Exists(_settingsDirectory))
                {
                    Directory.CreateDirectory(_settingsDirectory);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ayarlar dizini oluşturulamadı: {ex.Message}", ex);
            }
        }

        public void SaveApplicationSettings(ApplicationSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_appSettingsFile, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Uygulama ayarları kaydedilemedi: {ex.Message}", ex);
            }
        }

        public ApplicationSettings? LoadApplicationSettings()
        {
            try
            {
                if (!File.Exists(_appSettingsFile))
                    return new ApplicationSettings(); // Varsayılan ayarları döndür

                var json = File.ReadAllText(_appSettingsFile);
                return JsonSerializer.Deserialize<ApplicationSettings>(json);
            }
            catch (Exception ex)
            {
                // Hata durumunda varsayılan ayarları döndür
                System.Diagnostics.Debug.WriteLine($"Ayarlar yüklenirken hata: {ex.Message}");
                return new ApplicationSettings();
            }
        }

        public void SaveFolderSettings(FolderSettings settings)
        {
            try
            {
                var folderDataDict = LoadAllFolderSettings();
                
                // Klasör yolunu anahtar olarak kullan
                var key = settings.FolderPath.ToLowerInvariant();
                folderDataDict[key] = settings;

                var json = JsonSerializer.Serialize(folderDataDict, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                
                File.WriteAllText(_foldersDataFile, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Klasör ayarları kaydedilemedi: {ex.Message}", ex);
            }
        }

        public FolderSettings? LoadFolderSettings(string folderPath)
        {
            try
            {
                var folderDataDict = LoadAllFolderSettings();
                var key = folderPath.ToLowerInvariant();
                
                return folderDataDict.TryGetValue(key, out var settings) ? settings : null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Klasör ayarları yüklenirken hata: {ex.Message}");
                return null;
            }
        }

        public Dictionary<string, FolderSettings> LoadAllFolderSettings()
        {
            try
            {
                if (!File.Exists(_foldersDataFile))
                    return new Dictionary<string, FolderSettings>();

                var json = File.ReadAllText(_foldersDataFile);
                return JsonSerializer.Deserialize<Dictionary<string, FolderSettings>>(json) 
                       ?? new Dictionary<string, FolderSettings>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Tüm klasör ayarları yüklenirken hata: {ex.Message}");
                return new Dictionary<string, FolderSettings>();
            }
        }

        public void DeleteFolderSettings(string folderPath)
        {
            try
            {
                var folderDataDict = LoadAllFolderSettings();
                var key = folderPath.ToLowerInvariant();
                
                if (folderDataDict.ContainsKey(key))
                {
                    folderDataDict.Remove(key);
                    
                    var json = JsonSerializer.Serialize(folderDataDict, new JsonSerializerOptions 
                    { 
                        WriteIndented = true 
                    });
                    
                    File.WriteAllText(_foldersDataFile, json);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Klasör ayarları silinemedi: {ex.Message}", ex);
            }
        }

        public List<FolderSettings> GetRecentFolders(int count = 10)
        {
            try
            {
                var allSettings = LoadAllFolderSettings();
                var recentList = new List<FolderSettings>();

                foreach (var kvp in allSettings.Values)
                {
                    // Sadece aktif klasörleri ve mevcut olanları ekle
                    if (kvp.IsActive && Directory.Exists(kvp.FolderPath))
                    {
                        recentList.Add(kvp);
                    }
                }

                // Son değiştirilme tarihine göre sırala
                recentList.Sort((x, y) => y.LastModified.CompareTo(x.LastModified));

                // İstenen sayıda döndür
                return recentList.Count > count ? recentList.GetRange(0, count) : recentList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Son klasörler yüklenirken hata: {ex.Message}");
                return new List<FolderSettings>();
            }
        }

        public void CleanupOldSettings(TimeSpan maxAge)
        {
            try
            {
                var allSettings = LoadAllFolderSettings();
                var cutoffDate = DateTime.Now - maxAge;
                var keysToRemove = new List<string>();

                foreach (var kvp in allSettings)
                {
                    var settings = kvp.Value;
                    
                    // Eski veya mevcut olmayan klasörleri işaretle
                    if (settings.LastModified < cutoffDate || !Directory.Exists(settings.FolderPath))
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // İşaretlenen ayarları sil
                foreach (var key in keysToRemove)
                {
                    allSettings.Remove(key);
                }

                // Değişiklikleri kaydet
                if (keysToRemove.Count > 0)
                {
                    var json = JsonSerializer.Serialize(allSettings, new JsonSerializerOptions 
                    { 
                        WriteIndented = true 
                    });
                    
                    File.WriteAllText(_foldersDataFile, json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eski ayarlar temizlenirken hata: {ex.Message}");
            }
        }

        public void ResetAllSettings()
        {
            try
            {
                // Ayar dosyalarını sil
                if (File.Exists(_appSettingsFile))
                    File.Delete(_appSettingsFile);
                    
                if (File.Exists(_foldersDataFile))
                    File.Delete(_foldersDataFile);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ayarlar sıfırlanırken hata: {ex.Message}", ex);
            }
        }

        public long GetSettingsSize()
        {
            try
            {
                long totalSize = 0;

                if (File.Exists(_appSettingsFile))
                    totalSize += new FileInfo(_appSettingsFile).Length;

                if (File.Exists(_foldersDataFile))
                    totalSize += new FileInfo(_foldersDataFile).Length;

                return totalSize;
            }
            catch
            {
                return 0;
            }
        }

        public bool BackupSettings(string backupPath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(backupPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(backupPath)!);

                var backupDir = Path.Combine(backupPath, $"FolderAura_Backup_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(backupDir);

                if (File.Exists(_appSettingsFile))
                    File.Copy(_appSettingsFile, Path.Combine(backupDir, "appsettings.json"));

                if (File.Exists(_foldersDataFile))
                    File.Copy(_foldersDataFile, Path.Combine(backupDir, "folders.json"));

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Yedekleme hatası: {ex.Message}");
                return false;
            }
        }

        public string GetSettingsDirectory() => _settingsDirectory;
    }
}
