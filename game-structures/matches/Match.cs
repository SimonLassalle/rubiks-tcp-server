using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace game_structures
{
	/*
		The Match interface represents a match that can
		host players. It keeps in memory the initial
		map and the objective
	*/
	[DataContract]
	[KnownType(typeof(NormalMatch))]
	public abstract class Match
	{
		[DataMember]
		public int id;
		[DataMember]
		public Map map;
		[DataMember]
		public Map objective;
		Map GenerateMap(int x, int y) { return null; }
		Map GenerateObjective(Map map, int x, int y) { return null; }
		bool EvaluateMap(Map map) { return false; }
	}
}