using System;
using System.Collections.Generic;

namespace AvansPlus_DennisvanWilligen_Eindopdracht
{
    static class Program
    {
        /*
         * onderhouden van medewerkergegevens (naw, functie etc)
         * onderhouden van klantgegevens (naw, etc)
         * onderhouden van productgegevens, de bakkerij verkoopt alleen Limburgse vlaaien (denk aan Aardbeislagroom, Appelkruimel etc.
         */

        static void Main(string[] args)
        {
            //app info laten zien in beginscherm
            AppInfo();

            bool progLoop = true;

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Start menu");
                Console.ResetColor();
                Console.WriteLine("Wat wil je doen?\n" +
                                    " 1: Medewerkers inzien\t\t2: Medewerkers aanmaken\t\t3: Medewerkers wijzigen\n" +
                                    " 4: Klanten inzien\t\t5: Klanten aanmaken\t\t6: Klanten wijzigen\n" +
                                    " 7: Producten inzien\t\t8: Producten aanmaken\t\t9: Producten wijzigen\n" +
                                    "10: Afsluiten");
                string actie = Console.ReadLine();
                /* inloggen(int i, int x
                 * i 1 = bewerken
                 * i 2 = aanmaken
                 * 
                 * x 1 = medewerker
                 * x 2 = klant
                 * x 3 = product
                 */
                switch (actie)
                {
                    case "1":
                        Medewerker.Bekijken();
                        break;
                    case "2":
                        Inloggen(2, 1);
                        break;
                    case "3":
                        Inloggen(1, 1);
                        break;
                    case "4":
                        Klant.Bekijken();
                        break;
                    case "5":
                        Inloggen(2, 2);
                        break;
                    case "6":
                        Inloggen(1, 2);
                        break;
                    case "7":
                        Product.Bekijken();
                        break;
                    case "8":
                        Inloggen(2, 3);
                        break;
                    case "9":
                        Inloggen(1, 3);
                        break;
                    case "10":
                        Console.WriteLine("Het programma wordt nu afgesloten..");
                        progLoop = false;
                        break;
                    default:
                        Fout("Foutive invoer gebruikt, gebruik het corresponderende nummer");
                        break;
                }
            } while (progLoop);
        }
        static void AppInfo()
        {
            Console.WriteLine("|| ======================================== ||");
            Console.WriteLine("|| MultiVlaai\t\tGegevens app        ||\n|| Gemaakt door: Dennis\tAvansPlus C# cursus ||");
            Console.WriteLine("|| ======================================== ||\n");
        }
        public static void Fout(string f)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(f);
            Console.ResetColor();
        }
        public static void Fout(string f, int i)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(f, i);
            Console.ResetColor();
        }
        public static void Goed(string g)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(g);
            Console.ResetColor();
        }
        public static void Goed(string g, string a)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(g, a);
            Console.ResetColor();
        }
        public static void Goed(string g, string a, string b)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(g, a, b);
            Console.ResetColor();
        }
        public static void Succes(string S)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("{0} succesvol aangepast", S);
            Console.ResetColor();
        }
        public static void Inloggen(int i, int x)
        {
            string wachtwoord = "multivlaai";

            string optie = "";
            bool loginLoop = true;
            int pogingen = 3;
            if (i == 1)
            {
                optie = "aan te passen";
            }
            else if (i == 2)
            {
                optie = "aan te maken";
            }

            while(loginLoop)
            {
                Console.WriteLine("Voer het wachtwoord in om gegevens {0}", optie);
                string login = Console.ReadLine();

                if (login != wachtwoord && pogingen <= 1)
                {
                    Fout("Je hebt het wachtwoord 3 maal verkeerd ingevoerd, je keert terug naar het beginscherm");
                    loginLoop = false;
                }
                else if (login != wachtwoord)
                {
                    pogingen--;
                    Fout("Wachtwoord is onjuist ingevoerd, nog {0} pogingen", pogingen);
                }
                else if (login == wachtwoord && pogingen >= 1 && i == 1 && x == 1)
                {
                    loginLoop = false;
                    Medewerker.Bewerken();
                }
                else if (login == wachtwoord && pogingen >= 1 && i == 2 && x == 1)
                {
                    loginLoop = false;
                    Medewerker.Aanmaken();
                }
                else if (login == wachtwoord && pogingen >= 1 && i == 1 && x == 2)
                {
                    loginLoop = false;
                    Klant.Bewerken();
                }
                else if (login == wachtwoord && pogingen >= 1 && i == 2 && x == 2)
                {
                    loginLoop = false;
                    Klant.Aanmaken();
                }
                else if (login == wachtwoord && pogingen >= 1 && i == 1 && x == 3)
                {
                    loginLoop = false;
                    Product.Bewerken();
                }
                else if (login == wachtwoord && pogingen >= 1 && i == 2 && x == 3)
                {
                    loginLoop = false;
                    Product.Aanmaken();
                }
            }
        }
    } //end class
}// end namepace




// code to add ->
// Clear terminal na een handeling
// wachtwoord vervangen met ****
// Verwijder optie
// 