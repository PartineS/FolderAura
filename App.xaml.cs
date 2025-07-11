using System;
using System.Runtime;
using System.Windows;
using ModernWpf;

namespace FolderAura
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Maksimum bellek optimizasyonu
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            
            // Yönetici izinlerini kontrol et
            if (!IsRunningAsAdministrator())
            {
                MessageBox.Show(
                    "FolderAura yönetici izinleri ile çalıştırılmalıdır.\n\n" +
                    "Lütfen uygulamayı sağ tıklayıp 'Yönetici olarak çalıştır' seçeneğini kullanın.",
                    "Yönetici İzni Gerekli",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                
                Environment.Exit(1);
                return;
            }

            // Modern WPF temasını uygula (minimal kaynak kullanımı)
            ThemeManager.Current.ApplicationTheme = null;
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Agresif bellek temizliği
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced, true);
            
            base.OnExit(e);
        }

        private static bool IsRunningAsAdministrator()
        {
            try
            {
                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                var principal = new System.Security.Principal.WindowsPrincipal(identity);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }
}
