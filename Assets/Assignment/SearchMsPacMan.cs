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
    
    void Start()
    {
        map = GetComponent<MazeMap>();
        msPacMan = GetComponent<MsPacMan>();
        Graph = new MazeGraph<PositionData>();
        Graph.GenerateMsPacManGraph();
        currentNode = Graph.graph;
        OnDecisionRequired();
    }
    
    void Update()
    {
        if (agent.currentTile == Graph.graph.data.position)
        {
            currentNode = Graph.graph;
            OnDecisionRequired();
        }
            
        if (currentPath.Count > 0)
        {
            Debug.Log(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position));
            agent.Move(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position));
        }
        else
        {
            OnTileReached();
        }
    }

    public bool GoalTest(Node<PositionData> node)
    {
        return node.data.pickUp == PickupType.PILL;
    }

    public override void OnDecisionRequired()
    {
        BreadthFirstSearch.Search(currentNode, GoalTest, out currentPath);
    }

    public override void OnTileReached()
    {
        currentNode.data.pickUp = PickupType.NONE;
        if (currentPath.Count <= 0)
        {
            OnDecisionRequired();
        }
        else
        {
            currentNode = currentPath[0];
            currentPath.Remove(currentPath[0]);
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
