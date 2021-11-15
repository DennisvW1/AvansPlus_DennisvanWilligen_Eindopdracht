using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;


namespace AvansPlus_DennisvanWilligen_Eindopdracht
{
    [Keyless]
    internal class Medewerker : Persoon
    {
        private string mFunctie;
        public static int aantal = 0;

        public Medewerker()
        {
            Console.Write("Voer de functie in van de medewerker: ");
            Functie = Console.ReadLine();
            Medewerker.aantal++;
        }
        
        public string Functie
        {
            get { return mFunctie; }
            set { mFunctie = value; }
        }
        
        public static void Bekijken()
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Medewerkers", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            Program.Fout("Geen medewerkers bekend in de database");
                        }
                        else
                        {
                            Console.WriteLine("1. Bekijk alle medewerkers | 2. Zoeken naar een specifieke naam?");
                            string antwoord = Console.ReadLine();
                            if (antwoord == "1")
                            {
                                while (reader.Read())
                                {
// 0. ID | 1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Functie
                                    Console.WriteLine("ID: #{0}", reader.GetValue(0));
                                    Console.WriteLine("Naam: {0} {1}", reader.GetValue(1), reader.GetValue(2));
                                    Console.WriteLine("Adres: {0}", reader.GetValue(3));
                                    Console.WriteLine("{0} {1}", reader.GetValue(5), reader.GetValue(4));
                                    Console.WriteLine("Telefoonnummer: {0}", reader.GetValue(6));
                                    Console.WriteLine("Email: {0}", reader.GetValue(7));
                                    Console.WriteLine("Leeftijd: {0}", LeeftijdBerekenen(reader.GetValue(8).ToString()));
                                    Console.WriteLine("Functie: {0}\n", reader.GetValue(9));
                                }
                            }
                            else if (antwoord == "2")
                            {
                                Console.WriteLine("Naar welke voornaam wil je zoeken?");
                                string naamZoeken = Console.ReadLine();
                                Bekijken(naamZoeken);
                            }
                            else
                            {
                                Program.Fout("Vekeerde invoer gebruikt");
                            }
                        }
                        connection.Close();
                    }
                }
            }
        }
        public static void Bekijken(string zoeken)
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Medewerkers WHERE Voornaam = '" + zoeken + "' ", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            Program.Fout("Geen medewerkers bekend met deze naam in de database");
                        }
                        else
                        {
                            while (reader.Read())
                            {
// 0. ID | 1. Naam | 2. Prijs Heel | 3. Prijs Half | 4. Prijs Punt | 5. Allergenen | 6. Voorraad 
                                Console.WriteLine("ID: #{0}", reader.GetValue(0));
                                Console.WriteLine("Naam: {0} {1}", reader.GetValue(1), reader.GetValue(2));
                                Console.WriteLine("Adres: {0}", reader.GetValue(3));
                                Console.WriteLine("{0} {1}", reader.GetValue(5), reader.GetValue(4));
                                Console.WriteLine("Telefoonnummer: {0}", reader.GetValue(6));
                                Console.WriteLine("Email: {0}", reader.GetValue(7));
                                Console.WriteLine("Leeftijd: {0}", LeeftijdBerekenen(reader.GetValue(8).ToString()));
                                Console.WriteLine("Functie: {0}\n", reader.GetValue(9));
                            }
                        }
                        connection.Close();
                    }
                }
            }
        }
        public static void Aanmaken()
        {
            bool maken = true;
            while (maken)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB"));
//1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Functie
                    Medewerker med = new Medewerker();
                    SqlCommand cmd;
                    connection.Open();

                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Medewerkers (Voornaam, Achternaam, Adres, Woonplaats, Postcode, Telefoonnummer, Email, Geboortedatum, Functie) VALUES ('" + med.Voornaam + "' , '" + med.Achternaam + "' , '" + med.Adres + "' , '" + med.Woonplaats + "' , '" + med.Postcode + "' , '" + med.Telefoonnummer + "' , '" + med.Email + "' , '" + med.Geboortedatum + "' , '" + med.mFunctie + "')";
                    cmd.ExecuteNonQuery();

                    Program.Goed("{0} {1} is succesvol toegevoegd!", med.Voornaam, med.Achternaam);
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("Wil je nog een medewerker aanmaken? j/n");
                string antwoord = Console.ReadLine().ToLower();
                if (antwoord != "j")
                {
                    maken = false;
                }
            }
        }
        public static void Bewerken()
        {
            bool bewerken = true;
            while (bewerken)
            {
                Bekijken();
                SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB"));
                SqlCommand cmd = new SqlCommand("SELECT * FROM Medewerkers", connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows == false)
                {
                    bewerken = false;
                    connection.Close();
                }
                else
                {
                    connection.Close();
                    Console.WriteLine("Welk # medewerker wil je aanpassen?");
                    int wieAanpassen = 0;
                    int watAanpassen = 0;
                    try
                    {
                        wieAanpassen = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Program.Fout("Foutieve invoer gebruikt");
                    }
                    try
                    {
                        Console.WriteLine("Wat wil je aanpassen?");
                        Console.WriteLine("1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Functie");
                        watAanpassen = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Program.Fout("Foutive invoer gebruikt");
                    }
//1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Functie
                    Console.WriteLine("Voer de nieuwe waarde in: ");
                    string nieuw = Console.ReadLine();
                    connection.Open();
                    cmd = connection.CreateCommand();

                    try
                    {
                        switch (watAanpassen)
                        {
                            case 1:
                                cmd.CommandText = "UPDATE Medewerkers SET Voornaam = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Voornaam");
                                break;
                            case 2:
                                cmd.CommandText = "UPDATE Medewerkers SET Achternaam = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Achternaam");
                                break;
                            case 3:
                                cmd.CommandText = "UPDATE Medewerkers SET Adres = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Adres");
                                break;
                            case 4:
                                cmd.CommandText = "UPDATE Medewerkers SET Woonplaats = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Woonplaats");
                                break;
                            case 5:
                                cmd.CommandText = "UPDATE Medewerkers SET Postcode = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Postcode");
                                break;
                            case 6:
                                cmd.CommandText = "UPDATE Medewerkers SET Telefoonnummer = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Telefoonnummer");
                                break;
                            case 7:
                                cmd.CommandText = "UPDATE Medewerkers SET Email = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Email");
                                break;
                            case 8:
                                cmd.CommandText = "UPDATE Medewerkers SET Geboortedatum = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Geboortedatum");
                                break;
                            case 9:
                                cmd.CommandText = "UPDATE Medewerkers SET Functie = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Functie");
                                break;
                            default:
                                Program.Fout("Foutieve invoer gebruikt");
                                break;
                        }
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();
                    Console.WriteLine("Wil je nog een medewerker aanpassen? j/n");
                    string antwoord = Console.ReadLine().ToLower();
                    if (antwoord != "j")
                    {
                        bewerken = false;
                    }
                }
            }
        }
    }//end class
}//end namespace