namespace TrialOfTheSphinx.Core
{
    // A room in the temple containing one puzzle
    public class Room
    {
        public string Name { get; private set; }
        public Puzzle Puzzle { get; private set; }

        public Room(string name, Puzzle puzzle)
        {
            Name = name;
            Puzzle = puzzle;
        }
    }
}
