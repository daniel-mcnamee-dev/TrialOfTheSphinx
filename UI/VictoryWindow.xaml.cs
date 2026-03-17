using System.Windows;

namespace TrialOfTheSphinx.UI
{
    // Window shown when the player wins the game
    public partial class VictoryWindow : Window
    {
        public VictoryWindow() => InitializeComponent();

        // Exit the entire application
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Return to main menu to play again
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }
    }
}
