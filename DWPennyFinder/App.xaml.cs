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
using System.Linq;

namespace DWPennyFinder
{
    public partial class App : Application
    {
        private static ItemDatabase database;

        public static ItemDatabase Database
        {
            get
            {

                // Insert initial data into the database
                Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                Stream dbStream = assembly.GetManifestResourceStream("DWPennyFinder.item.db3");


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




        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AppShell());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Get the current Page from the navigation stack
            var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();

            // Check if the current Page is the ItemsViewPage
            if (currentPage is ItemsPage itemsPage)
            {
                // Call the OnAppearing method of the ItemsViewPage
                itemsPage.CallAppearing();
            }
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

