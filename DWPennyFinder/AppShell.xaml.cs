using System;
using System.Collections.Generic;
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
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}

