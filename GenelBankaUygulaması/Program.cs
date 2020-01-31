using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankaUygulamasiClasslari; //referans olarakta eklemek gerekiyor başka proje olduğu için

namespace GenelBankaUygulaması
{
    class Program
    {
        
        static string gondereninAdi; //başka class'da kullanmaya ihtiyaç duyulmazsa public yazmaya gerek yoktur
        public static List<vadesizHesap> hesapListesi = new List<vadesizHesap>(5); //class nesnelerini tutan bir liste oluşturuyoruz
        public static uint hesapNolari = 0; //pozitif bir şekilde hesapnolarını tutuyoruz
        public static bool vadesizParaArtisiniGoster = false; //tipe göre faiz artışı gösterimini kontrol için 
        public static bool aylikParaArtisiniGoster = false;
        public static bool yillikParaArtisiniGoster = false;
        static void Main(string[] args)
        {
            byte secim;
           
            while (true) //sürekli menü açılsın diye
            {
                
                Program girisPaneli = new Program();
                secim = girisPaneli.girisPaneli(); //giris paneli static olmadığı için class dan obje oluşturup çağırdık statik olsaydı direk kullanabilirdik
                switch (secim)
                {
                    case 1:
                        hesapAcilisi();
                        break; //diğer caselere bakılmasın diye
                    case 2:
                        paraYatirma();
                        break;
                    case 3:
                        paraCekme();
                        break;
                    case 4:
                        paraTransferi();
                        break;
                    case 5:
                        istenilenHesabiGoruntule();
                        break;
                    case 6:
                        hesaplariListele();
                        break;
                    case 7:
                        dosyayaKaydet();
                        break;
                    case 8:
                        dosyadanOku();
                        break;
                    case 9:
                        hesapSil();
                        break;
                    case 10:
                        faizArtisiniGor();
                        break;
                    case 11:
                        break;
                    default:
                        break;
                }
                Console.Clear();
                //11. işlem
                if (secim == 11) //Çıkış işlemi yapılıyor
                    break;
            }

        }
        public byte girisPaneli() //public başka classlardan da erişebilmemizi sağlıyor
        {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;
            yillikParaArtisiniGoster = false;
            byte secim = 11;

            do
            {

                Console.WriteLine("DenizBank giriş paneli");
                Console.WriteLine();
                Console.WriteLine(" 1 Hesap açılışı");
                Console.WriteLine(" 2 Para yatırma");
                Console.WriteLine(" 3 Para çekme");
                Console.WriteLine(" 4 Para transferi");
                Console.WriteLine(" 5 İstenilen hesabı görüntüle");
                Console.WriteLine(" 6 Hesapları listele");
                Console.WriteLine(" 7 Hesapları dosyaya kaydet");
                Console.WriteLine(" 8 Hesapları dosyadan getir");
                Console.WriteLine(" 9 Hesap sil");
                Console.WriteLine(" 10 Faiz artışını gör");
                Console.WriteLine(" 11 Çıkış");
                Console.WriteLine();
                Console.Write("Seçiminiz---------->");
                byte.TryParse(Console.ReadLine(), out secim);

                if (1 <= secim && secim <= 11)
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Doğru seçim yapmadınız, tekrar bir işlem seçiniz lütfen.");
                    Console.WriteLine();
                }
            } while (true);

