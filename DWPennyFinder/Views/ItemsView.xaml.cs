using System;
using System.Collections.Generic;
using DWPennyFinder.Models;
using Xamarin.Forms;

namespace DWPennyFinder.Views
{	
	public partial class ItemsView : ContentView
	{	
		public ItemsView ()
		{
			
		}
        private async void OnItemTapped(object sender, EventArgs e)
        {
            var item = BindingContext as Item;
            if (item != null)
            {
                await Navigation.PushAsync(new MapPage(item));
            }
        }

    }

}

