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
            List<string> postalCodes = new List<string>();
            CDatabase.Init();
            MySqlDataReader reader = CDatabase.Read($"SELECT PostalCode FROM city");

            while (reader.Read())
            {
                postalCodes.Add(reader["PostalCode"].ToString());
            }
            CDatabase.Close();
            return postalCodes;
        }

        public static List<string> GetSchools()
        {
            List<string> schoolNames = new List<string>();
            CDatabase.Init();
            MySqlDataReader reader = CDatabase.Read($"SELECT schoolsName FROM schools");

            while (reader.Read())
            {
                schoolNames.Add(reader["schoolsName"].ToString());
            }
            CDatabase.Close();
            return schoolNames;
        }

        public static List<string> GetJobs()
        {
            List<string> jobs = new List<string>();
            CDatabase.Init();
            MySqlDataReader reader = CDatabase.Read($"SELECT JobName FROM jobs");

            while (reader.Read())
            {
                jobs.Add(reader["JobName"].ToString());
            }
            CDatabase.Close();
            return jobs;
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
