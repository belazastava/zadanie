using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
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
            Soneta.Start.Loader loader = new Soneta.Start.Loader();
            loader.WithExtensions = true;
            loader.Load();
            SonetaHelper.Init("Nowa firma", "Administrator", "");

            //schedule_Timer();
            ZadanieCyklicznie(30000);
            

            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
           */
        }

        /*
        static void schedule_Timer()
        {
            timer = new System.Timers.Timer();
            //timer = new System.Windows.Forms.Timer();
            timer.Interval = 30000;
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //timer.Tick += new EventHandler(timer_Elapsed);
            timer.Start();
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Form1.wszystkieZapytania = Form1.NaObiekt(Form1.PobierzDaneGet());

            if (Form1.wszystkieZapytania.Count != 0)
            {
                SonetaHelper.SprawdzDefinicjeZgod(Form1.wszystkieZapytania);
                SonetaHelper.CreateWizytowki(Form1.wszystkieZapytania);
                Form1.PobierzDanePost();
            }
            else Console.WriteLine("Nic nie pobrano");

            timer.Start();
        }*/

        static void ZadanieCyklicznie(int interval)
        {
            while (true)
            {
                int MINIMUM_TICKS = 0;
                uint startTicks;
                int workTicks, remainingTicks;
                startTicks = (uint)Environment.TickCount;
                Zadanie();
                workTicks = (int)((uint)Environment.TickCount - startTicks);
                remainingTicks = interval - workTicks;
                //if (remainingTicks > 0) Thread.Sleep(remainingTicks);
                Thread.Sleep(Math.Max(remainingTicks, MINIMUM_TICKS));
            }
        }

        static void Zadanie()
        {
            Form1.wszystkieZapytania = Form1.NaObiekt(Form1.PobierzDaneGet());

            if (Form1.wszystkieZapytania.Count != 0)
            {
                SonetaHelper.SprawdzDefinicjeZgod(Form1.wszystkieZapytania);
                SonetaHelper.CreateWizytowki(Form1.wszystkieZapytania);
                Form1.PobierzDanePost();
            }
            else Console.WriteLine("Nic nie pobrano");
        }
    }
}
