using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenkovNikitaKR
{
    public class ConnectionString
    {
        public static string connectionString()
        {
            string dConnectionString = $"Server=127.0.0.1; Database=masterskay; Uid=root; Pwd=root";

            return dConnectionString;

        }
        public static string path = AppDomain.CurrentDomain.BaseDirectory + @"Photo\";
        // root
        // penkovv
        // pypok_1488pypok_1488
    }
}
