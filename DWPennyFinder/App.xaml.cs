﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DWPennyFinder.Services;
using DWPennyFinder.Views;


namespace DWPennyFinder
{
    public partial class App : Application
    {

        public App ()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
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
}

