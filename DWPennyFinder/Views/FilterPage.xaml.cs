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

        private Button _previouslyClickedButton;


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

        private async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            SelectedFilter = ((Button)sender).Text;
            _itemsViewModel.FilterItems(SelectedFilter);
            await PopupNavigation.Instance.PopAsync();
        }
    }

}
