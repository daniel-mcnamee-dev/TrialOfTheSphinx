using System;
using System.Collections.Generic;

namespace TrialOfTheSphinx.Core
{
    // Puzzle that chooses from multiple math question variants
    public class MathPuzzle : Puzzle
    {
        // List containing all possible versions of this math puzzle
        public List<PuzzleVariant> Variants { get; }

        public MathPuzzle(List<PuzzleVariant> variants)
            : base("", "")
        {
            // Ensure we got a valid list
            Variants = variants ?? throw new ArgumentNullException(nameof(variants));

            // Pick a random question to use
            SelectRandomVariant();
        }

        // Choose a random variant from the list
        public void SelectRandomVariant()
        {
            Random rnd = new Random();
            ActiveVariant = Variants[rnd.Next(Variants.Count)];
            Question = ActiveVariant.Question;
            KeyReward = ActiveVariant.KeyReward;
        }

        // Check if player's answer matches the correct answer exactly
        public override bool CheckAnswer(string answer)
        {
            return answer.Trim() == ActiveVariant.Answer.Trim();
        }
    }
}

