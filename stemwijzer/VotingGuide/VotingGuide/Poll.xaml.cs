using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingGuide.Connection;
using VotingGuide.Contract;
using VotingGuide.Objects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VotingGuide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Poll : ContentPage
    {
        ///<summary>
        ///how do you access your php endpoint (including port and /)
        ///</summary>
        private const string address = "http://145.120.201.160/"; 
        private static string[] questions;
        private static string total;
        private static Party[] parties;
        private int current = 1;
        public static Page instance;
        public Poll()
        {            
            InitializeComponent();

            btnAgree.WidthRequest = Application.Current.MainPage.Width;
            btnDisagree.WidthRequest = Application.Current.MainPage.Width;
            btnDontKnow.WidthRequest = Application.Current.MainPage.Width;
            instance = this;
            LoadData();
        }
        private async void LoadData()
        {
            await Task.Run(() =>
            {
                var sql = new Parser();
                total = sql.GetNoParse(address + "count.php");
                questions = sql.Parse(address + "questions.php");
                parties = Parties.LoadParties(sql.GetObjectArray(address + "answers.php"));
                ShowFirstQuestion();
            });
        }
        private void ShowFirstQuestion()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblQuestion.Text = questions[current - 1];
                lblCount.Text = current.ToString() + "/" + total;
                btnAgree.IsEnabled = true;
                btnDisagree.IsEnabled = true;
                btnDontKnow.IsEnabled = true;
            });
        }
        protected override bool OnBackButtonPressed()
        {
            if (current == 1)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await DisplayAlert("Exit?", "Are you sure you want to exit this page? All progress will be lost!", "Yes", "No"))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Navigation.PopModalAsync();
                        });
                        base.OnBackButtonPressed();
                    }
                });
            }
            else
            {
                Previous();
            }
            return true;
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            var button = ((Button)sender);
            //using ClassId as a generic property instead of creating a custom button just for that... if the id is 0, that means the button clicked is AGREE...
            AddPoint(button.ClassId);
            Next();
        }
        private void AddPoint(string id) //... if the button clicked is AGREE, add 1 point for every party that has answer 0(agree) on the current answer, based on the dictionary of every answer, ordered
        {
            var agrees = parties.Where(x => x.Answers[current.ToString()] == id);
            foreach(Party party in agrees)
            {
                party.AddPoint(current.ToString());
            }
            var disagrees = parties.Where(x => x.Answers[current.ToString()] != id);

        }
        private void RollBackPoint(string _current)
        {
            foreach (Party party in parties)
            {
                party.RemovePoint(_current);
            }
        }
        private void Next()
        {
            if (questions.Length+1 > current)
            {
                current++;
            }
            if (current > questions.Length)
            {
                var party = parties.OrderByDescending(x => x.points).ToArray()[0];
                double percent = (double)party.points / (double)questions.Length;
                var friendlyPercent = (percent * 100).ToString() + "%";
                Navigation.PushAsync(new End(party.party_name, friendlyPercent));
            }
            else
            {
                lblCount.Text = current.ToString() + "/" + total;
                var x = questions.Length;
                lblQuestion.Text = questions[current - 1];
            }
        }
        private void Previous()
        {
            RollBackPoint(current.ToString());
            current--;
            RollBackPoint(current.ToString());
            lblCount.Text = current.ToString() + "/" + total;
            lblQuestion.Text = questions[current - 1];
        }
    }
}