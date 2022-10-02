using MobileApp.Models;
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
    public partial class TermEdit : ContentPage
    {
        public TermEdit(Term term)
        {
            InitializeComponent();

            //Populate Controls
            TermId.Text = term.Id.ToString();
            TermTitle.Text = term.Title;
            StartDate.Date = term.StartDate;
            EndDate.Date = term.EndDate;
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
            if (StartDate.Date < DateTime.Today || EndDate.Date < DateTime.Today)
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

        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var id = int.Parse((TermId.Text));

            await DatabaseService.DeleteTerm(id);
            await Navigation.PopAsync();
        }
    }
}