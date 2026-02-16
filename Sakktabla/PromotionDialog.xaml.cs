using System.Windows;
using System.Windows.Controls;

namespace Sakktabla
{
    public partial class PromotionDialog : Window
    {
        public string SelectedPiece { get; private set; } = "Queen";

        public PromotionDialog()
        {
            InitializeComponent();
        }

        private void Choice_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = (string)((Button)sender).Content;
            this.DialogResult = true;
            this.Close();
        }
    }
}