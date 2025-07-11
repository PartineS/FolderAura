using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using ModernWpf;
using FolderAura.Core;
using FolderAura.Models;
using System.Windows.Media.Imaging;

namespace FolderAura
{
    public partial class MainWindow : INotifyPropertyChanged, IDisposable
    {
        private readonly FolderManager _folderManager;
        private readonly IconManager _iconManager;
        private readonly TransparencyManager _transparencyManager;
        private readonly SettingsManager _settingsManager;
        private readonly ColorManager _colorManager;

        private string _selectedFolderPath = string.Empty;
        private string _selectedIconPath = string.Empty;
        private Brush _selectedBorderColor = Brushes.Transparent;
        private string _folderLabel = string.Empty;
        private FolderSettings _currentSettings = new();
        private bool _disposed = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Başlığı güncelle
            Title = "FolderAura v1.0.0 - Masaüstü Klasör Özelleştirme";

            // Managers'ları başlat
            _folderManager = new FolderManager();
            _iconManager = new IconManager();
            _transparencyManager = new TransparencyManager();
            _settingsManager = new SettingsManager();
            _colorManager = new ColorManager();

            // Başlangıç ayarlarını yükle
            LoadInitialSettings();
            
            // Window'a transparency manager'ı bağla
            Loaded += OnWindowLoaded;
            Closing += OnWindowClosing;
            
            // Bellek optimizasyonu için window state yönetimi
            StateChanged += OnWindowStateChanged;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _transparencyManager.SetWindow(this);
            
