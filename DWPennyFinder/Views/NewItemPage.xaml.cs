using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DWPennyFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DWPennyFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        NewItemViewModel _viewModel;

        public NewItemPage()
        {
            InitializeComponent();

            _viewModel = new NewItemViewModel();
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;
        }
    }
}
