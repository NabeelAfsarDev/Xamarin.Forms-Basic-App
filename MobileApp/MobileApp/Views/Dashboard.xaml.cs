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
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private async void AddGadget_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermAdd());
        }

        private async void ViewTerms_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermList());
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }

        private async void AddTerms_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermAdd());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            TermsCollection.ItemsSource = await DatabaseService.GetTerms();
        }

        private async void TermsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new TermEdit(term));
            }
        }

        private async void ViewClass_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var selectedTerm = btn.CommandParameter as Term;
            await Navigation.PushAsync(new CourseList(selectedTerm));
        }
    }
}