            return secim;
        }
        //1. işlem
        public static void hesapAcilisi()
        {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;
            yillikParaArtisiniGoster = false;
            
            bool tcKontrol=false;
            string tcKontrol1; //length kullanmak için string oluşturduk 
            long tcKontrol2; //int yapamıyoruz boyutu yetmiyor, rakam kontrolü için oluşturdum
            
            vadesizHesap vh = new vadesizHesap();            
            
            vh.hesapNo = ++hesapNolari; //ilk başta arttırıyoruz ki 1 olsun numarası
            Console.WriteLine("Ad soyad giriniz : ");
            vh.adiSoyadi = Console.ReadLine();
            Console.WriteLine("Tc no : ");
            tcKontrol1 =Console.ReadLine();            
            tcKontrol = long.TryParse(tcKontrol1, out tcKontrol2); 
            while (true) {
                if (tcKontrol1.Length == 11 && tcKontrol==true) //Tc kontrol
                {                   
                    vh.tcNo = Convert.ToString(tcKontrol2);
                    break;
                }
                else
                {
                    //Console.Clear(); istenirse kullanılabilir önceki yazıları siler
                    Console.WriteLine("Tc numarası 11 haneli ve rakamlardan oluşmalıdır.");                  
                    Console.WriteLine("Tc no : ");
                    tcKontrol1 = Console.ReadLine();
                    tcKontrol = long.TryParse(tcKontrol1, out tcKontrol2);                   
                }
            }
                
            Console.WriteLine("Açılış bakiyesi giriniz : ");
            double.TryParse(Console.ReadLine(), out vh.hesapBakiyesi); //bir değer girmezse 0 verir
            vh.hesapAcilisTarihi = DateTime.Today;
            vh.vadeBaslangici = DateTime.Today;

         yanlisVade: //Bu bir labeldir
            Console.WriteLine("Vade tipi? Vadesiz=1, Aylık=2, yıllık=3");
            byte vtip;
            byte.TryParse(Console.ReadLine(), out vtip);
            if (vtip == 1)
            {
                vh.hesapVadesi = vadeTipi.vadesiz;
                //gelen faiz miktarını ilk başta başlatmak için aşağıdaki kodları yazdık               
                object obj = new object();
                obj = "Vadesiz hesap bakiyesi artışı : ";
                ilkAnaPara = vh.hesapBakiyesi;
                TimerCallback tcb = new TimerCallback(vadesizFaizEkle); //Timer için hazırlıyoruz içindeki fonksiyonu çalıştıracak
                Timer saat = new Timer(tcb, obj, 2000, 2000); //2 saniye sonra yazmaya başlasın 2 saniyede 1 yazsın
                 
            }
            else if (vtip == 2)
            {
                vh.hesapVadesi = vadeTipi.aylik;
                
                object obj = new object();
                obj = "Aylık hesap bakiyesi artışı : ";
                ilkAnaPara = vh.hesapBakiyesi; //yaratılan objenin bakiyesini tutuyoruz
                TimerCallback tcb = new TimerCallback(aylikFaizEkle); //fonksiyona objeyi de yolluyor
                Timer saat = new Timer(tcb, obj, 2000, 2000);
            }
            else if (vtip == 3)
            {
                vh.hesapVadesi = vadeTipi.yillik;

                object obj = new object();
                obj = "Yıllık hesap bakiyesi artışı : ";
                ilkAnaPara = vh.hesapBakiyesi;
                TimerCallback tcb = new TimerCallback(yillikFaizEkle);
                Timer saat = new Timer(tcb, obj, 2000, 2000);
            }
            else
            {
                goto yanlisVade; //verilen labele gider işleme o satırdan sonra devam eder
            }
            vh.gelenFaiz = 0;

            hesapListesi.Add(vh);
            Console.WriteLine();
            Console.WriteLine("Hesap numarası "+ vh.hesapNo +" olan kişinin kayıt işlemi başarıyla gerçekleştirilmiştir.");
            Console.ReadLine();

        }

        public static double ilkAnaPara;        

        public static double vadesizYeniHesapBakiyesi; //güncel hesap bilgisini tutuyoruz
        public static double yillikYeniHesapBakiyesi;
        public static double aylikYeniHesapBakiyesi;

        public static byte vadesizSayac = 0; //ilk para atamaları için oluşturduk
        public static byte yillikSayac = 0;
        public static byte aylikSayac = 0;      
        
