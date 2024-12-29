using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Project_Polished_Version.Classes
{
    public class ConnectionClass
    {
        public static string ConnectionString()
        {
          return "Server=10.0.4.121;Database=jsm_database;User ID=AdrianAdmin;Password=password124;SslMode=none;";
        }

    }
}
