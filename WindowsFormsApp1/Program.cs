using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Zapytanie zap = new Zapytanie();
            zap.id = "Id";
            zap.firma = "Firma";
            zap.plec = "Pan_Pani";
            zap.imie = "Imie";
            zap.nazwisko = "Nazwisko";
            zap.email = "E-mail";
            zap.kod_pocztowy = "Kod_pocztowy";
            zap.miejscowosc = "Miejscowosc";
            zap.ulica_nr = "Ulica_nr";
            zap.kraj = "Kraj";
            zap.telefon = "Telefon";
            zap.katalogi = "Katalogi";
            zap.droga_kontaktu = "Droga_kontaktu";
            zap.rodzaj_podmiotu = "Rodzaj_podmiotu";
            zap.zgoda_jedn = "Zgoda_jedn";
            zap.zgoda_jedn_tresc = "Zgoda_jedn_tresc";
            zap.zgoda_wielo = "Zgoda_wielo";
            zap.zgoda_wielo_tresc = "Zgoda_wielo_tresc";
            zap.wyslano = "Wyslano";

            string tekstowe = "{\"id\":\"Id\"," +
                "\"firma\":\"Firma\"," +
                "\"plec\":\"Pan_Pani\"," +
                "\"imie\":\"Imie\"," +
                "\"nazwisko\":\"Nazwisko\"," +
                "\"email\":\"Email\"," +
                "\"kod_pocztowy\":\"Kod_pocztowy\"," +
                "\"miejscowosc\":\"Miejscowosc\"," +
                "\"ulica_nr\":\"Ulica_nr\"," +
                "\"kraj\":\"Kraj\"," +
                "\"telefon\":\"Telefon\"," +
                "\"katalogi\":\"Katalogi\"," +
                "\"droga_kontaktu\":\"Droga_kontaktu\"," +
                "\"rodzaj_podmiotu\":\"Rodzaj_podmiotu\"," +
                "\"zgoda_jedn\":\"Zgoda_jedn\"," +
                "\"zgoda_jedn_tresc\":\"Zgoda_jedn_tresc\"," +
                "\"zgoda_wielo\":\"Zgoda_wielo\"," +
                "\"zgoda_wielo_tresc\":\"Zgoda_wielo_tresc\"," +
                "\"wyslano\":\"Wyslano\"}";

            string json = ToJson(zap);
            System.Console.WriteLine(json);

            List<Zapytanie> zap2 = new List<Zapytanie>();
            string test = PobierzDaneGet();
            System.Console.WriteLine(test);
            System.Console.WriteLine(tekstowe);
            zap2 = ToObject(test);
            //System.Console.WriteLine(zap2.id);


        }

        public static string ToJson(Zapytanie zap)
        {
            //serializacja
            MemoryStream msObj = new MemoryStream();
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Zapytanie));
            js.WriteObject(msObj, zap);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            return json;
        }

        public static Zapytania ToObject(string json)
        {
            //deserializacja
            string jejson = json;
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jejson));
            DataContractJsonSerializer deserializersList = new DataContractJsonSerializer(typeof(Zapytania));
            Zapytania zap = (Zapytania)deserializersList.ReadObject(ms);
            ms.Close();

            return zap;
        }

        public static string PobierzDaneGet()
        {
            //pobieranie danych metoda GET
            string json;
            string phase = string.Empty;
            string result = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Bearer", "test-token");
                json = wc.DownloadString("https://chomik.pl/form2/zapytania.php");
            }
            phase = json.Remove(0, 1);
            result = phase.Remove(phase.Length-1, 1);
            
            return result;
        }
        
    }


}
