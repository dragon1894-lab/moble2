using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentPart
{
    internal class DBHelper
    {
        private static string connStr =
           "Server=localhost;Database=djing;Uid=root;Pwd=1111;";
        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }
    }
}
