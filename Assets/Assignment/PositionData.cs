using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    
}
public class PositionData
{
    public Vector2 position;
    public PickupType pickUp;

    public string FYI =
        "A node is a basic unit of a data structure, such as a linked list or tree data structure. Nodes contain data and also may link to other nodes. Links between nodes are often implemented by pointers.";

    public PositionData(Vector2 position, PickupType pickUp)
    {
        this.position = position;
        this.pickUp = pickUp;
    }
}
