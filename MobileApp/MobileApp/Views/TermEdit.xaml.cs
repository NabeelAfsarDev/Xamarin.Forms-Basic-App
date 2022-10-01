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
            TermTitle.Text = term.Title;
            StartDate.Date = term.StartDate;
            EndDate.Date = term.EndDate;
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

        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            await DatabaseService.DeleteTerm(Convert.ToInt32(TermId.Text));
        }
    }
}