using System;
using System.Collections.Generic;

namespace Graphs
{
	public static class DepthFirstSearch
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

			Stack<Node<T>> frontier = new Stack<Node<T>>();
			frontier.Push(node);

			List<Node<T>> explored = new List<Node<T>>();

			while (frontier.Count != 0) {
				node = frontier.Pop();

				explored.Add(node);

				foreach(Node<T> child in node.Neighbors) {
					if (!frontier.Contains(child) && !explored.Contains(child)) {
						path.Add(child);
						if(goalTest(child)) {
							return true;
						}
						frontier.Push(child);
						path.Remove(child);
					}
				}
			}
			return false;
		}
	}
}
