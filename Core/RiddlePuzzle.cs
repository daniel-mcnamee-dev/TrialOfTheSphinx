using System;
using System.Collections.Generic;

namespace TrialOfTheSphinx.Core
{
    // Puzzle that randomly selects one riddle from a list
    public class RiddlePuzzle : Puzzle
    {
        // List containing all possible riddle variants
        public List<PuzzleVariant> Variants { get; }

        public RiddlePuzzle(List<PuzzleVariant> variants)
            : base("", "") // Question and reward are set from ActiveVariant
        {
            Variants = variants ?? throw new ArgumentNullException(nameof(variants));
            SelectRandomVariant();
        }

        // Choose a random riddle
        public void SelectRandomVariant()
        {
            Random rnd = new Random();
            ActiveVariant = Variants[rnd.Next(Variants.Count)];
            Question = ActiveVariant.Question;
            KeyReward = ActiveVariant.KeyReward;
        }

        // Case-insensitive answer checking
        public override bool CheckAnswer(string answer)
        {
            return answer.Trim().ToLower() == ActiveVariant.Answer.Trim().ToLower();
        }
    }
}
