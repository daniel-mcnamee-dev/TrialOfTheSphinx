namespace TrialOfTheSphinx.Core
{
    // Base class for all puzzle types
    public abstract class Puzzle
    {
        // Question shown to the player
        public string Question { get; protected set; }

        // Name of the key rewarded when solved
        public string KeyReward { get; protected set; }

        // The currently selected variant (if using variants)
        public PuzzleVariant ActiveVariant { get; protected set; }

        public Puzzle(string question, string keyReward)
        {
            Question = question;
            KeyReward = keyReward;
        }

        // Each puzzle defines its own answer-checking logic
        public abstract bool CheckAnswer(string answer);

        // Hint support via variants
        public virtual string GetHint() => ActiveVariant?.Hint;
    }
}
