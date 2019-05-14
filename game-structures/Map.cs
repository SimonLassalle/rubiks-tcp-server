using System;
using System.Collections;
using System.Collections.Generic; 
using System.Linq;

// JSON serialization
using System.Runtime.Serialization;

namespace game_structures
{
	[DataContract]
    public class Map
    {
		[DataMember]
        public int x;
		[DataMember]
        public int y;
		[DataMember]
        public string[] tiles;
		
		public static string[] possibleColors = new string[] { "#367ced", "#ce0a3b", "#14bc0b", "#eadb07", "#f0f2ed" };
		
		
		public Map(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		public Map UpdateMap(string[] newTiles)
		{
			if (newTiles.Length != tiles.Length)
			{
				throw new Exception("Map must be updated with the same number of tiles");
			}
			tiles = newTiles;
			return this;
		}
	}
}








