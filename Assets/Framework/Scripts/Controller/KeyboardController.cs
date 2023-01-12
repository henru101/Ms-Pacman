using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    MsPacMan msPacMan;
    
    private void Start() {
        msPacMan = this.GetComponent<MsPacMan>();
    }
    


    void Update()
    {
        if(Input.GetKey(KeyCode.W)) {
            msPacMan.Move(Direction.UP);
        } else 
        if (Input.GetKey(KeyCode.A)) {
            msPacMan.Move(Direction.LEFT);
        } else
        if (Input.GetKey(KeyCode.S)) {
            msPacMan.Move(Direction.DOWN);
        } else
        if (Input.GetKey(KeyCode.D)) {
            msPacMan.Move(Direction.RIGHT);
        } else {

        msPacMan.Move(Direction.NONE);}
    }
}
