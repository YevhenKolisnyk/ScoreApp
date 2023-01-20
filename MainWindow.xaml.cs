using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ScoreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshSummary()
        {
            try
            {
                if (string.IsNullOrEmpty(this.TeamName1.Text) && string.IsNullOrEmpty(this.TeamName2.Text))
                    this.Result.Text = " ...";
                else
                    this.Result.Text = string.Format("{0} {1}:{2} {3}", this.TeamName1.Text, this.Score1.Text, this.Score2.Text, this.TeamName2.Text);

                string path = ConfigurationManager.AppSettings["scores_path"].ToString();
                File.WriteAllText(path + "scores.txt", this.Result.Text, Encoding.Default);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        private void Minus1_Click(object sender, RoutedEventArgs e)
        {
            short i;
            if (!Int16.TryParse(this.Score1.Text, out i)) i = 0;
            if (i > 0) i--;
            this.Score1.Text = i.ToString();
            this.RefreshSummary();
        }

        private void Minus2_Click(object sender, RoutedEventArgs e)
        {
            short i;
            if (!Int16.TryParse(this.Score2.Text, out i)) i = 0;
            if (i > 0) i--;
            this.Score2.Text = i.ToString();
            this.RefreshSummary();
        }

        private void Plus1_Click(object sender, RoutedEventArgs e)
        {
            short i;
            if (!Int16.TryParse(this.Score1.Text, out i)) i = 0;
            i++;
            this.Score1.Text = i.ToString();
            this.RefreshSummary();
        }

        private void Plus2_Click(object sender, RoutedEventArgs e)
        {
            short i;
            if (!Int16.TryParse(this.Score2.Text, out i)) i = 0;
            i++;
            this.Score2.Text = i.ToString();
            this.RefreshSummary();
        }

        private void TeamName1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.RefreshSummary();
        }

        private void TeamName2_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.RefreshSummary();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            this.TeamName1.Text = "";
            this.TeamName2.Text = "";
            this.Score1.Text = "0";
            this.Score2.Text = "0";
            this.RefreshSummary();
        }

        private void Select1_Click(object sender, RoutedEventArgs e) =>
            this.SelectTeam((dialog, button) => ((o, a) => { this.TeamName1.Text = button.Content.ToString(); dialog.Close(); }));

        private void Select2_Click(object sender, RoutedEventArgs e) => 
            this.SelectTeam((dialog, button) => ((o, a) => { this.TeamName2.Text = button.Content.ToString(); dialog.Close(); }));
        

        private void SelectTeam(Func<SelectTeamDialog, Button, RoutedEventHandler> onTeamClick)
        {
            var dialog = new SelectTeamDialog();
            dialog.Owner = this;
            dialog.WindowState = WindowState.Maximized;
            string path = ConfigurationManager.AppSettings["scores_path"].ToString();
            if (string.IsNullOrEmpty(path) || Directory.Exists(path))
            {
                var teamList = File.ReadAllLines(path + "teams.txt", Encoding.Default);
                foreach (string team in teamList)
                {
                    var button = new Button();
                    button.Content = team;
                    button.Margin = new Thickness(16);
                    button.FontSize = 24;
                    button.Click += onTeamClick(dialog, button);
                    dialog.Space.Children.Add(button);
                }
            }
            dialog.ShowDialog();
        }
    }
}
