using System;
using System.IO;

namespace FolderAura.Core
{
    public class FolderManager
    {
        public bool IsValidFolder(string folderPath)
        {
            return !string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath);
        }

        public bool HasWritePermission(string folderPath)
        {
            try
            {
                if (!IsValidFolder(folderPath)) return false;

                // Test yazma izni için geçici bir dosya oluşturmayı dene
                var testFile = Path.Combine(folderPath, $"__temp_test_{Guid.NewGuid()}.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFolderDisplayName(string folderPath)
        {
            if (!IsValidFolder(folderPath)) return string.Empty;
            return Path.GetFileName(folderPath) ?? Path.GetDirectoryName(folderPath) ?? folderPath;
        }

        public long GetFolderSize(string folderPath)
        {
            if (!IsValidFolder(folderPath)) return 0;

            try
            {
                var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                long size = 0;
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    size += fileInfo.Length;
                }
                return size;
            }
            catch
            {
                return 0;
            }
        }

        public int GetItemCount(string folderPath)
        {
            if (!IsValidFolder(folderPath)) return 0;

            try
            {
                var files = Directory.GetFiles(folderPath);
                var dirs = Directory.GetDirectories(folderPath);
                return files.Length + dirs.Length;
            }
            catch
            {
                return 0;
            }
        }

        public void RefreshFolder(string folderPath)
        {
            if (!IsValidFolder(folderPath)) return;

            try
            {
                // Windows Explorer'ı yenileme için SHChangeNotify API'si kullanılabilir
                // Şimdilik basit bir implementation
                var directoryInfo = new DirectoryInfo(folderPath);
                directoryInfo.Refresh();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Klasör yenilenemedi: {ex.Message}", ex);
            }
        }
    }
}
