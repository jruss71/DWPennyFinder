using System;
using System.Collections.Generic;
using System.Linq;
using DWPennyFinder.Models;
using DWPennyFinder.ViewModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DWPennyFinder.Views
{
    public partial class FilterPage : PopupPage
    {

        
        // Public property to store the selected filter
        public string SelectedFilter { get; set; }
        private ItemsPage _itemsPage;
        private ItemsViewModel _itemsViewModel;

        public FilterPage()
        {
            InitializeComponent();
        }

        public FilterPage(ItemsPage itemsPage)
        {
            _itemsPage = itemsPage;
            InitializeComponent();
        }
        public FilterPage(ItemsViewModel itemsViewModel)
        {
            _itemsViewModel = itemsViewModel;
            InitializeComponent();
        }

        private async void OnAnimalKingdomClicked(object sender, EventArgs e)
        {
            SelectedFilter = "Animal Kingdom";
            //_itemsPage.SelectedFilter = SelectedFilter;
            _itemsViewModel.FilterItemsByLocation(SelectedFilter);
            await PopupNavigation.Instance.PopAsync();
        }


        private async void OnEpcotClicked(object sender, EventArgs e)
        {
            SelectedFilter = "Epcot";
            //_itemsPage.SelectedFilter = SelectedFilter;
            _itemsViewModel.FilterItemsByLocation(SelectedFilter);            
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnMagicKingdomClicked(object sender, EventArgs e)
        {
            SelectedFilter = "Magic Kingdom";
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnHollywoodStudiosClicked(object sender, EventArgs e)
        {
            SelectedFilter = "Hollywood Studios";
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnDisneySpringsClicked(object sender, EventArgs e)
        {
            SelectedFilter = "Disney Springs";
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnResortsClicked(object sender, EventArgs e)
        {
            SelectedFilter = "Resorts";
            await PopupNavigation.Instance.PopAsync();
        }
        private async void OnClearFilterClicked(object sender, EventArgs e)
        {
            Console.WriteLine("clear");
        }
        private async void OnClose(object sender, EventArgs e)
        {
            Console.WriteLine("close");
            await PopupNavigation.Instance.PopAsync();
        }
    }

}

