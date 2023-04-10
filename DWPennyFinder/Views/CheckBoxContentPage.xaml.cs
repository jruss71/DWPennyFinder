using DWPennyFinder.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DWPennyFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckBoxContentPage : PopupPage
    {

        public CheckBoxContentPage(ObservableCollection<Item> items)
        {
            InitializeComponent();
            listView.ItemsSource = items;

        }
        private void OnDismissButtonClicked(object sender, EventArgs e)
        {
            IsVisible = false;
            Content = null;
        }
        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private async void CheckedDone(object sender, CheckedChangedEventArgs e)
        {
            HapticFeedback.Perform(HapticFeedbackType.Click);
            // get the sender
            var cb = sender as CheckBox;

            // use the BindingContext to get the bound Item
            var item = cb.BindingContext as Item;

            // update the Collected value
            item.Collected = e.Value;

            // save the item to the database
            await App.Database.SaveItemAsync(item);

            //MessagingCenter.Send<CheckBoxContentPage>(this, "RefreshNeeded");
        }

    }
}