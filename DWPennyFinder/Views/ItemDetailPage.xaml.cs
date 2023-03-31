using System.ComponentModel;
using Xamarin.Forms;
using DWPennyFinder.ViewModels;

namespace DWPennyFinder.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
