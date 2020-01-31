using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankaUygulamasiClasslari
{
    public enum vadeTipi
    {
        vadesiz=1,
        aylik=2,
        yillik=3,
    }
    public class vadesizHesap
    {
        public uint hesapNo;
        public string adiSoyadi;
        public string tcNo;
        public double hesapBakiyesi;
        public DateTime hesapAcilisTarihi;
        public DateTime vadeBaslangici;
        public vadeTipi hesapVadesi;
        public double gelenFaiz;
        public const string bankaAdi = "DenizBank"; //const değerin public mi değil mi ne olduğu belirtmeliyiz       
        public virtual double faizUygula() //her vade tipi için ezilecek fonksiyon
        {
            
            double gelenFaiz=0;          

            return gelenFaiz; //Vadesiz hesaplarda faiz gelmez 0 TL dönderdik o yüzden
        }

    }

    public class aylikVadeli:vadesizHesap //vadesiz hesaptan miras alıyoruz
    {
        public const double saniyelikAylikFaizOrani = 0.0001;
        
        public override double faizUygula()
        {
            //int gecenSüre;
            //gecenSüre = ((DateTime.Today - vadeBaslangici).Days) / 30;

            double gelenFaiz;
            gelenFaiz = hesapBakiyesi * (saniyelikAylikFaizOrani*2); //2 saniyelik bulduk
            
            return gelenFaiz;
        }

    }
    public class yillikVadeli : vadesizHesap
    {
        public const double saniyelikYillikFaizOrani = 0.0012;
        public override double faizUygula()
        {
            //int gecenSüre;
            //gecenSüre = ((DateTime.Today - vadeBaslangici).Days) / 365;

            double gelenFaiz;           
            gelenFaiz = hesapBakiyesi * (saniyelikYillikFaizOrani*2); //2 saniyelik bulduk

            return gelenFaiz;
        }

    }
    class program
    {
        static void Main(string[] args)
        {

        }
    }
}
