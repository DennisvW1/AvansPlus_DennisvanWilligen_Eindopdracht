using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace AvansPlus_DennisvanWilligen_Eindopdracht
{

    public abstract class Persoon
    {
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Adres { get; set; }
        public string Woonplaats { get; set; }
        private string pPostcode;
        private string pTelnr;
        private string pEmail;
        private string pGebDatum;
        private int pLeeftijd;

        private static List<Persoon> personen = new List<Persoon>();


        public Persoon()
        {
            Console.Write("Voer de voornaam in: ");
            Voornaam = Console.ReadLine();

            Console.Write("Voer de achternaam in: ");
            Achternaam = Console.ReadLine();

            Console.Write("Voer het adres in: ");
            Adres = Console.ReadLine();

            Console.Write("Voer de woonplaats in: ");
            Woonplaats = Console.ReadLine();

            Console.Write("Voer de postcode in: ");
            Postcode = Console.ReadLine();

            Console.Write("Voer het telefoonnummer in: ");
            Telefoonnummer = Console.ReadLine();

            Console.Write("Voer het email adres in: ");
            Email = Console.ReadLine();

            Console.Write("Voer de geboortedatum in (jjjj-mm-dd): ");
            Geboortedatum = Console.ReadLine();

            DateTime dob = Convert.ToDateTime(pGebDatum);
            var leeftijdBerekenen = DateTime.Today.Year - dob.Year;
            if (dob.Date > DateTime.Today.AddYears(-leeftijdBerekenen)) leeftijdBerekenen--;
            pLeeftijd = leeftijdBerekenen;

        }
        public string Postcode
        {
            get { return pPostcode; }
            set
            {
                if (value.Length == 0)
                {
                    Console.Write("Voer de postcode in: ");
                    Postcode = Console.ReadLine();
                }
                else
                {
                    value.Trim();
                    // ervan uit gaande dat er alleen NL klanten zijn dus een postcode met 6 characters

                    //postcode = postcode.Trim();
                    value = value.Replace(" ", "");

                    int postcode1 = int.Parse(value.Substring(0, 1));
                    int postcode2 = int.Parse(value.Substring(1, 3));
                    bool c1 = Regex.IsMatch(value.Substring(4, 1), "[a-zA-Z]", RegexOptions.IgnoreCase);
                    bool c2 = Regex.IsMatch(value.Substring(5, 1), "[a-zA-Z]", RegexOptions.IgnoreCase);

                    if (value.Length == 6 && postcode1 >= 1 && postcode2 >= 0 && postcode2 >= 9 && c1 && c2)
                    {
                        pPostcode = value;
                    }
                    else
                    {
                        Console.WriteLine("Gebruik de volgende format: 1234AB");
                        Console.Write("Voer de postcode in: ");
                        Postcode = Console.ReadLine();
                    }
                }
            }
        }
        public string Telefoonnummer
        {
            get { return pTelnr; }
            set { pTelnr = value; }
        }
        public string Email
        {
            get { return pEmail; }
            set
            {
                if (value.Contains('@') && value.Contains('.'))
                {
                    pEmail = value;
                }
                else
                {
                    Console.WriteLine("Voer een geldig email adres in!");
                    Email = Console.ReadLine();
                }
            }
        }
        public string Geboortedatum
        {
            get { return pGebDatum; }
            set
            {
                try
                {
                    DateTime dob = Convert.ToDateTime(value);
                    pGebDatum = value;
                }
                catch
                {
                    Console.WriteLine("Foutieve invoer gebruikt (jjjj-mm-dd)");
                    Console.Write("Voer de geboortedatum in: ");
                    Geboortedatum = Console.ReadLine();
                }

            }
        }
        public int Leeftijd
        {
            get { return pLeeftijd; }
        }

        public static int LeeftijdBerekenen(string LB)
        {
            DateTime dob = Convert.ToDateTime(LB);
            var leeftijdBerekenen = DateTime.Today.Year - dob.Year;
            if (dob.Date > DateTime.Today.AddYears(-leeftijdBerekenen)) leeftijdBerekenen--;
            int leeftijd = leeftijdBerekenen;

            return leeftijd;
        }
    }//end class
}// end namespace
