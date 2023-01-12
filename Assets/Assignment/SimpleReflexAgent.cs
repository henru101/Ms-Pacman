using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple reflex agent controller.
/// 
/// A reflex agent reacts to every sensory input in an identical way, independent of previous experience.
/// It is "stateless" and should not have any variables that change due to input or actions.
/// </summary>
[RequireComponent(typeof(MazeMap))]
[RequireComponent(typeof(Eyes))]
public class SimpleReflexAgent : AgentController<MsPacMan>
{

    MazeMap map;
    Eyes eyes;
    MsPacMan msPacMan;

    protected  override void Awake()
    {
        base.Awake();
        map = GetComponent<MazeMap>();
        eyes = GetComponent<Eyes>();
        msPacMan = GetComponent<MsPacMan>();
    }

    private void Update() {
        var directions = map.GetPossibleMoves();
        var directions2 = map.GetPossibleMoves();
        foreach(Direction direction in directions) {
            if(eyes.Look(direction).type == PerceptType.GHOST) {
                directions2.Remove(direction);
            }
        }
        foreach(Direction direction in directions2) {
            if(eyes.Look(direction).type == PerceptType.ITEM) {
                 msPacMan.Move(direction);
                 return;
            }
        }
        msPacMan.Move(directions2[Random.Range(0, directions2.Count)]);
    }

	public override void OnDecisionRequired()
    {
    
       //this.GetComponent<MsPacMan>().Move(Direction.LEFT);
    }

    public override void OnTileReached()
    {
       // throw new System.NotImplementedException();
    }
}
