using System;
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

namespace DWPennyFinder.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        public ItemsPage()
        {
            InitializeComponent();
            _viewModel = new ItemsViewModel();
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;
            Resources.Add("LocationParkConverter", new LocationParkConverter());
            _viewModel.PropertyChanged += OnItemsViewModelPropertyChanged;

            LoadItems();
           
        }
        private void OnItemsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsCollected")
            {
                Console.WriteLine("Need to refresh the view here");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            _viewModel.ExecuteLoadItemsCommand();

        }

        private async Task LoadItems()
        {
            await _viewModel.ExecuteLoadItemsCommand();
        }

        private async void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem.item.Collected))
            {
                await _viewModel.ExecuteLoadItemsCommand();
                //ItemsListView.RefreshItem(_viewModel.SelectedItem.item.Id);
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
