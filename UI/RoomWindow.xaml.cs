using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrialOfTheSphinx.Core;

namespace TrialOfTheSphinx.UI
{
    // Window used for standard puzzle rooms (riddle + math)
    public partial class RoomWindow : Window
    {
        private GameManager GameManager;
        private TimerSystem Timer;

        // Total time (in seconds) allowed for a room
        private int totalSeconds = 120;

        public RoomWindow(GameManager gameManager)
        {
            InitializeComponent();
            GameManager = gameManager;

            // Load first or current room on open
            LoadRoom();
        }

        // Set up room UI and timer based on the current room
        private void LoadRoom()
        {
            var room = GameManager.CurrentRoom;
            RoomTitle.Text = room.Name;
            PuzzleQuestion.Text = room.Puzzle.Question;

            // Set up a new timer for this room
            Timer = new TimerSystem(totalSeconds);
            Timer.TimeUpdated += Timer_TimeUpdated;
            Timer.TimeExpired += Timer_TimeExpired;
            Timer.Start();

            UpdateInventoryVisual();
        }

        #region Timer

        // Update the timer bar and text each second
        private void Timer_TimeUpdated(TimeSpan remaining)
        {
            Dispatcher.Invoke(() =>
            {
                TimerBar.Value = remaining.TotalSeconds;
                TimerText.Text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";

                double percent = remaining.TotalSeconds / totalSeconds;

                // Change bar color based on percentage left
                if (percent > 0.6)
                    TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6FAF6F");
                else if (percent > 0.4)
                    TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#D4C66A");
                else if (percent > 0.2)
                    TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#C78C4A");
                else
                    TimerBar.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#A34A4A");
            });
        }

        // If timer runs out before puzzle is solved
        private void Timer_TimeExpired()
        {
            Dispatcher.Invoke(() =>
            {
                new Popup("Time's up! Game Over.", "Time's Up").ShowDialog();
                GameOverWindow gameOver = new GameOverWindow();
                gameOver.Show();
                this.Close();
            });
        }

        #endregion

        #region Puzzle Interaction

        // Handle when player presses "Submit Answer"
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var room = GameManager.CurrentRoom;
            bool solved = room.Puzzle.CheckAnswer(AnswerTextBox.Text);

            if (solved)
            {
                // Reward the player with the room's key
                GameManager.Player.AddKey(room.Puzzle.KeyReward);
                UpdateInventoryVisual();

                // Stop timer and allow moving to next room
                Timer.Stop();
                SubmitButton.IsEnabled = false;
                NextRoomButton.IsEnabled = true;

                new Popup("Correct! Proceed to the next room.", "Correct").ShowDialog();
            }
            else
            {
                new Popup("Incorrect answer. Try again!", "Incorrect").ShowDialog();
            }
        }

        // Show a hint, if supported by the current puzzle
        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            var room = GameManager.CurrentRoom;
            string hint = null;

            // Only RiddlePuzzle and MathPuzzle use hints via variants
            switch (room.Puzzle)
            {
                case RiddlePuzzle riddle:
                    hint = riddle.GetHint();
                    break;
                case MathPuzzle math:
                    hint = math.GetHint();
                    break;
            }

            if (!string.IsNullOrEmpty(hint))
                new Popup(hint).ShowDialog();
            else
                new Popup("No hint available for this puzzle.").ShowDialog();
        }

        // Move to the next room (or victory if no more rooms)
        private void NextRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            GameManager.NextRoom();

            // If there are no more rooms, show victory screen
            if (GameManager.CurrentRoom == null)
            {
                VictoryWindow victory = new VictoryWindow();
                victory.Show();
                this.Close();
                return;
            }

            // If the next puzzle is the memory puzzle, open its special window
            if (GameManager.CurrentRoom.Puzzle is MemoryPuzzle)
            {
                MemoryRoom memoryRoom = new MemoryRoom(GameManager.Player);
                memoryRoom.Show();
                this.Close();
                return;
            }

            // Otherwise, open another standard RoomWindow for the next puzzle
            RoomWindow nextRoom = new RoomWindow(GameManager);
            nextRoom.Show();
            this.Close();
        }

        #endregion

        #region Inventory

        // Update key icons below the puzzle based on the player's inventory
        private void UpdateInventoryVisual()
        {
            Image[] slots = { Slot1, Slot2, Slot3 };
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < GameManager.Player.Inventory.Count)
                    slots[i].Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/key.png", UriKind.Relative));
                else
                    slots[i].Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/slot.png", UriKind.Relative));
            }
        }

        #endregion
    }
}
