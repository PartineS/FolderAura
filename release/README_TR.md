# FolderAura - Masaüstü Klasör Özelleştirme Uygulaması

## 📂 Genel Bakış
FolderAura, Windows masaüstünüzdeki klasörlerin görünümünü değiştirmenizi sağlayan bir uygulamadır. Bu program ile:
- Klasörlere özel simgeler ekleyebilir
- Klasör pencerelerine modern Windows 11 efektleri (bulanıklık, şeffaflık) uygulayabilir
- Klasörlere renk etiketleri ekleyebilirsiniz

**ÖNEMLİ**: Bu uygulama sadece görsel değişiklikler yapar, klasör içeriğinize dokunmaz.

## ✨ Neler Yapabilir?
- **Klasör Simgesi Değiştirme**: Herhangi bir klasörün simgesini istediğiniz resimle değiştirebilirsiniz
- **Modern Efektler**: Klasör pencerelerine bulanıklık, şeffaflık efektleri ekleyebilirsiniz
- **Renk Sistemi**: Klasörlere renk kodları atayabilirsiniz (kırmızı=önemli, yeşil=tamamlandı vb.)
- **Ayar Kaydetme**: Yaptığınız değişiklikler otomatik kaydedilir
- **Geri Alma**: İstediğiniz zaman eski haline döndürebilirsiniz

## 🔧 Sistem Gereksinimleri
- **Windows 10** (sürüm 1809 veya üstü) ya da **Windows 11**
- **.NET 8.0 Runtime** (yoksa otomatik yüklenecek)
- **En az 1GB boş disk alanı**
- **Yönetici hakları** (klasör değişiklikleri için gerekli)

## 📥 KURULUM - ADIM ADIM

### Yöntem 1: Otomatik Kurulum (ÖNERİLEN)

1. **Dosyaları indirin**: `FolderAura` klasörünü masaüstünüze kopyalayın
2. **Kurulum dosyasını bulun**: `install.cmd` dosyasını bulun
3. **Yönetici olarak çalıştırın**:
   - `install.cmd` dosyasına **SAĞ TIKLAYIN**
   - **"Yönetici olarak çalıştır"** seçeneğini seçin
   - Windows UAC uyarısında **"Evet"** deyin
4. **Kurulum sihirbazını takip edin**:
   - Kurulum otomatik başlayacak
   - Masaüstü kısayolu istenirse **"E"** yazın
   - Kurulum tamamlanınca **Enter** tuşuna basın

### Yöntem 2: Manuel Kurulum

1. **Uygulama dosyasını kopyalayın**: `FolderAura.exe` dosyasını istediğiniz klasöre kopyalayın
2. **Her kullanımda yönetici olarak çalıştırın**:
   - `FolderAura.exe` dosyasına **SAĞ TIKLAYIN**
   - **"Yönetici olarak çalıştır"** seçin

## 🚀 UYGULAMA NASIL KULLANILIR?

### İlk Açılış
1. **Başlat Menüsünden**: 
   - **Windows tuşu** + **S** basın
   - **"FolderAura"** yazın
   - Çıkan sonuca tıklayın

2. **Veya Masaüstünden**:
   - Masaüstündeki **FolderAura** simgesine çift tıklayın

### Ana Kullanım

