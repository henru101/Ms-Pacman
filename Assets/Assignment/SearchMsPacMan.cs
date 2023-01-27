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
    
    
    private GameMode gameMode;

    void Start()
    {
        map = GetComponent<MazeMap>();
        msPacMan = GetComponent<MsPacMan>();
        gameMode = GameObject.Find("GameMode").GetComponent<SimpleGame>();
        Graph = new MazeGraph<PositionData>();
        Graph.GenerateMsPacManGraph();
        currentNode = Graph.graph;
        DoBreadthFirstSearch();
    }


    protected bool GoalTestBFS(Node<PositionData> node)
    {
        return node.data.pickUp == PickupType.PILL;
    }

    protected bool GoalTestAStar(Node<PositionData> node)
    {
        List<Ghost> ghostList = gameMode.GetAllGhosts();

        foreach (var ghost in ghostList)
        {
            if (ghost.currentTile == node.data.position)
            {
                return true;
            }
        }
        return false;
    }

    protected double HeuristicAStar(Node<PositionData> node)
    {
        return 1f;
    }

    public override void OnDecisionRequired()
    {
        if (currentPath.Count != 0)
        {
            GetComponent<MsPacMan>().Move(DirectionExtensions.ToDirection(currentPath[0].data.position - currentNode.data.position));
        }
    }

    public override void OnTileReached()
    {
        if (currentPath.Count == 0)
        {
            if (!gameMode.IsAnyGhostEdible())
            {
                DoBreadthFirstSearch();
                return;
            }
            else
            {
                double cost;
                AStar.Search(currentNode, HeuristicAStar, GoalTestAStar, Graph.GetAllNodes(), out currentPath, out cost);
                return;
            }
        }
        currentNode = currentPath[0];
        currentNode.data.pickUp = PickupType.NONE;
        
        if (currentPath.Count > 1)
        {
            currentPath.Remove(currentNode);
        }
        else
        {
            if (!gameMode.IsAnyGhostEdible())
            {
                BreadthFirstSearch.Search(currentNode, GoalTestBFS, out currentPath);
            }
            else
            {
                double cost;
                AStar.Search(currentNode, HeuristicAStar, GoalTestAStar, Graph.GetAllNodes(), out currentPath, out cost);
            }
        }
        
    }

    public void DoBreadthFirstSearch()
    {
        BreadthFirstSearch.Search(currentNode, GoalTestBFS, out currentPath);
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
