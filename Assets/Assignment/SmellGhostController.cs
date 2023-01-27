using System;
using System.Collections;
using System.Collections.Generic;
using Graphs;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
[RequireComponent(typeof(Nose))]
public class SmellGhostController : AgentController<Ghost>
{
    private Nose nose;
    private Ghost ghost;

    public GameMode gameMode;
    
    private bool active;
    private float lastTileSmell;
    private float currentTileSmell;

    protected override void Awake()
    {
        base.Awake();
        nose = GetComponent<Nose>();
        ghost = GetComponent<Ghost>();
    }

    protected void Start()
    {
        gameMode = GameObject.Find("GameMode").GetComponent<SimpleGame>();
    }

    public override void OnDecisionRequired()
    {
        agent.Move(DirectionExtensions.Random());
    }

    public override void OnTileReached()
    {
        lastTileSmell = currentTileSmell;
        currentTileSmell = nose.Smell();
        
        if (gameMode.IsGhostEdible(ghost))
        {
            if (lastTileSmell < currentTileSmell)
            {
                agent.Move(DirectionExtensions.Opposite(agent.currentMove), true);
            }
        }
        else
        {
            if (currentTileSmell < lastTileSmell-1)
            {
                agent.Move(DirectionExtensions.Opposite(agent.currentMove), true);
            }
        }

    }
}
