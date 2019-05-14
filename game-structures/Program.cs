using System;
using Sockets;

namespace game_structures
{
    class Program
    {
        static void Main(string[] args)
        {
            Room room = new Room("room", 2, Room.Privacy.PUBLIC);
            room.GenerateNewMatch<NormalMatch>(3, 3, (x, y) => new NormalMatch(x, y));
            foreach (string tile in room.match.map.tiles)
            {
                Console.WriteLine(tile);
            }
        }
    }
}
