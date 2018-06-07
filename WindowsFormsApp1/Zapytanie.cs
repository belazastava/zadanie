using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace WindowsFormsApp1
{
    [DataContract]
    public class Zapytanie
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string firma { get; set; }

        [DataMember]
        public string plec { get; set; }

        [DataMember]
        public string imie { get; set; }

        [DataMember]
        public string nazwisko { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string kod_pocztowy { get; set; }

        [DataMember]
        public string miejscowosc { get; set; }

        [DataMember]
        public string ulica_nr { get; set; }

        [DataMember]
        public string kraj { get; set; }

        [DataMember]
        public string telefon { get; set; }

        [DataMember]
        public string katalogi { get; set; }

        [DataMember]
        public string droga_kontaktu { get; set; }

        [DataMember]
        public string rodzaj_podmiotu { get; set; }

        [DataMember]
        public string zgoda_jedn { get; set; }

        [DataMember]
        public string zgoda_jedn_tresc { get; set; }

        [DataMember]
        public string zgoda_wielo { get; set; }

        [DataMember]
        public string zgoda_wielo_tresc { get; set; }

        [DataMember]
        public string wyslano { get; set; }

        /*public static void PobierzDanePost()
        {
            //pobieranie danych metoda POST
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type","application/x-www-form-urlencoded");
                byte[] byteArray = Encoding.Unicode.GetBytes(postData);
                byte[] byteResult = wc.UploadData("https://chomik.pl/form2/zapytania.php", "POST", byteArray);
                string zeStrony = Encoding.Unicode.GetString(byteResult);
            }
        }*/
    }

    [DataContract]
    public class Zapytania : List<Zapytanie>
    {

    }

}
