using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Hochbeet_Planer.Data
{
    class DatabaseHelper
    {
        //Name
        private static readonly string DatabaseFileName = "hochbeet.sqlite";

        //der Pfad
        private static readonly string DatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);

        //diese Adresse zur DB verwenden
        public static readonly string ConnectionString = $"Data Source={DatabasePath};Version=3;";
        
        //prüft ob exists und wenn nicht create
        public static void InitializeDatabase()
        {
            if (!File.Exists(DatabasePath))
            {
                SQLiteConnection.CreateFile(DatabasePath);
            }
            CreateTablesIfNotExists();
        }

        private static void CreateTablesIfNotExists()
        {
            //using schlißt die Verbindung automatisch, sicherer als conn.close
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            { 
                conn.Open();

                string sql = @"
                CREATE TABLE IF NOT EXISTS Beete (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Breite INTEGER NOT NULL,
                    Laenge INTEGER NOT NULL
                 );
                CREATE TABLE IF NOT EXISTS BeetBelegung (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    BeetId INTEGER NOT NULL,
                    Zeile INTEGER NOT NULL,
                    Spalte INTEGER NOT NULL,
                    PflanzenName TEXT NOT NULL

                );";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

            }


        }
    }
}
