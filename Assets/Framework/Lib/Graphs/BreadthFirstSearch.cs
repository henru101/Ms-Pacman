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
			path.Add(startNode);
			if(goalTest(startNode)) {
				return true;
			}

			Queue<Node<T>> queue = new Queue<Node<T>>();
			queue.Enqueue(node);

			List<Node<T>> explored = new List<Node<T>>();

			while (queue.Count != 0) {

				node = queue.Dequeue();

				explored.Add(node);

				foreach(Node<T> child in node.Neighbors) {
					if (!queue.Contains(child) && !explored.Contains(child)) {
						path.Add(child);
						if(goalTest(child)) {
							return true;
						}
						queue.Enqueue(child);
						path.Remove(child);
					}
				}
			}
			return false;
	    }
    }
}
