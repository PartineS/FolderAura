using System;
using System.Globalization;
using System.Windows.Media;

namespace FolderAura.Core
{
    public class ColorManager
    {
        public string BrushToHex(Brush brush)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                var color = solidBrush.Color;
                return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            return "#00FFFFFF"; // Transparent
        }

        public Brush HexToBrush(string? hex)
        {
            if (string.IsNullOrEmpty(hex))
                return Brushes.Transparent;

            try
            {
                // # karakterini kaldır
                if (hex.StartsWith("#"))
                    hex = hex.Substring(1);

                // ARGB formatında olduğunu varsay
                if (hex.Length == 8)
                {
                    var a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    var r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    var g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                    var b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                    
                    return new SolidColorBrush(Color.FromArgb(a, r, g, b));
                }
                // RGB formatında
                else if (hex.Length == 6)
                {
                    var r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                    
                    return new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            }
            catch
            {
                // Parsing hatası durumunda transparent döndür
            }

            return Brushes.Transparent;
        }

        public Color GetContrastColor(Color backgroundColor)
        {
            // Rengin parlaklığını hesapla
            var brightness = (backgroundColor.R * 0.299 + backgroundColor.G * 0.587 + backgroundColor.B * 0.114) / 255;
            
            // Parlak arka plan için siyah, koyu arka plan için beyaz döndür
            return brightness > 0.5 ? Colors.Black : Colors.White;
        }

        public Brush GetContrastBrush(Brush backgroundBrush)
        {
            if (backgroundBrush is SolidColorBrush solidBrush)
            {
                var contrastColor = GetContrastColor(solidBrush.Color);
                return new SolidColorBrush(contrastColor);
            }
            
            return Brushes.Black; // Varsayılan
        }

        public bool IsTransparent(Brush brush)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                return solidBrush.Color.A == 0;
            }
            return false;
        }

        public Brush WithOpacity(Brush brush, double opacity)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                var color = solidBrush.Color;
                var newAlpha = (byte)(255 * Math.Clamp(opacity, 0.0, 1.0));
                var newColor = Color.FromArgb(newAlpha, color.R, color.G, color.B);
                return new SolidColorBrush(newColor);
            }
            
            return brush;
        }

        public Brush[] GetPredefinedColors()
        {
            return new Brush[]
            {
                Brushes.Red,
                Brushes.Orange,
                Brushes.Yellow,
                Brushes.Green,
                Brushes.Blue,
                Brushes.Purple,
                Brushes.Pink,
                Brushes.Brown,
                Brushes.Gray,
                Brushes.Black,
                Brushes.Transparent
            };
        }

        public string GetColorName(Brush brush)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                var color = solidBrush.Color;
                
                // Yaygın renkleri kontrol et
                if (color == Colors.Red) return "Kırmızı";
                if (color == Colors.Green) return "Yeşil";
                if (color == Colors.Blue) return "Mavi";
                if (color == Colors.Yellow) return "Sarı";
                if (color == Colors.Orange) return "Turuncu";
                if (color == Colors.Purple) return "Mor";
                if (color == Colors.Pink) return "Pembe";
                if (color == Colors.Brown) return "Kahverengi";
                if (color == Colors.Gray) return "Gri";
                if (color == Colors.Black) return "Siyah";
                if (color == Colors.White) return "Beyaz";
                if (color.A == 0) return "Saydam";
                
                // RGB değerlerini döndür
                return $"RGB({color.R}, {color.G}, {color.B})";
            }
            
            return "Bilinmeyen";
        }

        public Brush BlendColors(Brush color1, Brush color2, double ratio = 0.5)
        {
            if (color1 is SolidColorBrush solid1 && color2 is SolidColorBrush solid2)
            {
                var c1 = solid1.Color;
                var c2 = solid2.Color;
                
                var r = (byte)(c1.R * (1 - ratio) + c2.R * ratio);
                var g = (byte)(c1.G * (1 - ratio) + c2.G * ratio);
                var b = (byte)(c1.B * (1 - ratio) + c2.B * ratio);
                var a = (byte)(c1.A * (1 - ratio) + c2.A * ratio);
                
                return new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }
            
            return color1;
        }

        public bool IsValidHexColor(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return false;

            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            return (hex.Length == 6 || hex.Length == 8) && 
                   int.TryParse(hex, NumberStyles.HexNumber, null, out _);
        }

        public string GetRandomColor()
        {
            var random = new Random();
            var r = random.Next(0, 256);
            var g = random.Next(0, 256);
            var b = random.Next(0, 256);
            
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
