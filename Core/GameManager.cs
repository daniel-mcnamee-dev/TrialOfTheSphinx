using System.Collections.Generic;
using TrialOfTheSphinx.Core;

namespace TrialOfTheSphinx
{
    // Handles game flow: player, rooms, and win condition
    public class GameManager
    {
        // The current player
        public Player Player { get; private set; }

        // All rooms in the game in order
        private List<Room> Rooms;

        // Index of the current room
        private int currentIndex = 0;

        public GameManager(Player player)
        {
            Player = player;
            Rooms = new List<Room>();
        }

        // Add a new room to the game sequence
        public void AddRoom(Room room) => Rooms.Add(room);

        // Get the room the player is currently in (or null if finished)
        public Room CurrentRoom => currentIndex < Rooms.Count ? Rooms[currentIndex] : null;

        // Move to the next room
        public void NextRoom() => currentIndex++;

        // Win condition: player must exist and have at least 3 keys
        public bool HasWon => Player != null && Player.Inventory.Count >= 3;
    }
}
