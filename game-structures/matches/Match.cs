using System;
using System.Collections;
using System.Collections.Generic;

namespace game_structures
{
	/*
		The Match interface represents a match that can
		host players. It keeps in memory the initial
		map and the objective
	*/
	public abstract class Match
	{
		int id { get; }
		public Map map { get; set; }
		public Map objective { get; set; }
		Map GenerateMap(int x, int y) { return null; }
		Map GenerateObjective(Map map, int x, int y) { return null; }
		bool EvaluateMap(Map map) { return false; }
	}
}