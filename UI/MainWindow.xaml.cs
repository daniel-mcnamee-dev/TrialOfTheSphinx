using System.Collections.Generic;
using System.Windows;
using TrialOfTheSphinx.Core;

namespace TrialOfTheSphinx.UI
{
    // Main menu window where the player enters their name and starts the game
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        // Called when the Start Game button is pressed
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Read and validate the player name from the input box
            string name = PlayerNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                // Show a message if the player did not enter a name
                new Popup("Please enter your name.", "Warning").ShowDialog();
                return;
            }

            // Create the player and main game manager
            Player player = new Player(name);
            GameManager gameManager = new GameManager(player);

            // ------------------------------------------------------------
            // Chamber of Wisdom – Riddle puzzle with multiple variations
            // ------------------------------------------------------------
            var riddleVariants = new List<PuzzleVariant>
            {
                new PuzzleVariant(
                    "I speak without a mouth and hear without ears. I have no body, but I come alive with the wind. What am I?",
                    "echo",
                    "Key Of Wisdom",
                    "Think of something you can hear but not see."
                ),
                new PuzzleVariant(
                    "The more you take, the more you leave behind. What am I?",
                    "footsteps",
                    "Key Of Wisdom",
                    "You leave these behind when walking."
                ),
                new PuzzleVariant(
                    "I am always hungry and will die if not fed, but whatever I touch becomes red. What am I?",
                    "fire",
                    "Key Of Wisdom",
                    "You see me in a fireplace."
                ),
                new PuzzleVariant(
                    "I have keys but no locks. I have space but no room. You can enter but can’t go outside. What am I?",
                    "keyboard",
                    "Key Of Wisdom",
                    "You type on me."
                ),
                new PuzzleVariant(
                    "I can fly without wings. I can cry without eyes. Wherever I go, darkness follows me. What am I?",
                    "cloud",
                    "Key Of Wisdom",
                    "You can see me in the sky."
                ),
                new PuzzleVariant(
                    "What comes once in a minute, twice in a moment, but never in a thousand years?",
                    "m",
                    "Key Of Wisdom",
                    "It's a letter."
                )
            };

            var riddlePuzzle = new RiddlePuzzle(riddleVariants);
            gameManager.AddRoom(new Room("Chamber of Wisdom", riddlePuzzle));

            // ------------------------------------------------------------
            // Chamber of Intellect – Programming-style math puzzles
            // ------------------------------------------------------------
            var mathVariants = new List<PuzzleVariant>
            {
                new PuzzleVariant(
                    "Solve: int x=2; for(int i=0;i<3;i++) x=x*x-1; What is x?",
                    "63",
                    "Key Of Intellect",
                    "Square x, then subtract 1 each loop."
                ),
                new PuzzleVariant(
                    "Compute: int y=1; for(int i=0;i<4;i++) y=y*2+1; What is y?",
                    "31",
                    "Key Of Intellect",
                    "Double y and add 1 each time."
                ),
                new PuzzleVariant(
                    "What is 2^5?",
                    "32",
                    "Key Of Intellect",
                    "2 multiplied by itself 5 times."
                ),
                new PuzzleVariant(
                    "int z=0; for(int i=1;i<=3;i++) z += i*i; What is z?",
                    "14",
                    "Key Of Intellect",
                    "Squares of 1, 2, and 3 summed together."
                ),
                new PuzzleVariant(
                    "Compute: int x=1; for(int i=0;i<4;i++) x = x*2; What is x?",
                    "16",
                    "Key Of Intellect",
                    "Start at 1, double it four times."
                )
            };

            // Create a dynamic programming-style puzzle
            var rng = new System.Random();

            int start = rng.Next(1, 5);     // starting value
            int loops = rng.Next(2, 6);     // number of loop iterations

            int result = start;
            for (int i = 0; i < loops; i++)
                result = result * 2 - 1;

            mathVariants.Add(
                new PuzzleVariant(
                    $"Compute: int x={start}; for(int i=0;i<{loops};i++) x=x*2-1; What is x?",
                    result.ToString(),
                    "Key Of Intellect",
                    "Each loop doubles x, then subtracts 1."
                )
            );

            var mathPuzzle = new MathPuzzle(mathVariants);
            gameManager.AddRoom(new Room("Chamber of Intellect", mathPuzzle));

            // ------------------------------------------------------------
            // Chamber of Memory – Final room of the game
            // ------------------------------------------------------------
            var memoryPuzzle = new MemoryPuzzle("Key Of Memory");
            gameManager.AddRoom(new Room("Chamber of Memory", memoryPuzzle));

            // Open the first puzzle room and close the main menu
            RoomWindow roomWindow = new RoomWindow(gameManager);
            roomWindow.Show();
            this.Close();
        }

        // Exit button on the main menu
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
