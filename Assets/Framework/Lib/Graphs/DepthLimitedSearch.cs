using System;
using System.Collections.Generic;

namespace Graphs
{
	public static class DepthLimitedSearch
	{

		public static bool Search<T>(Node<T> startNode,
									 int maxDepth, Func<Node<T>, bool> goalTest,
									 out List<Node<T>> path)
		{
			int currentDepth = 0;
			Node<T> node = startNode;
			path = new List<Node<T>>();
			path.Add(startNode);

			Stack<Node<T>> frontier = new Stack<Node<T>>();
			frontier.Push(node);

			List<Node<T>> explored = new List<Node<T>>();

			while (frontier.Count != 0) {
				node = frontier.Pop();
				if(goalTest(node)) {
						return true;
				}

				if(currentDepth > maxDepth) {
					currentDepth--;
					continue;
				}
				explored.Add(node);
				
				currentDepth++;
				foreach(Node<T> child in node.Neighbors) {
					if (!frontier.Contains(child) && !explored.Contains(child)) {
						path.Add(child);
						frontier.Push(child);
						path.Remove(child);
					}
				}
				currentDepth--;
			}
			return false;
		}
	}
}
