using System;
using System.Collections.Generic;

namespace Graphs
{
	public static class BreadthFirstSearch
	{

		public static bool Search<T>(Node<T> startNode,
									 Func<Node<T>, bool> goalTest,
									 out List<Node<T>> path)
        {	
			Node<T> node = startNode;
			path = new List<Node<T>>();

			Dictionary<Node<T>, Node<T>> cameFrom = new Dictionary<Node<T>, Node<T>>();
			Queue<Node<T>> queue = new Queue<Node<T>>();
			queue.Enqueue(node);

			List<Node<T>> explored = new List<Node<T>>();

			while (queue.Count != 0) {

				node = queue.Dequeue();

				explored.Add(node);
				if(goalTest(node))
				{
					path = AStar.ReconstructPath(cameFrom, node);
					return true;
				}
				foreach(Node<T> child in node.Neighbors) {
					if (!queue.Contains(child) && !explored.Contains(child)) {
						cameFrom[child] = node;
						queue.Enqueue(child);
					}
				}
			}
			return false;
	    }
    }
}
