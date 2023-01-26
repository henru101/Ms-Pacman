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

    private bool newList = false;
    
    void Start()
    {
        map = GetComponent<MazeMap>();
        msPacMan = GetComponent<MsPacMan>();
        rigidbody = GetComponent<Rigidbody2D>();
        Graph = new MazeGraph<PositionData>();
        Graph.GenerateMsPacManGraph();
        currentNode = Graph.graph;
        NextSearch();
        NextMove();
    }
    
    void Update()
    {
        if (agent.currentTile == Graph.graph.data.position)
        {
            currentNode = Graph.graph;
            NextSearch();
        }

        if (newList)
        {
            NextMove();
            newList = false;
        }
        
        if (newList && currentPath.Count != 0 && currentPath[0].data.position == this.rigidbody.position)
        {
            NextMove();
        }
    }

    public void NextSearch()
    {
        BreadthFirstSearch.Search(currentNode, GoalTest, out currentPath);
    }
    
    
    public void NextMove()
    {
        if (currentPath.Count == 1)
        {
            Debug.Log(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position));
            agent.Move(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position), true);
            currentNode = currentPath[0];
            currentNode.data.pickUp = PickupType.NONE;
            NextSearch();
            newList = true;
        }
        
        if(currentPath.Count > 1)
        {
            currentNode = currentPath[0];
            currentNode.data.pickUp = PickupType.NONE;
            currentPath.Remove(currentPath[0]);
            agent.Move(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position), true);
        }
    }
    
    public bool GoalTest(Node<PositionData> node)
    {
        return node.data.pickUp == PickupType.PILL;
    }

    
    
    public override void OnDecisionRequired()
    {
        //BreadthFirstSearch.Search(currentNode, GoalTest, out currentPath);
    }

    public override void OnTileReached()
    {
        /*
        currentNode.data.pickUp = PickupType.NONE;
        currentPath.Remove(currentPath[0]);
        if (currentPath.Count <= 0)
        {
            OnDecisionRequired();
        }
        else
        {
            currentNode = currentPath[0];
        }*/
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
