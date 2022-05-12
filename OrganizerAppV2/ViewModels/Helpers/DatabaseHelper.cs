using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;

namespace OrganizerAppV2.ViewModels.Helpers
{
    public class DatabaseHelper
    {

        //It can be const insted of static, but with static it will be easier to access it and modify without having an instance

        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");

        public static bool Insert<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                int rows = connection.Insert(item);
                
                if (rows > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool Update<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Update(item);
                
                if (rows > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Delete(item);
                
                if (rows > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static List<T> Read<T>() where T : new()
        {
            List<T> items;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();
            }

            return items;
        }
    }
}
