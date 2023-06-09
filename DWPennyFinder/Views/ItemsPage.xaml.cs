﻿using System;
using System.Collections.Generic;
using DWPennyFinder.ViewModels;
using Xamarin.Forms;
using DWPennyFinder.Models;
using DWPennyFinder.Views;
using System.Globalization;
using DWPennyFinder.Converters;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;

namespace DWPennyFinder.Views
{
    public partial class ItemsPage : ContentPage
    {
        public ICommand FilterItemCommand { get; }
        public ICommand SortItemCommand { get; }

        public ItemsViewModel _viewModel;
        // Public property to store the selected filter
        public string SelectedFilter { get; set; }


        public ItemsPage()
        {
            InitializeComponent();

            _viewModel = new ItemsViewModel();
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;
            Resources.Add("LocationParkConverter", new LocationParkConverter());
            // _viewModel.PropertyChanged += OnItemsViewModelPropertyChanged;
            FilterItemCommand = new Command<ItemDetail>(OnFilterItem);
            //LoadItems();


        }
        private async void OnFilterItem(object obj)
        {
            Console.WriteLine("itemfilterbuttonclicked");
            FilterPage filterPage = new FilterPage(this);
            await PopupNavigation.Instance.PushAsync(filterPage);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            LoadItems();
        }

        public async void CallAppearing()
        {
            this.OnAppearing();
        }

        private async Task LoadItems()
        {
            await _viewModel.ExecuteLoadItemsCommand();
        }

        private async void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem.Item.Collected))
            {
                await _viewModel.ExecuteLoadItemsCommand();
            }
        }

    }


}

namespace DWPennyFinder.Converters
{
    public class LocationParkConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var park = values[0] as string;
            var location = values[1] as string;

            if (string.IsNullOrEmpty(park))
            {
                return location ?? string.Empty;
            }

            if (string.IsNullOrEmpty(location))
            {
                return park ?? string.Empty;
            }

            return $"{park} - {location}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