            // İlk yükleme sonrası bellek temizliği
            GC.Collect(0, GCCollectionMode.Optimized);
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Pencere kapanırken ayarları kaydet
            SaveApplicationState();
        }

        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            // Minimize durumunda bellek temizliği
            if (WindowState == WindowState.Minimized)
            {
                GC.Collect(0, GCCollectionMode.Optimized);
            }
        }

        #region Properties

        public string SelectedFolderPath
        {
            get => _selectedFolderPath;
            set
            {
                if (SetProperty(ref _selectedFolderPath, value))
                {
                    OnFolderChanged();
                }
            }
        }

        public string SelectedIconPath
        {
            get => _selectedIconPath;
            set
            {
                if (SetProperty(ref _selectedIconPath, value))
                {
                    UpdateIconPreview();
                    OnPropertyChanged(nameof(CanApplyIcon));
                }
            }
        }

        public Brush SelectedBorderColor
        {
            get => _selectedBorderColor;
            set => SetProperty(ref _selectedBorderColor, value);
        }

        public string FolderLabel
        {
            get => _folderLabel;
            set => SetProperty(ref _folderLabel, value);
        }

        public bool CanApplyIcon => !string.IsNullOrEmpty(SelectedFolderPath) && !string.IsNullOrEmpty(SelectedIconPath);

        public bool HasCustomIcon => !string.IsNullOrEmpty(SelectedFolderPath) && _iconManager.HasCustomIcon(SelectedFolderPath);

        #endregion

        #region Event Handlers

        private void BrowseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog()
            {
                Title = "Özelleştirmek istediğiniz klasörü seçin"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedFolderPath = dialog.FolderName;
                UpdateStatus($"Klasör seçildi: {Path.GetFileName(SelectedFolderPath)}");
            }
        }

        private void BrowseIconButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "İkon dosyası seçin",
                Filter = "İkon dosyaları (*.ico)|*.ico|Tüm dosyalar (*.*)|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedIconPath = dialog.FileName;
                UpdateStatus($"İkon seçildi: {Path.GetFileName(SelectedIconPath)}");
            }
        }

        private void ApplyIconButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedFolderPath) || string.IsNullOrEmpty(SelectedIconPath))
                {
                    MessageBox.Show("Lütfen klasör ve ikon dosyası seçin.", "Eksik Bilgi", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _iconManager.SetFolderIcon(SelectedFolderPath, SelectedIconPath);
                OnPropertyChanged(nameof(HasCustomIcon));
                UpdateStatus("İkon başarıyla uygulandı!");
                
                MessageBox.Show("İkon başarıyla uygulandı! Değişikliklerin görünmesi için Explorer'ı yenileyin.", 
                    "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İkon uygulanırken hata: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus("İkon uygulanamadı!");
            }
        }

        private void RemoveIconButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedFolderPath))
                {
                    MessageBox.Show("Lütfen bir klasör seçin.", "Eksik Bilgi", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _iconManager.RemoveFolderIcon(SelectedFolderPath);
                OnPropertyChanged(nameof(HasCustomIcon));
                UpdateStatus("İkon kaldırıldı!");
                
                MessageBox.Show("İkon başarıyla kaldırıldı!", "Başarılı", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İkon kaldırılırken hata: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus("İkon kaldırılamadı!");
            }
        }

        private void EffectComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateEffectStatus();
        }

        private void ApplyEffectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = EffectComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
                var effectType = selectedItem?.Tag?.ToString() ?? "None";

                var result = _transparencyManager.ApplyEffect(effectType);
                
                if (result.Success)
                {
                    UpdateStatus($"Efekt uygulandı: {effectType}");
                    EffectStatusText.Text = $"Efekt Durumu: {effectType} aktif";
                }
                else
                {
                    UpdateStatus("Efekt uygulanamadı!");
                    EffectStatusText.Text = $"Efekt Durumu: Hata - {result.ErrorMessage}";
                    
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        MessageBox.Show(result.ErrorMessage, "Efekt Hatası", 
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Efekt uygulanırken hata: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ColorPickerButton_Click(object sender, RoutedEventArgs e)
        {
            // Basit WPF renk seçici
            var inputDialog = new Window()
            {
                Title = "Renk Seçin",
                Width = 300,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var colorCombo = new ComboBox()
            {
                Margin = new Thickness(20),
                VerticalAlignment = VerticalAlignment.Top
            };

            // Önceden tanımlı renkler
            var colors = new Dictionary<string, Color>()
            {
                {"Kırmızı", Colors.Red},
                {"Mavi", Colors.Blue},
                {"Yeşil", Colors.Green},
                {"Sarı", Colors.Yellow},
                {"Mor", Colors.Purple},
                {"Turuncu", Colors.Orange},
                {"Pembe", Colors.Pink},
                {"Cyan", Colors.Cyan},
                {"Gri", Colors.Gray},
                {"Siyah", Colors.Black}
            };

            foreach (var color in colors)
            {
                colorCombo.Items.Add(color.Key);
            }
            colorCombo.SelectedIndex = 0;

            var okButton = new Button()
            {
                Content = "Tamam",
                Width = 80,
                Height = 30,
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            okButton.Click += (s, ev) => inputDialog.DialogResult = true;

            Grid.SetRow(colorCombo, 0);
            Grid.SetRow(okButton, 1);
            grid.Children.Add(colorCombo);
            grid.Children.Add(okButton);
            inputDialog.Content = grid;

            if (inputDialog.ShowDialog() == true && colorCombo.SelectedItem != null)
            {
                var selectedColorName = colorCombo.SelectedItem.ToString();
                var selectedColor = colors[selectedColorName];
                SelectedBorderColor = new SolidColorBrush(selectedColor);
                UpdateStatus($"Renk seçildi: {selectedColorName}");
            }
        }

        private void ResetColorButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedBorderColor = Brushes.Transparent;
            UpdateStatus("Renk sıfırlandı");
        }

        private void ApplyLabelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedFolderPath))
                {
                    MessageBox.Show("Lütfen bir klasör seçin.", "Eksik Bilgi", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _currentSettings.FolderPath = SelectedFolderPath;
                _currentSettings.Label = FolderLabel;
                _currentSettings.BorderColor = _colorManager.BrushToHex(SelectedBorderColor);

                _settingsManager.SaveFolderSettings(_currentSettings);
                UpdateStatus("Etiket kaydedildi!");
                
                MessageBox.Show("Etiket başarıyla kaydedildi!", "Başarılı", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Etiket kaydedilirken hata: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = new FolderSettings
                {
                    FolderPath = SelectedFolderPath,
                    IconPath = SelectedIconPath,
                    Label = FolderLabel,
                    BorderColor = _colorManager.BrushToHex(SelectedBorderColor),
                    EffectType = (EffectComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Tag?.ToString() ?? "None"
                };

                _settingsManager.SaveFolderSettings(settings);
                UpdateStatus("Ayarlar kaydedildi!");
                
                MessageBox.Show("Tüm ayarlar başarıyla kaydedildi!", "Başarılı", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar kaydedilirken hata: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedFolderPath))
                {
                    MessageBox.Show("Lütfen önce bir klasör seçin.", "Eksik Bilgi", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var settings = _settingsManager.LoadFolderSettings(SelectedFolderPath);
                if (settings != null)
                {
                    LoadSettingsToUI(settings);
                    UpdateStatus("Ayarlar yüklendi!");
                }
                else
                {
                    MessageBox.Show("Bu klasör için kaydedilmiş ayar bulunamadı.", "Bilgi", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar yüklenirken hata: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetAllButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Tüm ayarları sıfırlamak istediğinizden emin misiniz? Bu işlem geri alınamaz.", 
                "Sıfırla Onayı", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ResetAllSettings();
                UpdateStatus("Tüm ayarlar sıfırlandı");
            }
        }

        #endregion

        #region Helper Methods

        private void OnFolderChanged()
        {
            OnPropertyChanged(nameof(CanApplyIcon));
            OnPropertyChanged(nameof(HasCustomIcon));
            
            // Klasör değiştiğinde mevcut ayarları yükle
            LoadFolderSettings();
        }

        private void LoadFolderSettings()
        {
            if (string.IsNullOrEmpty(SelectedFolderPath)) return;

            try
            {
                var settings = _settingsManager.LoadFolderSettings(SelectedFolderPath);
                if (settings != null)
                {
                    LoadSettingsToUI(settings);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Ayarlar yüklenemedi: {ex.Message}");
            }
        }

        private void LoadSettingsToUI(FolderSettings settings)
        {
            SelectedIconPath = settings.IconPath ?? string.Empty;
            FolderLabel = settings.Label ?? string.Empty;
            SelectedBorderColor = _colorManager.HexToBrush(settings.BorderColor);

            // Effect seçimini ayarla
            for (int i = 0; i < EffectComboBox.Items.Count; i++)
            {
                if (EffectComboBox.Items[i] is System.Windows.Controls.ComboBoxItem item)
                {
                    if (item.Tag?.ToString() == settings.EffectType)
                    {
                        EffectComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void UpdateIconPreview()
        {
            try
            {
                if (!string.IsNullOrEmpty(SelectedIconPath) && File.Exists(SelectedIconPath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(SelectedIconPath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    
                    IconPreview.Source = bitmap;
                    IconPreview.Visibility = Visibility.Visible;
                    DefaultIconPreview.Visibility = Visibility.Collapsed;
                }
                else
                {
                    IconPreview.Visibility = Visibility.Collapsed;
                    DefaultIconPreview.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                IconPreview.Visibility = Visibility.Collapsed;
                DefaultIconPreview.Visibility = Visibility.Visible;
            }
        }

        private void UpdateEffectStatus()
        {
            var selectedItem = EffectComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            var effectType = selectedItem?.Tag?.ToString() ?? "None";
            
            var supportInfo = _transparencyManager.CheckEffectSupport(effectType);
            EffectStatusText.Text = $"Efekt Durumu: {supportInfo.Message}";
        }

        private void LoadInitialSettings()
        {
            try
            {
                // Uygulama ayarlarını yükle
                var appSettings = _settingsManager.LoadApplicationSettings();
                if (appSettings != null)
                {
                    if (!string.IsNullOrEmpty(appSettings.LastSelectedFolder) && 
                        Directory.Exists(appSettings.LastSelectedFolder))
                    {
                        SelectedFolderPath = appSettings.LastSelectedFolder;
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Başlangıç ayarları yüklenemedi: {ex.Message}");
            }
        }

        private void ResetAllSettings()
        {
            SelectedFolderPath = string.Empty;
            SelectedIconPath = string.Empty;
            FolderLabel = string.Empty;
            SelectedBorderColor = Brushes.Transparent;
            EffectComboBox.SelectedIndex = 0;
            
            // Transparency'yi sıfırla
            _transparencyManager.ApplyEffect("None");
            
            UpdateStatus("Tüm ayarlar sıfırlandı");
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void SaveApplicationState()
        {
            try
            {
                var appSettings = new ApplicationSettings
                {
                    LastSelectedFolder = SelectedFolderPath
                };
                _settingsManager.SaveApplicationSettings(appSettings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Settings save error: {ex.Message}");
            }
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Managed resources'ları temizle
                    _transparencyManager?.Dispose();
                    
                    // Event handler'ları kaldır
                    Loaded -= OnWindowLoaded;
                    Closing -= OnWindowClosing;
                    StateChanged -= OnWindowStateChanged;
                }
                
                _disposed = true;
            }
        }

        ~MainWindow()
        {
            Dispose(false);
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            Dispose();
            base.OnClosed(e);
        }
    }
}