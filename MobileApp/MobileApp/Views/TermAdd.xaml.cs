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
            //Validate the inputs
            if (String.IsNullOrWhiteSpace(TermTitle.Text))
            {
                await App.Current.MainPage.DisplayAlert("Missing Input", "You must provide a title for the term.", "OK");
                return;
            }
            //Make sure the start date is not greater then the end date
            if (StartDate.Date >= EndDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "The start date can't be greater than the end date", "OK");
                return;
            }
            //Make sure that both start date and end date are greater than todat
            if(StartDate.Date < DateTime.Today || EndDate.Date < DateTime.Today)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "Start or end dates have to be greater than today's date.", "OK");
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