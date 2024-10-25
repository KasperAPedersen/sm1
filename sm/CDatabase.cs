using MySql.Data.MySqlClient;
using System.Data.Common;

namespace sm
{
    internal static class CDatabase
    {
        private static readonly string Server = "localhost";
        private static readonly string Name = "oop2";
        private static readonly string User = "root";
        private static readonly string Pword = "";

        private static readonly string Conn = $"SERVER={Server};DATABASE={Name};UID={User};PWD={Pword};Convert Zero Datetime=True;Pooling=True;";

        public static async Task<List<string[]>> Exec(string query)
        {
            await using MySqlConnection connection = new(Conn);

            await connection.OpenAsync();

            await using MySqlCommand cmd = new(query, connection);

            DbDataReader reader = await cmd.ExecuteReaderAsync();
            List<string[]> result = [];

            while (await reader.ReadAsync())
            {
                string[] row = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString() ?? "";
                }
                result.Add(row);
            }

            await reader.CloseAsync();
            return result;
        }

        public static async Task<List<string>> GetPostalCodes()
        {
            List<string[]> results = await Exec("SELECT PostalCode, CityName FROM city");
            return results.Select(s => $"{s[0]} {s[1]}").ToList();
        }

        public static async Task<List<string>> GetSchools()
        {
            List<string[]> results = await Exec("SELECT schoolsName FROM schools");
            return results.Select(s => s[0]).ToList();
        }

        public static async Task<List<string>> GetJobs()
        {
            List<string[]> results = await Exec("SELECT JobName FROM jobs");
            return results.Select(s => s[0]).ToList();
        }

        public static async Task<string> GetJobIndex(string job)
        {
            List<string[]> results = await Exec($"SELECT JobID FROM jobs WHERE JobName = '{job}'");
            return results[0][0];
        }

        public static async Task<string> GetEducationIndex(string edu)
        {
            List<string[]> results = await Exec($"SELECT educationID FROM schools WHERE schoolsName = '{edu}'");
            return results[0][0];
        }

        public static async void AddPostal(string postal, string city)
        {
            List<string[]> results = await Exec($"SELECT PostalCode FROM city;");

            bool alreadyExists = false;
            foreach (string[] s in results)
            {
                foreach(string item in s) if (item == postal) alreadyExists = true;
            }

            if(!alreadyExists) await Exec($"INSERT INTO city (PostalCode, CityName) VALUES ('{postal}', '{city}');");
        }

        public static async void AddJob(string job)
        {
            List<string[]> results = await Exec($"SELECT JobName FROM jobs;");

            bool alreadyExists = false;
            foreach (string[] s in results)
            {
                foreach(string item in s) if (item == job) alreadyExists = true;
            }

            if(!alreadyExists) await Exec($"INSERT INTO jobs (JobName) VALUES ('{job}');");
        }

        public static async void AddEducation(string edu)
        {
            List<string[]> results = await Exec($"SELECT schoolsName FROM schools;");

            bool alreadyExists = false;
            foreach (string[] s in results)
            {
                foreach(string item in s) if (item == edu) alreadyExists = true;
                
            }

            if(!alreadyExists) await Exec($"INSERT INTO schools (schoolsName) VALUES ('{edu}');");
        }
    }
}
