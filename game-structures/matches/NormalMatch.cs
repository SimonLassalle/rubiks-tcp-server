using System;
using System.Collections;
using System.Collections.Generic; 
using System.Linq;

namespace game_structures
{
    public class NormalMatch : Match
    {
        private string[] possibleColors = new string[] { "blue", "green", "yellow", "red" };
        private string block = "block";

        public NormalMatch(int x, int y)
        {
            map = GenerateMap(x, y);
            objective = GenerateObjective(map, x, y);
        }

        public Map GenerateMap(int x, int y)
        {
            Map map = new Map(x, y);
            Random rnd = new Random();
			string[] tiles = new string[x * y];
			for (int i = 0; i < x * y; i++)
			{
				int colorIndice = rnd.Next(0, possibleColors.Length);
				tiles[i] = possibleColors[colorIndice];
			}
			tiles[rnd.Next(0, x * y)] = "hole";
			map.tiles = tiles;
            return map;
        }

        public Map GenerateObjective(Map map, int x, int y) 
        {
            Map objective = new Map(map.x, map.y);
            string[] tiles = map.tiles;
			tiles = tiles.OrderBy(i => {
											Random rnd = new Random();
											return rnd.NextDouble();
										}).ToArray();
			for (int i = 0; i < x * y; i++)
			{
				if (i % x == 0 || i % x == x - 1 || i < x || i > (x * y) - x)
				{
					tiles[i] = block;
				}
			}
			objective.tiles = tiles;
            return objective;
        }
	
    	public bool EvaluateMap(Map mapToEval)
        { 
            if (mapToEval.tiles.Length != objective.tiles.Length)
            {
                throw new Exception("Unvalid map size");
            }
            for (int i = 0; i < objective.tiles.Length; i++)
            {
                if (objective.tiles[i] != block && objective.tiles[i] != mapToEval.tiles[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}