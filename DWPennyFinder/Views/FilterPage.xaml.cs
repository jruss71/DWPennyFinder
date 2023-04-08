using System;
using System.Collections.Generic;
using System.Linq;
using DWPennyFinder.Models;
using DWPennyFinder.ViewModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DWPennyFinder.Views
{
    public partial class FilterPage : PopupPage
    {

        private Button _previouslyClickedFilterButton;
        private Button _previouslyClickedSortButton;

        private string _lastSelectedFilter;
        private string _lastSelectedSort;
        // Public property to store the selected filter
        public string SelectedFilter { get; set; }
        public string SelectedSort { get; set; }
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get previously selected filter and sort values from preferences
            _lastSelectedFilter = Preferences.Get("LastSelectedFilter", null);
            _lastSelectedSort = Preferences.Get("LastSelectedSort", null);

            // Set previously clicked filter button's background color
            if (_lastSelectedFilter != null)
            {
                _previouslyClickedFilterButton = FilterButtonGrid.Children
                    .OfType<Button>()
                    .FirstOrDefault(b => b.Text == _lastSelectedFilter);

                if (_previouslyClickedFilterButton != null)
                {
                    _previouslyClickedFilterButton.BackgroundColor = Color.LightGray;
                }
            }

            // Set previously clicked sort button's background color
            if (_lastSelectedSort != null)
            {
                _previouslyClickedSortButton = SortButtonGrid.Children
                    .OfType<Button>()
                    .FirstOrDefault(b => b.Text == _lastSelectedSort);

                if (_previouslyClickedSortButton != null)
                {
                    _previouslyClickedSortButton.BackgroundColor = Color.LightGray;
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Save selected filter and sort values to preferences
            Preferences.Set("LastSelectedFilter", SelectedFilter);
            Preferences.Set("LastSelectedSort", SelectedSort);
        }
        private async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            if (_previouslyClickedFilterButton != null)
            {
                _previouslyClickedFilterButton.BackgroundColor = Color.Transparent;
            }

            SelectedFilter = ((Button)sender).Text;
            _itemsViewModel.FilterItems(SelectedFilter);

            // Set last clicked filter button
            _previouslyClickedFilterButton = (Button)sender;
            _previouslyClickedFilterButton.BackgroundColor = Color.LightGray;

            // Update last selected filter
            _lastSelectedFilter = SelectedFilter;

            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnSortButtonClicked(System.Object sender, System.EventArgs e)
        {
            if (_previouslyClickedSortButton != null)
            {
                _previouslyClickedSortButton.BackgroundColor = Color.Transparent;
            }

            SelectedSort = ((Button)sender).Text;
            _itemsViewModel.SortItems(SelectedSort);

            // Set last clicked sort button
            _previouslyClickedSortButton = (Button)sender;
            _previouslyClickedSortButton.BackgroundColor = Color.LightGray;

            // Update last selected sort
            _lastSelectedSort = SelectedSort;

            await PopupNavigation.Instance.PopAsync();
        }

    }
}
