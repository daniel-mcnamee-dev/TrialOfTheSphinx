using System.Collections.Generic;

namespace TrialOfTheSphinx.Core
{
    // Represents the player and their collected keys
    public class Player
    {
        public string Name { get; private set; }

        // Simple inventory storing key names as strings
        public List<string> Inventory { get; private set; }

        public Player(string name)
        {
            Name = name;
            Inventory = new List<string>();
        }

        // Add a key if the player doesn't already have it
        public void AddKey(string key)
        {
            if (!Inventory.Contains(key))
                Inventory.Add(key);
        }
    }
}


