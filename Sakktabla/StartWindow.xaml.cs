using System.Windows;

namespace Sakktabla
{
    public partial class StartWindow : Window
    {
        public StartWindow() { InitializeComponent(); }


        private void PlayBot_Click(object sender, RoutedEventArgs e) { StartGame(true); }
        private void PlayPvP_Click(object sender, RoutedEventArgs e) { StartGame(false); }

        private void StartGame(bool isBot)
        {
            MainWindow game = new MainWindow(isBot);
            game.Show();
            this.Close();
        }
    }
}