using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal static class CDatabase
    {
        private static readonly string server = "localhost";
        private static readonly string name = "oop2";
        private static readonly string user = "root";
        private static readonly string pword = "";

        private static readonly string conn = $"SERVER={server};DATABASE={name};UID={user};PWD={pword};Convert Zero Datetime=True;Pooling=True;";

        public static async Task<List<string[]>> Exec(string _query)
        {
            using MySqlConnection connection = new(conn);

            await connection.OpenAsync();

            using MySqlCommand cmd = new(_query, connection);

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
            return results[0][0] ?? "0";
        }

        public static async Task<string> GetEducationIndex(string edu)
        {
            Console.Title = edu;
            List<string[]> results = await Exec($"SELECT educationID FROM schools WHERE schoolsName = '{edu}'");
            return results[0][0] ?? "0";
        }

        public static async void AddPostal(string postal, string city)
        {
            await Exec($"INSERT INTO city (PostalCode, CityName) VALUES ('{postal}', '{city}');");
        }

        public static async void AddJob(string job)
        {
            await Exec($"INSERT INTO jobs (JobName) VALUES ('{job}');");
        }

        public static async void AddEducation(string edu)
        {
            await Exec($"INSERT INTO schools (schoolsName) VALUES ('{edu}');");
        }
    }
}