#### Klasör Simgesi Değiştirme:
1. **"Klasör Seç"** butonuna tıklayın
2. Değiştirmek istediğiniz klasörü seçin (örn: Masaüstü'ndeki "Belgelerim")
3. **"Simge Seç"** butonuna tıklayın
4. Kullanmak istediğiniz resmi seçin (.ico, .png, .jpg dosyaları)
5. **"Değişiklikleri Uygula"** butonuna tıklayın
6. **Klasör Explorer'ı yenileyin** (F5 tuşu)

#### Efekt Uygulama:
1. Bir klasör seçin
2. **Efekt türünü** seçin:
   - **Mica**: Modern Windows 11 bulanık efekti
   - **Acrylic**: Yarı şeffaf cam efekti
   - **Blur**: Basit bulanıklık efekti
3. **Şeffaflık seviyesini** ayarlayın (0-100)
4. **"Değişiklikleri Uygula"** butonuna tıklayın

#### Renk Ekleme:
1. Bir klasör seçin
2. **"Renk Seç"** butonuna tıklayın
3. Listeden bir renk seçin
4. **"Değişiklikleri Uygula"** butonuna tıklayın

## ⚙️ Ayarlar ve Özellikler

### Ayar Dosyası Konumu
- Ayarlarınız şurada saklanır: `C:\Users\[KullanıcıAdı]\AppData\Local\FolderAura\`
- Bu klasörü silmek tüm ayarları sıfırlar

### Ayarları Sıfırlama
- Uygulama içinde **"Ayarları Sıfırla"** butonuna tıklayın
- Veya yukarıdaki klasörü silin

### Yedekleme
- Ayar dosyanızı başka yere kopyalayarak yedek alabilirsiniz
- `settings.json` dosyası tüm ayarlarınızı içerir

## 🔧 SORUN GİDERME

### "Uygulama açılmıyor" Sorunu
**Çözüm 1**: Yönetici İzinlerini Kontrol Edin
- Uygulamaya **SAĞ TIKLAYIN** → **"Yönetici olarak çalıştır"**
- Windows UAC uyarısında **"Evet"** deyin

**Çözüm 2**: .NET Runtime Kontrol Edin
- **Windows + R** tuşlarına basın
- **"cmd"** yazın ve **Enter** basın
- **"dotnet --version"** yazın
- Hata veriyorsa [Microsoft .NET 8.0 indirin](https://dotnet.microsoft.com/download/dotnet/8.0)

### "Klasör değişiklikleri görünmüyor" Sorunu
**Çözüm 1**: Explorer'ı Yenileyin
- Klasör penceresinde **F5** tuşuna basın
- Veya **Ctrl + F5** yapın

**Çözüm 2**: Windows Explorer'ı Yeniden Başlatın
- **Ctrl + Shift + Esc** tuşlarına basın (Görev Yöneticisi)
- **"Windows Explorer"** bulun
- **Sağ tıklayın** → **"Yeniden başlat"**

**Çözüm 3**: Bilgisayarı Yeniden Başlatın
- Değişiklikler bazen restart sonrası görünür

### "Simge gözükmüyor" Sorunu
**Çözüm 1**: ICO Formatı Kullanın
- PNG/JPEG yerine **.ico** uzantılı dosya kullanın
- Online ICO dönüştürücülerle resminizi .ico yapın

**Çözüm 2**: Simge Boyutunu Kontrol Edin
- Çok büyük dosyalar sorun çıkarabilir
- 256x256 piksel veya daha küçük kullanın

### "İzin reddedildi" Hatası
**Çözüm**: Klasör İzinlerini Kontrol Edin
- Değiştirmeye çalıştığınız klasöre **yazma izniniz** olmalı
- Sistem klasörlerini (Windows, Program Files) değiştirmeyin

## 🗑️ KALDIRMA

### Otomatik Kaldırma (Kurulum yapıldıysa)
1. **Denetim Masası** açın
2. **"Program ekle veya kaldır"** kısmına gidin
3. **"FolderAura"** bulun ve **"Kaldır"** tıklayın
4. Veya kurulum klasöründe **"uninstall.cmd"** dosyasını **yönetici olarak çalıştırın**

### Manuel Kaldırma
1. Uygulamayı kapatın
2. `FolderAura.exe` dosyasını silin
3. Ayarları temizlemek için: **Windows + R** → **`%LOCALAPPDATA%`** → **FolderAura** klasörünü silin

## 🔒 Güvenlik ve Gizlilik
- ✅ Sadece seçtiğiniz klasörlerde değişiklik yapar
- ✅ Dosyalarınızı okumaz veya değiştirmez
- ✅ İnternet bağlantısı kullanmaz
- ✅ Kişisel bilgi toplamaz
- ✅ Virüs/zararlı yazılım içermez

## � Destek ve İletişim
- **Sorun yaşıyorsanız**: GitHub Issues bölümünü kullanın
- **Önerileriniz için**: Yine GitHub Issues'e yazın
- **Hızlı yardım**: README dosyasındaki sorun giderme bölümünü okuyun

## 📜 Lisans
Bu uygulama **tamamen ücretsizdir** ve kişisel kullanım içindir.

---
**FolderAura v1.0.0** - © 2025 FolderAura Team

**Son Güncelleme**: 11 Temmuz 2025
