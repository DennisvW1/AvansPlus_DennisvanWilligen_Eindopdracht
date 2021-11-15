using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvansPlus_DennisvanWilligen_Eindopdracht
{ 
    class Klant : Persoon
    {
        public static int aantal = 0;
        private string kType;
        //private static List<Klant> klanten = new List<Klant>();

        public Klant()
        {
            Console.Write("Wat voor type klant is het (Vast of Niet vast) ");
            Type = Console.ReadLine();
            Klant.aantal++;
        }

        public string Type
        {
            get { return kType; }
            set
            {

                if (value == "Vast" || value == "Niet vast")
                {
                    kType = value;
                }
                else
                {
                    Console.Write("Wat voor type klant is het (Vast of Niet vast) ");
                    Type = Console.ReadLine();
                }
            }
        }
        public static void Bekijken()
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Klanten", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            Program.Fout("Geen klanten bekend in de database");
                        }
                        else
                        {
                            Console.WriteLine("1. Bekijk alle klanten | 2. Zoeken naar een specifieke naam?");
                            string antwoord = Console.ReadLine();
                            if (antwoord == "1")
                            {
                                while (reader.Read())
                                {
// 0. ID | 1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Type
                                    Console.WriteLine("ID: #{0}", reader.GetValue(0));
                                Console.WriteLine("Naam: {0} {1}", reader.GetValue(1), reader.GetValue(2));
                                Console.WriteLine("Adres: {0}", reader.GetValue(3));
                                Console.WriteLine("{0} {1}", reader.GetValue(5), reader.GetValue(4));
                                Console.WriteLine("Telefoonnummer: {0}", reader.GetValue(6));
                                Console.WriteLine("Email: {0}", reader.GetValue(7));
                                Console.WriteLine("Leeftijd: {0}", LeeftijdBerekenen(reader.GetValue(8).ToString()));
                                Console.WriteLine("Type: {0}\n", reader.GetValue(9));
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
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Klanten WHERE Voornaam = '" + zoeken + "' ", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            Program.Fout("Geen klanten bekend in de database");
                        }
                        else
                        {
                            while (reader.Read())
                            {
// 0. ID | 1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Type
                                Console.WriteLine("ID: #{0}", reader.GetValue(0));
                                Console.WriteLine("Naam: {0} {1}", reader.GetValue(1), reader.GetValue(2));
                                Console.WriteLine("Adres: {0}", reader.GetValue(3));
                                Console.WriteLine("{0} {1}", reader.GetValue(5), reader.GetValue(4));
                                Console.WriteLine("Telefoonnummer: {0}", reader.GetValue(6));
                                Console.WriteLine("Email: {0}", reader.GetValue(7));
                                Console.WriteLine("Leeftijd: {0}", LeeftijdBerekenen(reader.GetValue(8).ToString()));
                                Console.WriteLine("Type: {0}\n", reader.GetValue(9));
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
                    Klant klan = new Klant();
                    SqlCommand cmd;
                    connection.Open();

                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Klanten (Voornaam, Achternaam, Adres, Woonplaats, Postcode, Telefoonnummer, Email, Geboortedatum, Type) VALUES ('" + klan.Voornaam + "' , '" + klan.Achternaam + "' , '" + klan.Adres + "' , '" + klan.Woonplaats + "' , '" + klan.Postcode + "' , '" + klan.Telefoonnummer + "' , '" + klan.Email + "' , '" + klan.Geboortedatum + "' , '" + klan.Type + "')";
                    cmd.ExecuteNonQuery();

                    Program.Goed("{0} {1} is succesvol toegevoegd!", klan.Voornaam, klan.Achternaam);
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine("Wil je nog een klant aanmaken? j/n ");
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Klanten", connection);
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
                    Console.WriteLine("Welke # klant wil je aanpassen?");
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
                        Console.WriteLine("1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Type");
                        watAanpassen = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Program.Fout("Foutive invoer gebruikt");
                    }

                    //1. Voornaam | 2. Achternaam | 3. Adres | 4. Woonplaats | 5. Postcode | 6. Telefoon nummer | 7. Email | 8. Geboortedatum | 9. Type
                    Console.WriteLine("Voer de nieuwe waarde in: ");
                    string nieuw = Console.ReadLine();
                    cmd = connection.CreateCommand();
                    connection.Open();
                    try
                    {
                        switch (watAanpassen)
                        {
                            case 1:
                                cmd.CommandText = "UPDATE Klanten SET Voornaam = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Voornaam");
                                break;
                            case 2:
                                cmd.CommandText = "UPDATE Klanten SET Achternaam = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Achternaam");
                                break;
                            case 3:
                                cmd.CommandText = "UPDATE Klanten SET Adres = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Adres");
                                break;
                            case 4:
                                cmd.CommandText = "UPDATE Klanten SET Woonplaats = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Woonplaats");
                                break;
                            case 5:
                                cmd.CommandText = "UPDATE Klanten SET Postcode = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Postcode");
                                break;
                            case 6:
                                cmd.CommandText = "UPDATE Klanten SET Telefoonnummer = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Telefoonnummer");
                                break;
                            case 7:
                                cmd.CommandText = "UPDATE Klanten SET Email = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Email");
                                break;
                            case 8:
                                cmd.CommandText = "UPDATE Klanten SET Geboortedatum = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Geboortedatum");
                                break;
                            case 9:
                                cmd.CommandText = "UPDATE Klanten SET Type = '" + nieuw + "' WHERE Id = '" + wieAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Type");
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


                    Console.WriteLine("Wil je nog een klanewerker aanpassen? j/n");
                    string antwoord = Console.ReadLine();
                    if (antwoord != "j")
                    {
                        bewerken = false;
                    }
                }
            }
        }
    } // end class
} // end namespace
