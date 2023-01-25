using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphs;
using System.Web;

public class MazeGraph<Data> : MonoBehaviour where Data : MazeGraph<Data>.PositionData, new()
{
	protected Node<PositionData> graph;

	protected GameMode gameMode;
	protected Maze maze;

	protected int nodeWeight = 1;

	[Header("Drawing")]
	[SerializeField] bool Draw = true;
	[SerializeField] Vector2 DrawingOffset = new Vector2(0, 0);
	[SerializeField] Color DrawingColor = Color.green;

	void Awake()
	{
		maze = GameObject.Find("Maze").GetComponent<Maze>();
	}

	void GenerateMsPacManGraph()
	{
		var AlreadyVisited = new List<Node<PositionData>>();
		var NotYetVisited = new List<Node<PositionData>>();
		Node<PositionData> currentNode;
		
		graph = new Node<PositionData>(new PositionData(maze.msPacManSpawn, maze.GetLocationPickUpType(maze.msPacManSpawn)));
		NotYetVisited.Add(graph);

		while (NotYetVisited.Count != 0)
		{
			currentNode = NotYetVisited[0];
			List<Direction> NeighborDirections = maze.GetPossibleDirectionsOfTile(currentNode.data.position);
			foreach (var Direction in NeighborDirections)
			{
				var position = currentNode.data.position + Direction.ToVector2();
				var child = new Node<PositionData>(new PositionData(position, maze.GetLocationPickUpType(position)));
				currentNode.SetEdge(child, 1);

				if (!ListContainsNodeWithLocation(AlreadyVisited, position))
				{
					NotYetVisited.Add(child);
				}
			}
			NotYetVisited.Remove(currentNode);
			AlreadyVisited.Add(currentNode);
		}


	}
	
	void GenerateGhostGraph()
	{
		var AlreadyVisited = new List<Node<PositionData>>();
		var NotYetVisited = new List<Node<PositionData>>();
		Node<PositionData> currentNode;
		
		graph = new Node<PositionData>(new PositionData(maze.msPacManSpawn, maze.GetLocationPickUpType(maze.msPacManSpawn)));
		NotYetVisited.Add(graph);

		while (NotYetVisited.Count != 0)
		{
			currentNode = NotYetVisited[0];
			List<Direction> NeighborDirections = maze.GetPossibleDirectionsOfTile(currentNode.data.position);
			List <Tuple<Node<PositionData>, int>> IntersectionNeighborNodes = new List <Tuple<Node<PositionData>, int>>();

			foreach (var Direction in NeighborDirections)
			{
				nodeWeight = 1;
				var position = Direction.ToVector2() + currentNode.data.position;
				IntersectionNeighborNodes.Add(new Tuple<Node<PositionData>, int>(FindNextIntersection(currentNode, new Node<PositionData>(new PositionData(position, maze.GetLocationPickUpType(position)))), nodeWeight));
			}

			foreach (var tuple in IntersectionNeighborNodes)
			{
				currentNode.SetEdge(tuple.Item1, tuple.Item2);

				if (!AlreadyVisited.Contains(tuple.Item1))
				{
					NotYetVisited.Add(tuple.Item1);
				}
			}
			NotYetVisited.Remove(currentNode);
			AlreadyVisited.Add(currentNode);
		}


	}

	private Node<PositionData> FindNextIntersection(Node<PositionData> parent, Node<PositionData>  child)
	{
		nodeWeight++;
		var Directions = maze.GetPossibleDirectionsOfTile(child.data.position);
		
		if (Directions.Count <= 2)
		{
			foreach (var direction in Directions)
			{
				var position = direction.ToVector2() + child.data.position;
				if (position != parent.data.position)
				{
					return FindNextIntersection(child,
						new Node<PositionData>(new PositionData(position, maze.GetLocationPickUpType(position))));
				}
			}
		}
		
		return child;
	}

	private bool ListContainsNodeWithLocation(List<Node<PositionData>> list, Vector2 location)
	{
		foreach (var node in list)
		{
			if (node.data.position == location)
			{
				return true;
			}
		}

		return false;
	}

	void Update()
	{
		if (Draw)
			DrawGraph();
	}

	void DrawGraph()
	{
		if (graph == null)
			return;

		var ToDraw = new List<Node<PositionData>>();
		var Drawing = new List<Node<PositionData>>();
		var Drawn = new List<Node<PositionData>>();

		ToDraw.Add(graph);

		while (ToDraw.Count > 0)
		{
			Drawing.AddRange(ToDraw);
			ToDraw.Clear();

			foreach (var c in Drawing)
			{
                Debug.DrawLine(c.data.position + DrawingOffset, c.data.position - DrawingOffset, DrawingColor);

                foreach (var cc in c.Neighbors)
				{
					Debug.DrawLine(c.data.position + DrawingOffset, cc.data.position + DrawingOffset, DrawingColor);
					ToDraw.Add(cc);
				}
				Drawn.Add(c);
			}

			Drawing.Clear();
			ToDraw.RemoveAll((c) => { return Drawn.Contains(c); });
		}
	}

	public void DrawPath(List<Node<Data>> path)
	{
		DrawPath(path, DrawingColor);
	}

	public void DrawPath(List<Node<Data>> path, Color color)
	{
		for (int i = 1; i < path.Count; i++)
		{
			Debug.DrawLine(path[i - 1].data.position - DrawingOffset,
						   path[i].data.position - DrawingOffset,
						   color);
		}
	}

	public class PositionData
	{
		public Vector2 position;
		public PickupType pickUp;

		public string FYI =
			"A node is a basic unit of a data structure, such as a linked list or tree data structure. Nodes contain data and also may link to other nodes. Links between nodes are often implemented by pointers.";

		public PositionData(Vector2 position, PickupType pickUp)
		{
			this.position = position;
			this.pickUp = pickUp;
		}
	}

}
