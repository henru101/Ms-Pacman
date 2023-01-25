using System.Collections;
using System.Collections.Generic;
using Graphs;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MazeMap))]
[RequireComponent(typeof(Eyes))]
public class SearchMsPacMan<Data> : AgentController<MsPacMan> where Data : MazeGraph<Data>.PositionData, new()
{

    private MazeMap map;
    private MsPacMan msPacMan;
    private MazeGraph<Data> Graph;

    private List<Node<MazeGraph<Data>.PositionData>> searchedNodes;
    
    private List<Node<MazeGraph<Data>.PositionData>> currentPath;

    private Node<MazeGraph<Data>.PositionData> currentNode;
    
    void Start()
    {
        map = GetComponent<MazeMap>();
        msPacMan = GetComponent<MsPacMan>();
        Graph = new MazeGraph<Data>();
        Graph.GenerateMsPacManGraph();
        
    }
    
    void Update()
    {
        agent.Move(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position));
        
        if (agent.currentTile == currentPath[0].data.position)
        {
            OnTileReached();
        }
    }

    public bool GoalTest(Node<MazeGraph<Data>.PositionData> node)
    {
        return node.data.pickUp == PickupType.PILL;
    }

    public override void OnDecisionRequired()
    {
        BreadthFirstSearch.Search(currentNode, GoalTest, out currentPath);
    }

    public override void OnTileReached()
    {
        currentNode = currentPath[0];
        currentPath.Remove(currentPath[0]);

        if (currentPath.Count <= 0)
        {
            OnDecisionRequired();
        }
    }

    private void FindCurrentNode(Node<MazeGraph<Data>.PositionData> node)
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
