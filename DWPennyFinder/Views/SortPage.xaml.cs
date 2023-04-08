using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DWPennyFinder.Views
{	
	public partial class SortPage : PopupPage
	{
        private ItemsPage _itemsPage;

        public SortPage()
        {
            InitializeComponent();
        }
        public SortPage(ItemsPage itemsPage)
        {
            _itemsPage = itemsPage;
            InitializeComponent();
        }
    }
}

