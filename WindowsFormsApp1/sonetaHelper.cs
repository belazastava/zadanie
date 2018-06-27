using Soneta.Business;
using Soneta.Business.App;
using Soneta.CRM;
using Soneta.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.CRM.Wizytowki;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static partial class SonetaHelper
    {
        public static Database baza;
        public static Login login;

        public static void Init(string nazwaBazy, string user, string pass)
        {
            baza = BusApplication.Instance[nazwaBazy];
            login = baza.Login(false, user, pass);
        }


        //===================================================================================
        //
        // Metoda, która tworzy w systemie wizytówki w oparciu o dane przesłane w formularzu
        //
        //===================================================================================

        public static void CreateWizytowki(Zapytania wszystkieZapytania)
        {
            int licznik = 0;    //będzie zliczać ilość tworzonych wizytówek

            foreach (Zapytanie zap in wszystkieZapytania)
            {
                using (Session session = login.CreateSession(false, true))
                {
                    CRMModule crm = CRMModule.GetInstance(session);
                    CoreModule core = CoreModule.GetInstance(session);

                    using (ITransaction trans = session.Logout(true))
                    {
                        // Cecha słownikowa, która przechowuje rodzaj podmiotu jaki wysłał zapytanie
                        string dictCategory = "F.Podmiot";
                        List<string> Podmioty = new List<string>();
                        foreach (var item in core.Business.Dictionary.WgDataContext[null, dictCategory])
                        {
                            Podmioty.Add(item.Value);
                        }

                        // Sprawdzenie czy przypadkiem nie przesłano zapytania, o id które już istnieje w systemie
                        bool jestWizytowka = false;
                        foreach (KontaktOsoba kow in crm.KontaktyOsoby)
                            if (kow.Features["ID_Zapytanie"].ToString() == zap.id)
                            {
                                jestWizytowka = true;
                                break;
                            }

                        if (jestWizytowka)
                            continue;


                        KontaktOsobaWizytowka kontaktOsobaWizytowka = new KontaktOsobaWizytowka();
                        crm.KontaktyOsoby.AddRow(kontaktOsobaWizytowka);

                        int.TryParse(zap.id, out int idZam);

                        //Wypełnienie pól kontaktu osoby
                        kontaktOsobaWizytowka.Imie = zap.imie;
                        kontaktOsobaWizytowka.Nazwisko = zap.nazwisko;
                        kontaktOsobaWizytowka.Kontakt.TelefonKomorkowy = zap.telefon;
                        kontaktOsobaWizytowka.Kontakt.EMAIL = zap.email;
                        kontaktOsobaWizytowka.ZgodnoscGIODOPotwierdzona = true;
                        kontaktOsobaWizytowka.Stanowisko = "";
                        kontaktOsobaWizytowka.Adres.Telefon = "";
                        kontaktOsobaWizytowka.Features["ID_Zapytanie"] = idZam;
                        kontaktOsobaWizytowka.WizytowkaFirmy.NazwaFirmy = zap.firma;
                        kontaktOsobaWizytowka.WizytowkaFirmy.NIP = "";
                        kontaktOsobaWizytowka.Adres.NrLokalu = "";
                        kontaktOsobaWizytowka.Adres.NrDomu = "";
                        kontaktOsobaWizytowka.Adres.Ulica = zap.ulica_nr;
                        kontaktOsobaWizytowka.Adres.Miejscowosc = zap.miejscowosc;
                        kontaktOsobaWizytowka.Adres.KodPocztowyS = zap.kod_pocztowy;
                        kontaktOsobaWizytowka.Adres.Poczta = "";
                        kontaktOsobaWizytowka.Adres.fillGminy();
                        kontaktOsobaWizytowka.Adres.fillPowiaty();
                        kontaktOsobaWizytowka.Adres.retrieveWojewodztwo();
                        kontaktOsobaWizytowka.Adres.Kraj = zap.kraj;
                        kontaktOsobaWizytowka.Adres.KodKraju = "PL";
                        kontaktOsobaWizytowka.WizytowkaFirmy.WWW = "www." + zap.firma + ".pl";
                        kontaktOsobaWizytowka.Uwagi = "Kontakt: " + zap.droga_kontaktu
                            + Environment.NewLine + "Oferta: " + zap.katalogi;
                        if (Podmioty.Contains(zap.rodzaj_podmiotu))
                            kontaktOsobaWizytowka.Features["Podmiot"] = zap.rodzaj_podmiotu;
                        else
                            kontaktOsobaWizytowka.Features["Podmiot"] = Podmioty[5];

                        // Kopiowanie danych z kontaktu do wizytowki firmy. Potrzebne do skojażenia wizytówki z istniejącymi kontrahentami
                        kontaktOsobaWizytowka.WizytowkaFirmy.Adres.Copy(kontaktOsobaWizytowka.Adres);


                        // Dodawanie zgód
                        //================

                        // Jeśli zaznaczono w formularzu zgodę pierwszą, to szuka jej w tabeli definicji oświadczeń GIODO
                        // i na jej podstawie przypisuje oświadczenie GIODO do kontaktu
                        if (zap.zgoda_jedn == "1")
                        {
                            foreach (GIODODefinicjaOświadczenia definicja in core.GIODODefOswiadcz)
                            {
                                if (definicja.Tresc == zap.zgoda_jedn_tresc)
                                {
                                    GIODOOświadczenie gIODO = new GIODOOświadczenie(kontaktOsobaWizytowka, definicja);
                                    core.GIODOOswiadcz.AddRow(gIODO);
                                    gIODO.Oswiadczenie = true;
                                    break;
                                }
                            }

                            /*if (czyJestDefinicja == false)
                            {
                                trescZgody = zap.zgoda_jedn_tresc;
                                oswiadczeniecennik += " - wersja " + zap.id + " zmieniona";
                                GIODODefinicjaOświadczenia nowaDefinicja = new GIODODefinicjaOświadczenia();
                                nowaDefinicja = AddDefinicjaOświadczenie(session, oswiadczeniecennik);
                                trans.CommitUI();
                                session.InvokeSaved();
                                GIODOOświadczenie gIODO = new GIODOOświadczenie(kontaktOsobaWizytowka, nowaDefinicja);
                                core.GIODOOswiadcz.AddRow(gIODO);
                                gIODO.Oswiadczenie = true;
                            }*/

                            /*
                            foreach (GIODODefinicjaOświadczenia oswiadczenie in gIODOOświadczenia)
                            {
                                Console.WriteLine(oswiadczenie.ToString() + "  ");
                            }*/
                        }

                        // Jeśli zaznaczono w formularzu zgodę drugą, to szuka jej w tabeli definicji oświadczeń GIODO
                        // i na jej podstawie przypisuje oświadczenie GIODO do kontaktu
                        if (zap.zgoda_wielo == "1")
                        {
                            foreach (GIODODefinicjaOświadczenia definicja in core.GIODODefOswiadcz)
                            {
                                if (definicja.Tresc == zap.zgoda_wielo_tresc)
                                {
                                    GIODOOświadczenie gIODO = new GIODOOświadczenie(kontaktOsobaWizytowka, definicja);
                                    core.GIODOOswiadcz.AddRow(gIODO);
                                    gIODO.Oswiadczenie = true;
                                    break;
                                }
                            }

                            /*if (czyJestDefinicja == false)
                            {
                                trescZgody = zap.zgoda_wielo_tresc;
                                oswiadczenienewsletter += " - wersja " + zap.id + " zmieniona";
                                GIODODefinicjaOświadczenia nowaDefinicja = new GIODODefinicjaOświadczenia();
                                nowaDefinicja = AddDefinicjaOświadczenie(session, oswiadczenienewsletter);
                                trans.CommitUI();
                                session.InvokeSaved();
                                GIODOOświadczenie gIODO = new GIODOOświadczenie(kontaktOsobaWizytowka, nowaDefinicja);
                                core.GIODOOswiadcz.AddRow(gIODO);
                                gIODO.Oswiadczenie = true;
                            }*/
                        }
                        licznik++;
                        trans.CommitUI();
                    }
                    session.Save();
                }
            }
            Console.WriteLine("Utworzono " + licznik + " nowych wizytówek.");
        }



        //=========================================================================
        //
        // Metoda sprawdza, czy zgody przesyłane w zapytaniach istnieją w systemie
        // W przypadku jej braku tworzy nową definicje zgody GIODO
        //
        //=========================================================================

        public static void SprawdzDefinicjeZgod(Zapytania wszystkie)
        {
            using (Session session = login.CreateSession(false, true))
            {
                CRMModule crm = CRMModule.GetInstance(session);
                CoreModule core = CoreModule.GetInstance(session);

                using (ITransaction trans = session.Logout(true))
                {
                    foreach (Zapytanie zap in wszystkie)
                    {
                        string oswiadczeniecennik = "Zgoda na przetwarzanie danych osobowych w celu otrzymania cennika";
                        string oswiadczenienewsletter = "Zgoda na otrzymywanie informacji handlowych drogą elektroniczną";

                        bool jestDefinicjaCennik = false;
                        bool jestDefinicjaNewsletter = false;

                        // Sprawdza czy w tabeli definicji oświadczeń GIODO istnieje zgoda_jedn o takiej samej treści jak w przesłanym formularzu
                        if (zap.zgoda_jedn == "1")
                        {
                            foreach (GIODODefinicjaOświadczenia definicja in core.GIODODefOswiadcz)
                            {
                                if (definicja.Tresc == zap.zgoda_jedn_tresc)
                                {
                                    jestDefinicjaCennik = true;
                                    break;
                                }
                            }

                            // Jeśli jej nie ma to tworzy nową
                            if (jestDefinicjaCennik == false)
                            {
                                oswiadczeniecennik += " - wersja " + zap.id + " zmieniona";
                                AddDefinicjaOświadczenie(session, oswiadczeniecennik, zap.zgoda_jedn_tresc);
                            }
                        }

                        // Sprawdza czy w tabeli definicji oświadczeń GIODO istnieje zgoda_wielo o takiej samej treści jak w przesłanym formularzu
                        if (zap.zgoda_wielo == "1")
                        {
                            foreach (GIODODefinicjaOświadczenia definicja in core.GIODODefOswiadcz)
                            {
                                if (definicja.Tresc == zap.zgoda_wielo_tresc)
                                {
                                    jestDefinicjaNewsletter = true;
                                    break;
                                }
                            }

                            // Jeśli jej nie ma to tworzy nową
                            if (jestDefinicjaNewsletter == false)
                            {
                                oswiadczenienewsletter += " - wersja " + zap.id + " zmieniona";
                                AddDefinicjaOświadczenie(session, oswiadczenienewsletter, zap.zgoda_wielo_tresc);
                            }
                        }
                    }
                    trans.CommitUI();
                }
                session.Save();
            }
        }



        //==================================================================
        //
        // Metoda, która tworzy w systemie nową definicję oświadczenia GIODO
        //
        //==================================================================

        public static void AddDefinicjaOświadczenie(Session session, string oswiadczenie, string trescZgody)
        {
            
        
            CoreModule core = CoreModule.GetInstance(session);

            using (ITransaction trans = session.Logout(true))
            {
                GIODODefinicjaOświadczenia gIODODefinicja = new GIODODefinicjaOświadczenia();
                core.GIODODefOswiadcz.AddRow(gIODODefinicja);

                gIODODefinicja.Oswiadczenie = oswiadczenie;
                gIODODefinicja.Rodzaj = RodzajeOświadczeńGIODO.UdzielenieZgody;
                gIODODefinicja.Pracownik = false;
                gIODODefinicja.OsobaFizyczna = true;
                gIODODefinicja.OsobaKontakowa = true;
                gIODODefinicja.Blokada = false;
                gIODODefinicja.Symbol = "GIODO";
                gIODODefinicja.Tresc = trescZgody;

                trans.CommitUI();
            }
            session.InvokeSaved();
            
        }
    }
}