        public static void vadesizFaizEkle(object Yazi)
        {
            double gelenFaiz = 0;
            string yazi = Convert.ToString(Yazi);
            vadesizHesap vh = new vadesizHesap();
            if (vadesizSayac == 0) //İşlemleri 1 kez yapsın 
            {
                vh.hesapBakiyesi = ilkAnaPara; 
                vadesizYeniHesapBakiyesi = ilkAnaPara;
                vadesizSayac++;
            }
            else
            {
                vh.hesapBakiyesi = vadesizYeniHesapBakiyesi;
            }

            gelenFaiz += vh.faizUygula();
            vadesizYeniHesapBakiyesi += gelenFaiz;

            if (vadesizParaArtisiniGoster)
            {
                Console.WriteLine(yazi + vh.hesapBakiyesi + " " + DateTime.Now);
            }
        }
        public static void aylikFaizEkle(object Yazi)
        {
            double gelenFaiz=0;
            string yazi = Convert.ToString(Yazi); //Console'da kullanmak için çevirdik  
            vadesizHesap vh = new aylikVadeli(); //child'ın methoduna erişmek için (polymorphism)
            if (aylikSayac==0) //İşlemleri 1 kez yapsın 
            {
                vh.hesapBakiyesi = ilkAnaPara; //faiz için ilk ana parayı atadık
                aylikYeniHesapBakiyesi = ilkAnaPara;

                aylikSayac++;                
            }
            else
            {
                vh.hesapBakiyesi = aylikYeniHesapBakiyesi;
            }

            gelenFaiz += vh.faizUygula();
            aylikYeniHesapBakiyesi += gelenFaiz;
            

            if (aylikParaArtisiniGoster)
            {
                Console.WriteLine(yazi + aylikYeniHesapBakiyesi + " " + DateTime.Now); 
            }
        }
        public static void yillikFaizEkle(object Yazi)
        {
            double gelenFaiz = 0;
            string yazi = Convert.ToString(Yazi);
            vadesizHesap vh = new yillikVadeli();
            if (yillikSayac == 0) //İşlemleri 1 kez yapsın 
            {
                vh.hesapBakiyesi = ilkAnaPara;
                yillikYeniHesapBakiyesi = ilkAnaPara;
                yillikSayac++;
            }
            else
            {
                vh.hesapBakiyesi = yillikYeniHesapBakiyesi;
            }
            gelenFaiz += vh.faizUygula();
            yillikYeniHesapBakiyesi += gelenFaiz;


            if (yillikParaArtisiniGoster)
            {
                Console.WriteLine(yazi + vh.hesapBakiyesi + " " + DateTime.Now);
            }
        }
        //2. işlem
        public static void paraYatirma()
        {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;
            yillikParaArtisiniGoster = false;
            uint hNo = 0; //para yatırmak istenilen hesap no için tanımladık
            double yatanMiktar = 0;
            bool kontrol = true;

            Console.WriteLine("Para yatırma");
            Console.WriteLine();
            Console.Write("Hesap no giriniz : ");
            uint.TryParse(Console.ReadLine(), out hNo);
            Console.Write("Yatacak para miktarını giriniz : ");            
            double.TryParse(Console.ReadLine(), out yatanMiktar);
            Console.WriteLine();

            if (hNo != 0)
            {
                foreach (vadesizHesap vh in hesapListesi)
                {
                    if (hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                    {
                            int index = hesapListesi.IndexOf(vh);
                            if (yatanMiktar > 0) 
                            { 
                                vh.hesapBakiyesi += yatanMiktar; 
                            }else
                            {
                                kontrol = false;
                                Console.WriteLine("Yatırılacak para miktarı sıfırdan büyük olmalıdır.");
                                break;
                            }
                            hesapListesi[index] = vh;
                            Console.WriteLine(hNo + " Nolu hesaba "+ yatanMiktar + " TL başarılı bir şekilde yatırılmıştır.");
                            Console.WriteLine("Hesapta ki güncel para durumunu : "+ vh.hesapBakiyesi +" TL'dir");
                        kontrol = false;
                        break;
                    }                  
                }
                if (kontrol) 
                {
                    Console.WriteLine("Aranılan hesap numarası bulunamadı!"); 
                }               
            }
            else
            {
                Console.WriteLine("yanlış hesap numarası girdiniz!");
            }
            
            Console.ReadLine();
        }
        //3. işlem
        public static void paraCekme()
            {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;
            yillikParaArtisiniGoster = false;
            uint hNo = 0; //para cekilecek hesap no
                double cekilecekMiktar = 0;
                bool kontrol = true;

                Console.WriteLine("Para çekme");
                Console.WriteLine();
                Console.Write("Hesap no giriniz : ");
                uint.TryParse(Console.ReadLine(), out hNo);               
                Console.Write("Çekilecek para miktarını giriniz : ");               
                double.TryParse(Console.ReadLine(), out cekilecekMiktar);
                Console.WriteLine();
            
            if (hNo != 0)
                {
                    foreach (vadesizHesap vh in hesapListesi)
                    {
                        if (hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                        {
                            int index = hesapListesi.IndexOf(vh);
                            if (cekilecekMiktar > 0 && cekilecekMiktar<vh.hesapBakiyesi)
                            {
                                vh.hesapBakiyesi -= cekilecekMiktar;
                            }else
                            {
                                kontrol = false;
                                Console.WriteLine("Çekilecek para miktarı sıfırdan büyük ve hesap bakiyesinden küçük olmalıdır.");
                                break;
                            }                            
                            hesapListesi[index] = vh;                            
                            Console.WriteLine(hNo + " Nolu hesaptan " + cekilecekMiktar + " TL başarılı bir şekilde çekilmiştir.");
                            Console.WriteLine("Hesapta ki güncel para durumunu : " + vh.hesapBakiyesi + " TL'dir");                                                
                        kontrol = false;
                            break;
                        }
                    }
                
                if (kontrol)
                    {
                        Console.WriteLine("Aranılan hesap numarası bulunamadı!");
                    }
                }
                else
                {                
                Console.WriteLine("yanlış hesap numarası girdiniz!");
                }
                
            Console.ReadLine();
        }
        //4. işlem
        public static void paraTransferi()
        {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;
            yillikParaArtisiniGoster = false;

            uint cekilecek_hNo = 0; //para cekilecek hesap no
            uint yatilacak_hNo = 0; //para yatırmak istenilen hesap no için tanımladık

            double cekilecekMiktar = 0;
            double yatanMiktar = 0;
                                
            bool kontrol = true;
            bool kontrol1 = true;
            bool kontrol2 = true; //paranın 2. transfer işleminde de 0'dan büyük olduğunu kontrol etmek için tanımladık

            Console.WriteLine("Para transferi");
            Console.WriteLine();
            Console.Write("Paranın çekileceği hesap no giriniz : ");
            uint.TryParse(Console.ReadLine(), out cekilecek_hNo);
            Console.Write("Paranın yatırılacağı hesap no giriniz : ");
            uint.TryParse(Console.ReadLine(), out yatilacak_hNo);
            Console.Write("Transfer edilecek para miktarını giriniz : ");
            double.TryParse(Console.ReadLine(), out cekilecekMiktar);
            Console.WriteLine();
            

            if (cekilecek_hNo != 0)
            {
                foreach (vadesizHesap vh in hesapListesi)
                {
                    if (cekilecek_hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                    {
                        gondereninAdi=vh.adiSoyadi;
                        int index = hesapListesi.IndexOf(vh);
                        if (cekilecekMiktar > 0 && cekilecekMiktar<vh.hesapBakiyesi)
                        {
                            vh.hesapBakiyesi -= cekilecekMiktar;
                        }
                        else
                        {
                            kontrol = false;
                            kontrol2 = false;
                            Console.WriteLine("Transfer edilecek para miktarı sıfırdan büyük ve hesap bakiyesinden küçük olmalıdır.");
                            break;
                        }
                        hesapListesi[index] = vh;
                        yatanMiktar = cekilecekMiktar;
                        Console.WriteLine(cekilecek_hNo + " Nolu hesaptan " + cekilecekMiktar + " TL başarılı bir şekilde çekilmiştir.");
                        Console.WriteLine("Hesapta ki güncel para durumunu : " + vh.hesapBakiyesi + " TL'dir");
                        Console.WriteLine();                        
                        kontrol = false;
                        break;
                    }
                }
                if (kontrol)
                {
                    Console.WriteLine("Aranılan hesap numarası bulunamadı!");
                }
            }
            else
            {
                Console.WriteLine("yanlış hesap numarası girdiniz!");
            }
            
            if (yatilacak_hNo != 0)
            {
                foreach (vadesizHesap vh in hesapListesi)
                {
                    if (yatilacak_hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                    {
                        kontrol1 = false;
                        if (kontrol2)
                        {
                            int index = hesapListesi.IndexOf(vh);
                            vh.hesapBakiyesi += yatanMiktar;                            
                            hesapListesi[index] = vh;
                            Console.WriteLine(yatilacak_hNo + " Nolu hesaba "+ yatanMiktar + " TL başarılı bir şekilde transfer edilmiştir.");                          
                            Console.WriteLine("Hesapta ki güncel para durumunu : " + vh.hesapBakiyesi + " TL'dir");
                            Console.WriteLine("<***************************************************>");
                            Console.WriteLine("Gönderen kişinin adı : " + gondereninAdi);
                            Console.WriteLine("Alıcı kişinin adı : " + vh.adiSoyadi);
                            break;
                        }                                                                      
                    }
                }
                if (kontrol1) 
                {
                    Console.WriteLine("Transfer edilecek hesap numarası bulunamadı!");
                }

            }
            else
            {
                Console.WriteLine("Yanlış hesap numarası girdiniz!");
            }

            Console.ReadLine();
        }
        //5. işlem
        public static void istenilenHesabiGoruntule()
        {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;            
            yillikParaArtisiniGoster = false;           

            uint hNo = 0; //Görüntülemek istediğimiz hesap numarası için tanımladık           
            bool kontrol = true;

            Console.WriteLine("İstenilen hesabı görüntüleme");
            Console.WriteLine();
            Console.Write("Hesap no giriniz : ");
            uint.TryParse(Console.ReadLine(), out hNo);
            Console.WriteLine();

            if (hNo != 0)
            {
                
                foreach (vadesizHesap vh in hesapListesi)
                {
                    if (hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                    {
                        Console.Write(vadesizHesap.bankaAdi); //const olduğu için class adı ile çağırıyoruz
                        Console.WriteLine("--İstenilen Hesabın Bilgileri--");

                        Console.WriteLine("\n");
                        Console.WriteLine("Hesap No : " + vh.hesapNo);
                        Console.WriteLine("Adı soyadı : " + vh.adiSoyadi);
                        Console.WriteLine("Tc No : " + vh.tcNo);
                        Console.WriteLine("Hesap Bakiyesi : " + vh.hesapBakiyesi);
                        Console.WriteLine("Açılış Tarihi : " + vh.hesapAcilisTarihi);
                        Console.WriteLine("Vade Başlangıcı : " + vh.vadeBaslangici);
                        Console.WriteLine("Vade Tipi : " + vh.hesapVadesi);
                        if(vh.hesapVadesi==vadeTipi.aylik)
                        {
                            Console.WriteLine("*********************************************************");
                            Console.WriteLine("Güncel faizli hesap bakiyesi : " + aylikYeniHesapBakiyesi);
                        }
                        else if(vh.hesapVadesi==vadeTipi.yillik)
                        {
                            Console.WriteLine("*********************************************************");
                            Console.WriteLine("Güncel faizli hesap bakiyesi : " + yillikYeniHesapBakiyesi);
                        }                      
                        
                        kontrol = false;
                        break;
                    }
                }
                if (kontrol)
                {
                    Console.WriteLine("Aranılan hesap numarası bulunamadı!");
                }

            }
            else
            {
                Console.WriteLine("yanlış hesap numarası girdiniz!");
            }
                       
            Console.ReadLine();
        }
        //6. işlem
        public static void hesaplariListele()
        {
            vadesizParaArtisiniGoster = false;
            aylikParaArtisiniGoster = false;
            yillikParaArtisiniGoster = false;
            Console.Write(vadesizHesap.bankaAdi); //const olduğu için class adı ile çağırıyoruz
            Console.WriteLine("--Hesapların Listesi--");        
                      
            foreach (vadesizHesap vh in hesapListesi)
            {
                
                Console.WriteLine("\n");
                Console.WriteLine("Hesap No : " + vh.hesapNo);
                Console.WriteLine("Adı soyadı : " + vh.adiSoyadi);
                Console.WriteLine("Tc No : " + vh.tcNo);
                Console.WriteLine("Hesap Bakiyesi : " + vh.hesapBakiyesi); //İlk yatırılan hesap bakiyesi
                Console.WriteLine("Açılış Tarihi : " + vh.hesapAcilisTarihi);
                Console.WriteLine("Vade Başlangıcı : " + vh.vadeBaslangici);
                Console.WriteLine("Vade Tipi : " + vh.hesapVadesi);
                
                if (vh.hesapVadesi == vadeTipi.aylik)
                {
                    Console.WriteLine("*********************************************************");
                    Console.WriteLine("Güncel faizli hesap bakiyesi : " + aylikYeniHesapBakiyesi); //Listelendiği anda parada ki değişim için buraya bak             
                }
                else if (vh.hesapVadesi == vadeTipi.yillik)
                {
                    Console.WriteLine("*********************************************************");
                    Console.WriteLine("Güncel faizli hesap bakiyesi : " + yillikYeniHesapBakiyesi);
                }                

            }
           Console.ReadLine();

        }
        //7. işlem
        public static void dosyayaKaydet()
        {
            string dosyaYolu = "C:/Users/user/Desktop/YAZILIM-DENİZ/Yazılım-Kursu/Yazılım-7.Hafta/GenelBankaUygulaması/KisiBilgileri.txt";
            if (File.Exists(dosyaYolu))
            {
                StreamWriter Yaz = new StreamWriter(dosyaYolu);
                
                foreach (vadesizHesap vh in hesapListesi)
                {   Yaz.WriteLine("Hesap No : " + vh.hesapNo);
                    Yaz.WriteLine("Adı soyadı : " + vh.adiSoyadi);
                    Yaz.WriteLine("Tc No : " + vh.tcNo);
                    Yaz.WriteLine("Hesap Bakiyesi : " + vh.hesapBakiyesi); //İlk yatırılan hesap bakiyesi
                    Yaz.WriteLine("Açılış Tarihi : " + vh.hesapAcilisTarihi);
                    Yaz.WriteLine("Vade Başlangıcı : " + vh.vadeBaslangici);
                    Yaz.WriteLine("Vade Tipi : " + vh.hesapVadesi);
                    if (vh.hesapVadesi == vadeTipi.aylik)
                    {
                        Yaz.WriteLine("*********************************************************");
                        Yaz.WriteLine("Güncel faizli hesap bakiyesi : " + aylikYeniHesapBakiyesi); //Listelendiği anda parada ki değişim için buraya bak             
                    }
                    else if (vh.hesapVadesi == vadeTipi.yillik)
                    {
                        Yaz.WriteLine("*********************************************************");
                        Yaz.WriteLine("Güncel faizli hesap bakiyesi : " + yillikYeniHesapBakiyesi);
                    }
                    Yaz.WriteLine("\n");
                }
                    Yaz.Close();
                Console.WriteLine("Dosyaya yazdırma işlemi başarılı.");
            }
            else
            {
                Console.WriteLine("Aranılan dosya bulunamadı!");
            }
            Console.ReadLine();
        }
        //8. işlem
        public static void dosyadanOku() 
        {
            string dosyaYolu = "C:/Users/user/Desktop/YAZILIM-DENİZ/Yazılım-Kursu/Yazılım-7.Hafta/GenelBankaUygulaması/KisiBilgileri.txt";
            if (File.Exists(dosyaYolu))
            {
                StreamReader Oku = new StreamReader(dosyaYolu, Encoding.UTF8); //Metin belgesinin formatını belirtiyoruz

                while(!Oku.EndOfStream) //Akışın sonu değilse devam et
                    {
                    Console.WriteLine(Oku.ReadLine());
                    }
                Oku.Close();
            }else
            {
                Console.WriteLine("Aranılan dosya bulunamadı!");
            }
            Console.ReadLine();
        }
        //9. işlem
        public static void hesapSil()
        {
            vadesizParaArtisiniGoster = false; 
            aylikParaArtisiniGoster = false; 
            yillikParaArtisiniGoster = false;  
            uint hNo = 0; //Silinecek hesap no için tanımladık           
            bool kontrol = true;

            Console.WriteLine("Hesap Silme");
            Console.WriteLine();
            Console.Write("Hesap no giriniz : ");
            uint.TryParse(Console.ReadLine(), out hNo);          
            Console.WriteLine();

            if (hNo != 0)
            {
                foreach (vadesizHesap vh in hesapListesi)
                {
                    if (hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                    {
                        //2 yollada nesneyi silebiliriz
                        //int index = hesapListesi.IndexOf(vh);
                        //hesapListesi.RemoveAt(index);
                        hesapListesi.Remove(vh);
                        kontrol = false;
                        Console.WriteLine("Hesap silme işlemi başarı ile gerçekleştirilmiştir.");
                        break;                       
                    }
                }
                if (kontrol)
                {
                    Console.WriteLine("Aranılan hesap numarası bulunamadı!");
                }

            }
            else
            {
                Console.WriteLine("yanlış hesap numarası girdiniz!");
            }

            Console.ReadLine();
        }
        //10. işlem
        
        public static void faizArtisiniGor()
        {
            uint hNo = 0; //faiz artışını görmek istediğimiz hesap no           
            bool kontrol = true;

            Console.WriteLine("Faiz artışını gör");
            Console.WriteLine();
            Console.Write("Hesap no giriniz : ");
            uint.TryParse(Console.ReadLine(), out hNo);
            
            Console.WriteLine();

            if (hNo != 0)
            {
                foreach (vadesizHesap vh in hesapListesi)
                {
                    if (hNo == vh.hesapNo) //hesap noları uyuşuyorsa o kişinin nesnesini al
                    {
                        if (vh.hesapVadesi == vadeTipi.vadesiz)
                        {
                            vadesizParaArtisiniGoster = true;
                        }else if (vh.hesapVadesi == vadeTipi.aylik)
                        {
                            aylikParaArtisiniGoster = true;
                        }else if (vh.hesapVadesi == vadeTipi.yillik)
                        {
                            yillikParaArtisiniGoster = true;
                        }else
                        {
                            vadesizParaArtisiniGoster = false;
                            aylikParaArtisiniGoster = false;
                            yillikParaArtisiniGoster = false;
                        }


                        kontrol = false;
                        break;
                    }
                }
                if (kontrol)
                {
                    Console.WriteLine("Aranılan hesap numarası bulunamadı!");
                }
            }
            else
            {
                Console.WriteLine("yanlış hesap numarası girdiniz!");
            }
                      
            Console.ReadLine();

        }

    }
        
}


