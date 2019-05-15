using System;
using System.Collections;
using System.Collections.Generic; 

namespace game_structures
{
	public class PlayerManager
	{
		private static PlayerManager instance;
		
		private static List<Player> players;
		
		// Returns the PlayerManager singleton
		public static PlayerManager GetInstance() 
		{
			if (instance == null)
				instance = new PlayerManager();
			return instance;
		}
		
		// Creates the singleton
		private PlayerManager() 
		{
			players = new List<Player>();
		}
		
		// Gives the list of all players
		public List<Player> GetPlayers()
		{
			return players;
		}
		
		// Gets the player for the given id if any, or null
		public Player GetPlayer(int id)
		{
			foreach (Player p in players)
			{
				if (p.id == id)
					return p;
			}
			throw new MyException(string.Format("No player found with id {0}", id));
		}
		
		// Finds the player which has the given name if any, or null
		public Player GetPlayerByName(string name)
		{
			foreach (Player player in players)
			{
				if (player.name == name)
				{
					return player;
				}
			}
			throw new MyException(string.Format("No player found with name {0}", name));
		}
		
		// Creates a new player and adds it to the players list
		public Player CreatePlayer(int id, string name)
		{
			Player player = new Player(id, name);
			players.Add(player);
			return player;
		}
		
		public void DeletePlayer(int id)
		{
			if (!players.Remove(GetPlayer(id)))
			{
				throw new Exception("Impossible to delete player with id " + id);
			}
		}
	}
	
	/*
		The Player class represents a player. It contains
		his last map state, his name...
	*/
	public class Player
	{
		public int id;
		public string name;
		public Map map;

		public Player(int id, string name)
		{
			this.id = id;
			this.name = name;
		}
	}
	
}