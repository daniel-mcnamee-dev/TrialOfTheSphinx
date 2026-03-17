using System.Windows;

namespace TrialOfTheSphinx.UI
{
    // Simple popup dialog used for messages, warnings, hints, etc.
    public partial class Popup : Window
    {
        public Popup(string message, string title = "Message")
        {
            InitializeComponent();

            // Fill in text for the popup
            PopupTitle.Text = title;
            PopupMessage.Text = message;
        }

        // Close the popup when OK is clicked
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
