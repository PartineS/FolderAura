using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using FolderAura.Models;
using Vanara.PInvoke;

namespace FolderAura.Core
{
    public class TransparencyManager : IDisposable
    {
        private Window? _window;
        private IntPtr _windowHandle;
        private bool _disposed = false;

        // Windows 11 Mica efekti için gerekli değerler
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_MICA_EFFECT = 1029;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        // Backdrop türleri
        private enum DWM_SYSTEMBACKDROP_TYPE
        {
            DWMSBT_AUTO = 0,
            DWMSBT_NONE = 1,
            DWMSBT_MAINWINDOW = 2,
            DWMSBT_TRANSIENTWINDOW = 3,
            DWMSBT_TABBEDWINDOW = 4
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        private enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_ENABLE_HOSTBACKDROP = 5,
            ACCENT_INVALID_STATE = 6
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        public void SetWindow(Window window)
        {
            _window = window;
            _windowHandle = new WindowInteropHelper(window).Handle;
        }

        public EffectResult ApplyEffect(string effectType)
        {
            if (_window == null || _windowHandle == IntPtr.Zero)
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = "Pencere henüz hazır değil",
                    EffectType = effectType
                };
            }

            try
            {
                return effectType.ToLowerInvariant() switch
                {
                    "mica" => ApplyMicaEffect(),
                    "acrylic" => ApplyAcrylicEffect(),
                    "blur" => ApplyBlurEffect(),
                    "none" or _ => RemoveAllEffects()
                };
            }
            catch (Exception ex)
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = $"Efekt uygulanırken hata: {ex.Message}",
                    EffectType = effectType
                };
            }
        }

        private EffectResult ApplyMicaEffect()
        {
            // Windows 11 versiyonunu kontrol et
            var version = Environment.OSVersion.Version;
            if (version.Major < 10 || (version.Major == 10 && version.Build < 22000))
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = "Mica efekti Windows 11 veya üzeri gerektirir",
                    EffectType = "Mica"
                };
            }

            try
            {
                // Mica efektini etkinleştir
                int value = (int)DWM_SYSTEMBACKDROP_TYPE.DWMSBT_MAINWINDOW;
                int result = DwmSetWindowAttribute(_windowHandle, DWMWA_SYSTEMBACKDROP_TYPE, ref value, sizeof(int));

                if (result == 0)
                {
                    return new EffectResult
                    {
                        Success = true,
                        EffectType = "Mica"
                    };
                }
                else
                {
                    // Eski API'yi dene
                    value = 1;
                    result = DwmSetWindowAttribute(_windowHandle, DWMWA_MICA_EFFECT, ref value, sizeof(int));
                    
                    return new EffectResult
                    {
                        Success = result == 0,
                        ErrorMessage = result != 0 ? "Mica efekti etkinleştirilemedi" : null,
                        EffectType = "Mica"
                    };
                }
            }
            catch (Exception ex)
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    EffectType = "Mica"
                };
            }
        }

        private EffectResult ApplyAcrylicEffect()
        {
            try
            {
                var accent = new AccentPolicy
                {
                    AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND,
                    AccentFlags = 2,
                    GradientColor = 0x01000000, // Hafif siyah ton
                    AnimationId = 0
                };

                var accentStructSize = Marshal.SizeOf(accent);
                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData
                {
                    Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                    Data = accentPtr,
                    SizeOfData = accentStructSize
                };

                int result = SetWindowCompositionAttribute(_windowHandle, ref data);
                Marshal.FreeHGlobal(accentPtr);

                return new EffectResult
                {
                    Success = result != 0,
                    ErrorMessage = result == 0 ? "Acrylic efekti etkinleştirilemedi" : null,
                    EffectType = "Acrylic"
                };
            }
            catch (Exception ex)
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    EffectType = "Acrylic"
                };
            }
        }

        private EffectResult ApplyBlurEffect()
        {
            try
            {
                var accent = new AccentPolicy
                {
                    AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND,
                    AccentFlags = 2,
                    GradientColor = 0,
                    AnimationId = 0
                };

                var accentStructSize = Marshal.SizeOf(accent);
                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData
                {
                    Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                    Data = accentPtr,
                    SizeOfData = accentStructSize
                };

                int result = SetWindowCompositionAttribute(_windowHandle, ref data);
                Marshal.FreeHGlobal(accentPtr);

                return new EffectResult
                {
                    Success = result != 0,
                    ErrorMessage = result == 0 ? "Blur efekti etkinleştirilemedi" : null,
                    EffectType = "Blur"
                };
            }
            catch (Exception ex)
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    EffectType = "Blur"
                };
            }
        }

        private EffectResult RemoveAllEffects()
        {
            try
            {
                // Mica'yı kapat
                int value = (int)DWM_SYSTEMBACKDROP_TYPE.DWMSBT_NONE;
                DwmSetWindowAttribute(_windowHandle, DWMWA_SYSTEMBACKDROP_TYPE, ref value, sizeof(int));

                // Acrylic/Blur'u kapat
                var accent = new AccentPolicy
                {
                    AccentState = AccentState.ACCENT_DISABLED,
                    AccentFlags = 0,
                    GradientColor = 0,
                    AnimationId = 0
                };

                var accentStructSize = Marshal.SizeOf(accent);
                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData
                {
                    Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                    Data = accentPtr,
                    SizeOfData = accentStructSize
                };

                SetWindowCompositionAttribute(_windowHandle, ref data);
                Marshal.FreeHGlobal(accentPtr);

                return new EffectResult
                {
                    Success = true,
                    EffectType = "None"
                };
            }
            catch (Exception ex)
            {
                return new EffectResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    EffectType = "None"
                };
            }
        }

        public EffectSupportInfo CheckEffectSupport(string effectType)
        {
            var version = Environment.OSVersion.Version;
            
            return effectType.ToLowerInvariant() switch
            {
                "mica" => new EffectSupportInfo
                {
                    IsSupported = version.Major >= 10 && version.Build >= 22000,
                    Message = version.Build >= 22000 ? "Destekleniyor" : "Windows 11 gerekli",
                    RequiredVersion = "Windows 11 (Build 22000+)"
                },
                "acrylic" => new EffectSupportInfo
                {
                    IsSupported = version.Major >= 10,
                    Message = version.Major >= 10 ? "Destekleniyor" : "Windows 10+ gerekli",
                    RequiredVersion = "Windows 10+"
                },
                "blur" => new EffectSupportInfo
                {
                    IsSupported = version.Major >= 10,
                    Message = version.Major >= 10 ? "Destekleniyor" : "Windows 10+ gerekli",
                    RequiredVersion = "Windows 10+"
                },
                "none" => new EffectSupportInfo
                {
                    IsSupported = true,
                    Message = "Destekleniyor",
                    RequiredVersion = "Tüm Windows sürümleri"
                },
                _ => new EffectSupportInfo
                {
                    IsSupported = false,
                    Message = "Bilinmeyen efekt",
                    RequiredVersion = "N/A"
                }
            };
        }

        public bool IsWindowsVersionSupported()
        {
            var version = Environment.OSVersion.Version;
            return version.Major >= 10;
        }

        public string GetWindowsVersionInfo()
        {
            var version = Environment.OSVersion.Version;
            return $"Windows {version.Major}.{version.Minor} (Build {version.Build})";
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
                    // Tüm efektleri kaldır
                    if (_windowHandle != IntPtr.Zero)
                    {
                        try
                        {
                            RemoveAllEffects();
                        }
                        catch
                        {
                            // Dispose sırasında hataları sessizce geç
                        }
                    }
                    
                    _window = null;
                    _windowHandle = IntPtr.Zero;
                }
                
                _disposed = true;
            }
        }

        ~TransparencyManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
