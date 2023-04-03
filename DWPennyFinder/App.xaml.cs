using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DWPennyFinder.Views;
using System.IO;
using DWPennyFinder.Data;
using DWPennyFinder.Models;
using SQLite;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace DWPennyFinder
{
    public partial class App : Application
    {
        private static ItemDatabase database;

        public static ItemDatabase Database
        {
            get
            {
                

                    //database = new ItemDatabase(Path.Combine("/Users/jruss/projects/DWPennyFinder/DWPennyFinder.iOS/Resources", "item.db3"));

                    
                    // Insert initial data into the database
                    Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                Stream dbStream = assembly.GetManifestResourceStream("DWPennyFinder.item.db3");
                //Stream dbStream = new FileStream("/Users/jruss/projects/DWPennyFinder/DWPennyFinder.iOS/Resources/item.db3", FileMode.Open);
                // Debug.WriteLine("test" + assembly.GetManifestResourceNames());
                //string resourceName = "DWPennyFinder.Resources.item.db3";
                //Stream dbStream = assembly.GetManifestResourceStream(resourceName);
                

                if (dbStream == null)
                {
                    throw new ArgumentException($"Resource  not found.");
                }

                if (!File.Exists(DatabasePath))
                    {
                    
                    FileStream fileStream = File.Create(DatabasePath);
                        dbStream.Seek(0, SeekOrigin.Begin);
                        dbStream.CopyTo(fileStream);
                        dbStream.Close();
                    }
                //else
                //{
                //    File.Deslete(DatabasePath);
                //}
                    //InsertInitialData();
                    database = new ItemDatabase(DatabasePath);
                    return database;

            }
        }
        public static string DatabasePath
        {
            get
            {
                String databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "item.db3");
                return databasePath;
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
                var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Item");
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

