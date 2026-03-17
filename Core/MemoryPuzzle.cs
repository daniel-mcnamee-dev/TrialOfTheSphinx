using System;
using System.Collections.Generic;

namespace TrialOfTheSphinx.Core
{
    // Memory game puzzle where the player repeats a growing sequence
    public class MemoryPuzzle : Puzzle
    {
        private List<int> _sequence;
        private static readonly Random rng = new Random();

        public MemoryPuzzle(string keyReward) : base("Repeat the sequence.", keyReward)
        {
            _sequence = new List<int>();
        }

        // Read-only access to the internal sequence (for UI)
        public IReadOnlyList<int> Sequence => _sequence.AsReadOnly();

        // Adds one random step (value 0-3) and returns that value
        public int GenerateNextStep()
        {
            int step = rng.Next(0, 4);
            _sequence.Add(step);
            return step;
        }

        // Clear the sequence and start again
        public void Reset()
        {
            _sequence.Clear();
        }

        // How many steps are currently in the sequence
        public int RoundCount => _sequence.Count;

        // Check if a full player sequence matches the stored sequence
        public bool VerifyPlayerSequence(List<int> playerSequence)
        {
            if (playerSequence == null) return false;
            if (playerSequence.Count != _sequence.Count) return false;

            for (int i = 0; i < _sequence.Count; i++)
            {
                if (playerSequence[i] != _sequence[i]) return false;
            }

            return true;
        }

        // Check one step at a specific index
        public bool VerifyPlayerStep(int index, int value)
        {
            if (index < 0 || index >= _sequence.Count) return false;
            return _sequence[index] == value;
        }

        // String answers are not used for this puzzle
        public override bool CheckAnswer(string answer) => false;
    }
}
