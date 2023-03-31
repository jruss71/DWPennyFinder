using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using DWPennyFinder.Models;
using Xamarin.Forms;

namespace DWPennyFinder.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string name;
        private string park;
        private string location;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += 
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(park) && !String.IsNullOrWhiteSpace(location);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Park
        {
            get => park;
            set => SetProperty(ref park, value);
        }

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Park = Park,
                Location = Location
            };
            
            await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

