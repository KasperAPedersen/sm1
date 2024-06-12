using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal static class CDatabase
    {
        private static string server = "localhost";
        private static string name = "oop2";
        private static string user = "root";
        private static string pword = "";

        static MySqlConnection connection = new MySqlConnection($"SERVER={server};DATABASE={name};UID={user};PWD={pword};Convert Zero Datetime=True");

        public static void Init()
        {
            connection.Open();
        }

        public static void Close()
        {
            connection.Close();
        }

        public static void Write(string _query)
        {
            MySqlCommand cmd = new MySqlCommand(_query, connection);
            cmd.ExecuteReader();

        }

        public static void Write1(string _query)
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(_query, connection);
            cmd.ExecuteReader();
            connection.Close();

        }

        public static List<List<string>> Read1(string _query, List<string> _returns)
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(_query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<List<string>> tmp1 = new List<List<string>>();

            while (reader.Read())
            {
                List<string> tmp = new List<string>();
                foreach (string s in _returns)
                {
                    tmp.Add(reader[s]?.ToString() ?? "Not found");
                }
                tmp1.Add(tmp);
            }
            connection.Close();
            return tmp1;
        }

        public static MySqlDataReader Read(string _query)
        {
            MySqlCommand cmd = new MySqlCommand(_query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }

        public static List<string> GetPostalCodes()
        {
            List<List<string>> results = Read1("SELECT PostalCode FROM city", ["PostalCode"]);
            
            List<string> tmp = new List<string>();
            foreach (List<string> s in results)
            {
                tmp.Add(s[0]);
            }

            return tmp;
        }

        public static List<string> GetSchools()
        {
            List<List<string>> results = Read1("SELECT schoolsName FROM schools", ["schoolsName"]);

            List<string> tmp = new List<string>();
            foreach (List<string> s in results)
            {
                tmp.Add(s[0]);
            }

            return tmp;
        }

        public static List<string> GetJobs()
        {
            List<List<string>> results = Read1("SELECT JobName FROM jobs", ["JobName"]);

            List<string> tmp = new List<string>();
            foreach (List<string> s in results)
            {
                tmp.Add(s[0]);
            }

            return tmp;
        }

        public static int GetJobIndex(string job)
        {
            int tmp = 0;
            CDatabase.Init();
            MySqlDataReader reader = CDatabase.Read($"SELECT JobID FROM jobs WHERE JobName = '{job}'");

            while (reader.Read())
            {
                tmp = (int)reader["JobID"];
            }
            CDatabase.Close();
            return tmp;
        }

        public static int GetEducationIndex(string edu)
        {
            int tmp = 0;
            CDatabase.Init();
            MySqlDataReader reader = CDatabase.Read($"SELECT educationID FROM schools WHERE schoolsName = '{edu}'");

            while (reader.Read())
            {
                tmp = (int)reader["educationID"];
            }
            CDatabase.Close();
            return tmp;
        }
    }
}
