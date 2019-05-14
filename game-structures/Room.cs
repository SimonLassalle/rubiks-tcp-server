using System;
using System.Collections.Generic;

namespace game_structures
{
    public class Room
    {
        public enum Privacy
        {
            PUBLIC,
            PRIVATE
        }
        public string name;
        public int maxPlayers;
        public Privacy privacy;
        public Match match;
        private List<Player> players = new List<Player>();

        public Room(string name, int maxPlayers, Privacy privacy)
        {
            this.name = name;
            this.maxPlayers = maxPlayers;
            this.privacy = privacy;
        }

        ~Room()
        {
            foreach (Player p in players)
            {
                DataProccessor.Send(p.id, "room;;deleted");
            }
        }

        #region Player related
        public bool HasPlayer(Player player)
        {
            return players.Contains(player);
        }

        public void AddPlayer(Player player)
        {
            if (players.Count >= maxPlayers)
            {
                throw new Exception(string.Format("Room {} is full", this.name));
            }
            if (HasPlayer(player))
            {
                throw new Exception(string.Format("Player {} already in room {}", player.name, this.name));
            }
            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            if (!HasPlayer(player))
            {
                throw new Exception(string.Format("Player {} is not in room {}", player.name, this.name));
            }
            players.Remove(player);
        }
        #endregion

        #region Match related
        public void GenerateNewMatch<T>(int x, int y, Func<int, int, T> Creator) where T : Match
        {
            this.match = Creator(x, y);
        }


        #endregion
    }
}