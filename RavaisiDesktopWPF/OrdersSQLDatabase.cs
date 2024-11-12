using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    result = result + reader[0];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " ");
            }
            finally
            {
                reader.Close();
                connect.Close();
            }
            return result;
        }
        static public DataRow[] getRowsArray(string sql)
        {
            MySqlConnection connect = new MySqlConnection();
            connect.ConnectionString = dbconnect;   
            connect.Open();
            MySqlCommand command = new MySqlCommand(sql);
            command.Connection = connect;
            command.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            try
            {
                dt.Load(command.ExecuteReader());//Load the command to a datatable
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + sql);
            }
            DataRow[] rows = dt.AsEnumerable().ToArray();//convert rows of dt to an array    
            connect.Close();
            return rows;
        }
       
    } 
}
