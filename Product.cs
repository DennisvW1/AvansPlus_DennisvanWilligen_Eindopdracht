using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvansPlus_DennisvanWilligen_Eindopdracht
{
    class Product
    {
        public string Naam { get; set; }
        public double PrijsHeel { get; set; }
        public double PrijsHalf { get; set; }
        public double PrijsPunt { get; set; }
        public string Allergenen { get; set; }
        public int Voorraad { get; set; }
        public static int aantal;

        public Product()
        {
            Console.WriteLine("Voer de naam van het product in: ");
            Naam = Console.ReadLine();

            Console.WriteLine("Voer de prijs in voor een hele vlaai: ");
            PrijsHeel = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.WriteLine("Voer de prijs in voor een halve vlaai: ");
            PrijsHalf = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.WriteLine("Voer de prijs in voor een punt van een vlaai: ");
            PrijsPunt = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.WriteLine("Voer de allergenen informatie in: ");
            Allergenen = Console.ReadLine();

            Console.WriteLine("Voer de aantal in voorraad in: ");
            Voorraad = int.Parse(Console.ReadLine());

            Product.aantal++;
        }

        /// <summary>
        /// Code om alle inhoud van de database te bekijken
        /// </summary>
        public static void Bekijken()
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Producten", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            Program.Fout("Geen producten bekend in de database");
                        }
                        else
                        {
                            Console.WriteLine("1. Bekijk alle producten | 2. Zoeken naar een specifieke naam?");
                            string antwoord = Console.ReadLine();
                            if (antwoord == "1")
                            {
                                while (reader.Read())
                                {
// 0. ID | 1. Naam | 2. Prijs Heel | 3. Prijs Half | 4. Prijs Punt | 5. Allergenen | 6. Voorraad
                                    Console.WriteLine("ID: #{0}", reader.GetValue(0));
                                    Console.WriteLine("Naam: {0}", reader.GetValue(1));
                                    Console.WriteLine("Prijs heel: {0}", reader.GetValue(2));
                                    Console.WriteLine("Prijs half: {0}", reader.GetValue(3));
                                    Console.WriteLine("Prijs punt: {0}", reader.GetValue(4));
                                    Console.WriteLine("Allergenen: {0}", reader.GetValue(5));
                                    Console.WriteLine("Voorraad: {0}\n", reader.GetValue(6));
                                }
                            }
                            else if (antwoord == "2")
                            {
                                Console.WriteLine("Naar welke naam wil je zoeken?");
                                string naamZoeken = Console.ReadLine();
                                Bekijken(naamZoeken);
                            }
                        }
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Code voor het bekijken van de inhoud van de database op basis van een specifieke naam
        /// </summary>
        /// <param name="zoeken">
        /// op basis van deze string word er gezocht in de database
        /// </param>
        public static void Bekijken(string zoeken)
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Producten WHERE Naam = '"+ zoeken +"' ", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            Program.Fout("Geen producten bekend met die naam in de database");
                        }
                        else
                        {
                            while (reader.Read())
                            {
// 0. ID | 1. Naam | 2. Prijs Heel | 3. Prijs Half | 4. Prijs Punt | 5. Allergenen | 6. Voorraad
                                Console.WriteLine("ID: #{0}", reader.GetValue(0));
                                Console.WriteLine("Naam: {0}", reader.GetValue(1));
                                Console.WriteLine("Prijs heel: {0}", reader.GetValue(2));
                                Console.WriteLine("Prijs half: {0}", reader.GetValue(3));
                                Console.WriteLine("Prijs punt: {0}", reader.GetValue(4));
                                Console.WriteLine("Allergenen: {0}", reader.GetValue(5));
                                Console.WriteLine("Voorraad: {0}\n", reader.GetValue(6));
                            }
                        }
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Code voor het aanmaken van een nieuw product in de database
        /// </summary>
        public static void Aanmaken()
        {
            bool maken = true;
            while (maken)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB"));
//1. Naam | 2. Prijs heel | 3. Prijs half | 4. Prijs punt | 5. Allergenen | 6. Voorraad
                    Product prod = new Product();
                    SqlCommand cmd;
                    connection.Open();

                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Producten (Naam, PrijsHeel, PrijsHalf, PrijsPunt, Allergenen, Voorraad) VALUES ('" + prod.Naam + "' , '" + prod.PrijsHeel + "' , '" + prod.PrijsHalf + "' , '" + prod.PrijsPunt + "', '" + prod.Allergenen + "', '" + prod.Voorraad+ "')";
                    cmd.ExecuteNonQuery();

                    Program.Goed("{0} is succesvol toegevoegd!", prod.Naam);
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("Wil je nog een product aanmaken? j/n");
                string antwoord = Console.ReadLine().ToLower();
                if (antwoord != "j")
                {
                    maken = false;
                }
            }
        }

        /// <summary>
        /// Code voor het bewerken voor het bewerken van een bestaand product
        /// als er geen product ingevoerd is, word er niet gevraagd wat er aangepast moet worden
        /// </summary>
        public static void Bewerken()
        {
            bool bewerken = true;
            while (bewerken)
            {
                Bekijken();
                SqlConnection connection = new SqlConnection(Connect.ConDB("MvDB"));
                SqlCommand cmd = new SqlCommand("SELECT * FROM Producten", connection);
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
                    Console.WriteLine("Welk # product wil je aanpassen?");
                    int welkeAanpassen = 0;
                    int watAanpassen = 0;
                    try
                    {
                        welkeAanpassen = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Program.Fout("Foutieve invoer gebruikt");
                    }
                    try
                    {
                        Console.WriteLine("Wat wil je aanpassen?");
                        Console.WriteLine("1. Naam | 2. Prijs heel | 3. Prijs half | 4. Prijs punt | 5. Allergenen | 6. Voorraad");
                        watAanpassen = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Program.Fout("Foutive invoer gebruikt");
                    }
//1. Naam | 2. Prijs heel | 3. Prijs half | 4. Prijs punt | 5. Allergenen | 6. Voorraad
                    Console.WriteLine("Voer de nieuwe waarde in: ");
                    string nieuw = Console.ReadLine();
                    cmd = connection.CreateCommand();
                    connection.Open();
                    try
                    {
                        switch (watAanpassen)
                        {
                            case 1:
                                cmd.CommandText = "UPDATE Producten SET Naam = '" + nieuw + "' WHERE Id = '" + welkeAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Naam");
                                break;
                            case 2:
                                cmd.CommandText = "UPDATE Producten SET PrijsHeel = '" + nieuw + "' WHERE Id = '" + welkeAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Prijs heel");
                                break;
                            case 3:
                                cmd.CommandText = "UPDATE Producten SET PrijsHalf = '" + nieuw + "' WHERE Id = '" + welkeAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Prijs half");
                                break;
                            case 4:
                                cmd.CommandText = "UPDATE Producten SET PrijsPunt = '" + nieuw + "' WHERE Id = '" + welkeAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Punt prijs");
                                break;
                            case 5:
                                cmd.CommandText = "UPDATE Producten SET Allergenen = '" + nieuw + "' WHERE Id = '" + welkeAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Allergenen");
                                break;
                            case 6:
                                cmd.CommandText = "UPDATE Producten SET Voorraad = '" + nieuw + "' WHERE Id = '" + welkeAanpassen + "' ";
                                cmd.ExecuteNonQuery();
                                Program.Succes("Voorraad");
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
                    Console.WriteLine("Wil je nog een product aanpassen? j/n");
                    string antwoord = Console.ReadLine().ToLower();
                    if (antwoord != "j")
                    {
                        bewerken = false;
                    }
                }
            }
        }
    }//end class
}// end namespace
