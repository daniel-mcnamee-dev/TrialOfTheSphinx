using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TrialOfTheSphinx.Core;

namespace TrialOfTheSphinx.UI
{
    // Dedicated window for the (Simon Says–style) memory puzzle
    public partial class MemoryRoom : Window
    {
        // Buttons mapped to sequence indices (0-3)
        private List<Button> ColoredButtons = new List<Button>();

        // The pattern the player must repeat
        private List<int> Sequence = new List<int>();

        // Player's current position in the sequence
        private int PlayerStep = 0;

        private Random rand = new Random();
        private Player Player;

        // Timer with 2-minute limit
        private DispatcherTimer Timer;
        private int TimeRemaining = 120; // seconds

        public MemoryRoom(Player player)
        {
            InitializeComponent();
            Player = player;

            // Map color buttons to indices
            ColoredButtons.Add(RedButton);    // 0
            ColoredButtons.Add(BlueButton);   // 1
            ColoredButtons.Add(GreenButton);  // 2
            ColoredButtons.Add(YellowButton); // 3

            // Subscribe to click events for all color buttons
            foreach (var btn in ColoredButtons)
                btn.Click += ColoredButton_Click;

            // Show keys the player already has
            UpdateInventoryVisual();

            StartTimer();
            StartNewRound();
        }

        #region Timer

        // Set up and start the countdown timer
        private void StartTimer()
        {
            Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        // Called every second
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeRemaining--;
            TimerBar.Value = TimeRemaining;

            // Change bar color based on remaining time
            if (TimeRemaining > 90)
                TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6FAF6F");   // soft green
            else if (TimeRemaining > 60)
                TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#D4C66A");   // soft yellow
            else if (TimeRemaining > 30)
                TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#C78C4A");   // muted orange
            else
                TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#A34A4A");   // faded red

            // If time runs out, show game over
            if (TimeRemaining <= 0)
            {
                Timer.Stop();
                new Popup("Time's up! You failed to escape.", "Time's Up").ShowDialog();
                GameOverWindow gameOver = new GameOverWindow();
                gameOver.Show();
                this.Close();
            }
        }

        #endregion

        #region Game Logic

        // Start a new round by adding one more step to the sequence
        private async void StartNewRound()
        {
            Sequence.Add(rand.Next(0, 4)); // add a new random step
            PlayerStep = 0;

            await FlashSequence();
        }

        // Visually flash the entire stored sequence to the player
        private async Task FlashSequence()
        {
            SetButtonsEnabled(false);

            foreach (int index in Sequence)
            {
                // Set the color based on the sequence value
                switch (index)
                {
                    case 0: SequenceIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#A34A4A"); break;   // soft red
                    case 1: SequenceIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#4F6D8A"); break;   // dusty blue
                    case 2: SequenceIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#6FAF6F"); break;   // olive green
                    case 3: SequenceIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#D4C66A"); break;   // muted gold-yellow
                }

                await Task.Delay(700); // show the color

                // Short grey gap between flashes so repeated colors are visible
                SequenceIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#666666");
                await Task.Delay(150); // short pause
            }

            // Reset indicator to grey when sequence finished
            SequenceIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#666666");

            PlayerStep = 0;
            SetButtonsEnabled(true);
        }

        // Enable or disable all color buttons
        private void SetButtonsEnabled(bool enabled)
        {
            foreach (var btn in ColoredButtons)
                btn.IsEnabled = enabled;
        }

        // Handle when player clicks one of the color buttons
        private async void ColoredButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int clickedIndex = ColoredButtons.IndexOf(btn);

            // If player clicked the correct next step
            if (clickedIndex == Sequence[PlayerStep])
            {
                PlayerStep++;

                // Player finished the whole sequence correctly
                if (PlayerStep == Sequence.Count)
                {
                    new Popup("Correct sequence!", "Success").ShowDialog();

                    // Reward key once sequence reaches length 5
                    if (Sequence.Count >= 5)
                    {
                        new Popup("You collected the Key of Memory!", "Key Collected").ShowDialog();
                        Player.AddKey("Key of Memory");
                        UpdateInventoryVisual();

                        VictoryWindow victory = new VictoryWindow();
                        victory.Show();
                        this.Close();
                    }
                    else
                    {
                        await Task.Delay(500);
                        StartNewRound(); // next round grows sequence
                    }
                }
            }
            else
            {
                // Wrong step -> reset attempt and replay the sequence
                new Popup("Wrong button! Try again.", "Incorrect").ShowDialog();
                PlayerStep = 0;
                await FlashSequence();
            }
        }

        #endregion

        #region Inventory

        // Update the key icons shown at the bottom of the window
        private void UpdateInventoryVisual()
        {
            Image[] slots = { Slot1, Slot2, Slot3 };
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < Player.Inventory.Count)
                    slots[i].Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/key.png", UriKind.Relative));
                else
                    slots[i].Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/slot.png", UriKind.Relative));
            }
        }

        #endregion
    }
}
