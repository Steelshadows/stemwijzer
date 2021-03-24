using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VotingGuide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class End : ContentPage, INotifyPropertyChanged
    {
        public End(string result, string percent)
        {
            Party = result+string.Format(" by {0} percent",percent);
            InitializeComponent();
            btnHome.WidthRequest = Application.Current.MainPage.Width;
            btnOver.WidthRequest = Application.Current.MainPage.Width;
            Navigation.RemovePage(Poll.instance);
            BindingContext = this;
        }
        public string Party { get; set; }
        private void btn_Clicked(object sender, EventArgs e)
        {
            if(((Button)sender).ClassId=="0")
            {
                OnBackButtonPressed();
            }
            else
            {
                Navigation.InsertPageBefore(new Poll(),this);
                Navigation.PopAsync();
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Exit?", "Are you sure you want to exit this page? You will need to take the poll again to see the results!", "Yes", "No"))
                {
                    await Navigation.PopModalAsync();
                }
            });
            return true;
        }
    }
}