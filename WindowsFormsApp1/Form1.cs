using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static Dictionary<string, Zapytanie> zapytania { get; set; }
        public static Zapytania wszystkieZapytania = new Zapytania();
        Zapytanie jednoZapytanie = new Zapytanie();

        public Form1()
        {
            InitializeComponent();
            
            zapytania = new Dictionary<string, Zapytanie>();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button4.Enabled = false;
        }


        //serializacja - przekształcenie obiektu Zapytanie na JSON
        public static string NaJson(Zapytanie jednoZapytanie)
        {
            MemoryStream msObj = new MemoryStream();
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Zapytanie));
            js.WriteObject(msObj, jednoZapytanie);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();
            return json;
        }
        

        //deserializacja - przekształcanie JSON na obiekt Zapytania
        public static Zapytania NaObiekt(string json)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer deserializersList = new DataContractJsonSerializer(typeof(Zapytania));
            Zapytania zap = (Zapytania)deserializersList.ReadObject(ms);
            ms.Close();
            return zap;
        }


        //pobieranie danych metodą GET
        public static string PobierzDaneGet()
        {
            string json;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("Bearer", "test-token");
                    json = wc.DownloadString("https://chomik.pl/form2/zapytania.php");
                }
                return json;
            }
            catch (System.Net.WebException ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Connection error", ex);
                throw argEx;
            }
        }


        public static void PobierzDanePost()
        {
            //pobieranie danych metoda POST
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Bearer", "test-token");
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                foreach (Zapytanie jedenZap in wszystkieZapytania)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(NaJson(jedenZap));
                    byte[] byteResult = wc.UploadData("https://chomik.pl/form2/zapytania.php", "POST", byteArray);
                    string zeStrony = Encoding.UTF8.GetString(byteResult);
                }
            }
        }


        /*//Metoda, która daje możliwość modyfikacji zapytania pobranego z bazy
        //zanim trafi ono do systemu enova
        public void ChangeZapytania()
        {
            foreach (string key in zapytania.Keys)
                Console.WriteLine("{0} - {1}", key, zapytania[key] + Environment.NewLine);

            if (zapytania.ContainsKey("24"))
                MessageBox.Show("Obiekt istnieje");
            else
                MessageBox.Show("Obiekt nie istnieje");
        }*/



        //    Przyciski
        //===================


        // --- Przycisk do pobierania zapytań z bazy ---   

        private void ToObject_Click(object sender, EventArgs e)
        {
            //pobranie wszystkich zapytań z bazy i przekształcenie na obiekt
            wszystkieZapytania = NaObiekt(PobierzDaneGet());


            if (wszystkieZapytania.Count == 0)
                MessageBox.Show("Baza zapytań jest pusta :(");
            else
            {
                button4.Enabled = true;
                MessageBox.Show("Wszystkie zapytania zostały pobrane");
                
                //tworzenie słownika
                foreach (Zapytanie zap in wszystkieZapytania)
                {
                    if (!zapytania.ContainsKey(zap.id))
                        zapytania.Add(zap.id, zap);
                }
            }
        }



        // --- Przycisk do tworzenia wizytówek ---

        private void button4_Click(object sender, EventArgs e)
        {
            SonetaHelper.CreateWizytowki(wszystkieZapytania);
            button4.Enabled = false;
        }



        // --- Przycisk do usuwania zapytań z bazy ---

        private void ToJSon_Click(object sender, EventArgs e)
        {
            PobierzDanePost();
            MessageBox.Show("Dziękuję :)  - RODO");
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}