using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DWPennyFinder.Views;
using System.IO;
using DWPennyFinder.Data;
using DWPennyFinder.Models;
using SQLite;
using System.Diagnostics;

namespace DWPennyFinder
{
    public partial class App : Application
    {
        private static ItemDatabase database;

        public static ItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ItemDatabase(Path.Combine("/Users/jruss/projects/DWPennyFinder/DWPennyFinder.iOS/Database", "item.db3"));
               
                    // Insert initial data into the database
                    InsertInitialData();
                }
                return database;
            }
        }
        public static string DatabasePath
        {
            get
            {
                var basePath = "/Users/jruss/projects/DWPennyFinder/DWPennyFinder.iOS/Database";
                return Path.Combine(basePath, "item.db3");
            }
        }

        private static void InsertInitialData()
        {
            // Open the database connection
            using (var conn = new SQLiteConnection(App.DatabasePath))
            {
                // Create the table if it doesn't exist
                conn.CreateTable<Item>();

                // Check if the Items table already has data
                var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Items");
                if (count == 0)
                {
                    // Add some initial data
                    conn.Insert(new Item { Name = "Item 1", Park = "Description 1" });
                    conn.Insert(new Item { Name = "Item 2", Park = "Description 2" });
                    conn.Insert(new Item { Name = "Item 3", Park = "Description 3" });
                }
            }
        }


        public App ()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new AppShell());
        }

        protected override void OnStart ()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mydatabase.db3");
            var database = new SQLiteService(dbPath);

        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }

    internal class SQLiteService
    {
        private string dbPath;

        public SQLiteService(string dbPath)
        {
            this.dbPath = dbPath;
        }
    }
}

