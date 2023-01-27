using System.Collections;
using System.Collections.Generic;
using Graphs;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MazeMap))]
[RequireComponent(typeof(Eyes))]
public class SearchMsPacMan : AgentController<MsPacMan>
{

    private MazeMap map;
    private MsPacMan msPacMan;
    private MazeGraph<PositionData> Graph;

    private List<Node<PositionData>> searchedNodes;
    
    private List<Node<PositionData>> currentPath;

    private Node<PositionData> currentNode;

    private Rigidbody2D rigidbody;

    void Start()
    {
        map = GetComponent<MazeMap>();
        msPacMan = GetComponent<MsPacMan>();
        Graph = new MazeGraph<PositionData>();
        Graph.GenerateMsPacManGraph();
        currentNode = Graph.graph;
        BreadthFirstSearch.Search(currentNode, GoalTest, out currentPath);
    }


    public bool GoalTest(Node<PositionData> node)
    {
        return node.data.pickUp == PickupType.PILL;
    }

    public override void OnDecisionRequired()
    {
        this.GetComponent<MsPacMan>().Move(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position));
    }

    public override void OnTileReached()
    {
        if (currentPath.Count > 1)
        {
            currentNode = currentPath[0];
            currentNode.data.pickUp = PickupType.NONE;
            currentPath.Remove(currentNode);
        }
        else
        {
            currentNode = currentPath[0];
            currentNode.data.pickUp = PickupType.NONE;
            BreadthFirstSearch.Search(currentNode, GoalTest, out currentPath);
        }
        
    }

    private void FindCurrentNode(Node<PositionData> node)
    {
        if (agent.currentTile == node.data.position)
        {
            currentNode = node;
            return;
        }
        
        searchedNodes.Add(node);
        foreach (var child in node.Neighbors)
        {
            if (!searchedNodes.Contains(child))
            {
                FindCurrentNode(child);
            }
        }

        Debug.Log("current position not in the node graph! Have fun fixing that!");
    }
}
