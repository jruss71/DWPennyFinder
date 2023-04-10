using System;
using System.Collections.Generic;
using System.Data;
using DWPennyFinder.ViewModels;
using DWPennyFinder.Views;
using Xamarin.Forms;

namespace DWPennyFinder
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));

            Routing.RegisterRoute(nameof(FilterPage), typeof(FilterPage));
        }

    }
}

