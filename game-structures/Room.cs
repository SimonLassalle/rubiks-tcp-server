using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace game_structures
{
    [DataContract]
    public class Room
    {
        public enum Privacy
        {
            PUBLIC,
            PRIVATE
        }
        [DataMember]
        public string name;
        [DataMember]
        public int maxPlayers;
        [DataMember]
        public Privacy privacy;
        private Match match;
        private List<Player> players = new List<Player>();
        private Player master;

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
                throw new MyException(string.Format("Room {0} is full", this.name));
            }
            if (HasPlayer(player))
            {
                throw new MyException(string.Format("Player {0} already in room {1}", player.name, this.name));
            }
            if (players.Count == 0)
            {
                this.master = player;
            }
            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            if (!HasPlayer(player))
            {
                throw new MyException(string.Format("Player {1} is not in room {1}", player.name, this.name));
            }
            players.Remove(player);
            if (this.master == player && players.Count > 0)
            {
                this.master = players[0];
                DataProccessor.Send(this.master.id, "room;;promoted");
            }
        }
        #endregion

        #region Match related
        public void GenerateNewMatch<T>(int x, int y, Func<int, int, T> Creator) where T : Match
        {
            this.match = Creator(x, y);
        }

        public Match GetMatch()
        {
            return this.match;
        }
        #endregion
    }
}