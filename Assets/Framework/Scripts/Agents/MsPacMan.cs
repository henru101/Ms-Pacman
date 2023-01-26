using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Ms. PacMan can make any move that is possible.
/// </summary>
public class MsPacMan : Agent
{
    private SmellMap smellMap;
    
    protected override bool IsMoveValid(Direction newDirection)
    {
        // Ms. PacMan can do whatever she likes.
        return true;
    }

    protected override void Awake()
    {
        base.Awake();
        
        smellMap = GameObject.Find("Maze").GetComponent<SmellMap>();
    }

    protected override void Update()
    {
        base.Update();
        
        smellMap.SetSmell(currentTile);
    }
}