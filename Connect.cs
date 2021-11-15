using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace AvansPlus_DennisvanWilligen_Eindopdracht
{
    public class Connect
    {

        public static string ConDB(string C)
        {
            //MvDB
            string con = "Data Source=Laptop-Dennis;Initial Catalog="+C+";Integrated Security=True";
            return con;
        }


    }
}
