using System.Collections;
using System.Collections.Generic;
using Graphs;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class Nose : Sensor
{
    private SmellMap smellMap;
    
    protected void Start()
    {
        smellMap = GameObject.Find("Maze").GetComponent<SmellMap>();
    }
    
    public float Smell()
    {
        return smellMap.GetSmell(agent.currentTile);
    }
}
