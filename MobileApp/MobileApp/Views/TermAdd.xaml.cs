using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermAdd : ContentPage
    {
        public TermAdd()
        {
            InitializeComponent();
        }

        private void StartDate_DateSelected(object sender, DateChangedEventArgs e)
        {
           
        }

        private void EndDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            if (StartDate.Date >= EndDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "The start date can't be greater than the end date", "OK");
                return;
            }
            await DatabaseService.AddTerm(TermTitle.Text, StartDate.Date, EndDate.Date);
            await Navigation.PopAsync();
        }

        private async void CancelTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}