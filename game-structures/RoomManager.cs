using System;
using System.Collections;
using System.Collections.Generic; 

namespace game_structures
{

    public class RoomManager
    {
        private static RoomManager instance;
        private static int roomCount = 0;
        private static Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        public static RoomManager GetInstance()
        {
            if (instance == null)
            {
                new RoomManager();
            }
            return instance;
        }

        private RoomManager()
        {
            instance = this;
        }
        
        public List<Room> GetRooms() 
        {
            List<Room> resultRooms = new List<Room>();
            foreach (Room room in rooms.Values)
            {
                resultRooms.Add(room);
            }
            return resultRooms;
        }

        public bool TryGetRoom(string roomName, out Room room)
        {
            room = null;
            foreach (Room r in rooms.Values)
            {
                if (r.name == roomName)
                {
                    room = r;
                    return true;
                }
            }
            return false;
        }

        public void CreateRoom(string roomName, int maxPlayers, Room.Privacy privacy=Room.Privacy.PUBLIC)
        {
            rooms.Add(roomCount++, new Room(roomName, maxPlayers, privacy));
        }

        public void JoinRoom(string roomName, Player player)
        {
            if (TryGetRoom(roomName, out Room room))
            {
                room.AddPlayer(player);
                return;
            }
            throw new Exception(string.Format("Room {} does not exist", roomName));
        }

        public void LeaveRoom(string roomName, Player player)
        {
            if (TryGetRoom(roomName, out Room room))
            {
                room.RemovePlayer(player);
                return;
            }
            throw new Exception(string.Format("Room {} does not exist", roomName));
        }
    }
}