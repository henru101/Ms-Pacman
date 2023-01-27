using System;
using System.Collections.Generic;
using System.Collections;

namespace Graphs
{
	public static class AStar
	{
		public static bool Search<T>(Node<T> startNode, Func<Node<T>, double> heuristic, Func<Node<T>, bool> goalTest, List<Node<T>> AllNodes, out List<Node<T>> path, out double cost)
		{
			path = new List<Node<T>>();

			cost = float.PositiveInfinity;

			PriorityQueue<Node<T>> openSet = new PriorityQueue<Node<T>>();
			
			openSet.Enqueue(startNode, heuristic(startNode));

			Dictionary<Node<T>, Node<T>> cameFrom = new Dictionary<Node<T>, Node<T>>();

			Dictionary<Node<T>, double> gScore = new Dictionary<Node<T>, double>();
			AddAllNodesWithInfinityToDictionary(gScore, AllNodes);
			gScore[startNode] = 0;

			Dictionary<Node<T>, double> fScore = new Dictionary<Node<T>, double>();
			AddAllNodesWithInfinityToDictionary(fScore, AllNodes);
			fScore[startNode] = heuristic(startNode);

			while(openSet.Count() != 0) {
				var current = openSet.Dequeue();
				
				if(goalTest(current))
				{
					path = ReconstructPath(cameFrom, current);
					return true;
				}

				foreach(Node<T> child in current.Neighbors) {
					var tentative_gScore = gScore[current] + current.Edges[child];

					if(tentative_gScore - gScore[child] < 0) {
						cameFrom[child] = current;
						gScore[child] = tentative_gScore;
						fScore[child] = tentative_gScore + heuristic(child);

						if(!openSet.Contains(child)) {
							openSet.Enqueue(child, fScore[child]);
						}
					}
					
				}
			}
			return false;
		}

		private static void AddAllNodesWithInfinityToDictionary<T>(Dictionary<Node<T>, double> dictionary, List<Node<T>> AllNodes)
		{
			foreach (var node in AllNodes)
			{
				dictionary.Add(node, double.PositiveInfinity);
			}
		}
		
		public static  List<Node<T>> ReconstructPath<T>(Dictionary<Node<T>, Node<T>> cameFrom, Node<T> current) {
			List<Node<T>> path = new List<Node<T>>();
			var total_path = current;
			while (cameFrom.ContainsKey(total_path)) {
				path.Add(total_path);
				total_path = cameFrom[total_path];
			}
			path.Reverse();
			return path;
		}
	}
}
