namespace TrialOfTheSphinx.Core
{
    // One version of a puzzle: question, answer, reward, and hint
    public class PuzzleVariant
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string KeyReward { get; set; }
        public string Hint { get; set; }

        public PuzzleVariant(string question, string answer, string keyReward, string hint)
        {
            Question = question;
            Answer = answer;
            KeyReward = keyReward;
            Hint = hint;
        }
    }
}
