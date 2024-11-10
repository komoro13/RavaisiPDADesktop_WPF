using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavaisiDesktopWPF
{
    class OrdersSQLDatabase
    {
        static public int Post(string sql)
        {
            MySqlConnection connect = new MySqlConnection();
            connect.ConnectionString = dbconnect;
            connect.Open();
            MySqlCommand command = new MySqlCommand(sql);
            command.Connection = connect;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            String result = "";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    result = result + reader.GetString(0);
                }
            }
            finally
            {
                reader.Close();
                connect.Close();
            }
            return 0;
        }
        static string dbconnect = "server=127.0.0.1; User=root; password=;database=ravaisi";
        static public string GetString(string sql)
        {
            MySqlConnection connect = new MySqlConnection();
            connect.ConnectionString = dbconnect;
            connect.Open();
            MySqlCommand command = new MySqlCommand(sql);
            command.Connection = connect;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            string result = "";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    result = result + reader.GetString(0);
                }
            }
            finally
            {
                reader.Close();
                connect.Close();
            }
            return result;
        }

       
    } 
}
