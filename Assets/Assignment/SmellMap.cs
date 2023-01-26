using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellMap : MonoBehaviour
{
    private float[,] Smells = new float[32, 32];

    private const float BASE_SMELL = 25f;
    private const float DECAY_RATE = 5f;
    void Update()
    {
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if (Smells[i, j] > 0)
                {
                    Smells[i, j] -= Time.deltaTime * DECAY_RATE;
                }
            }
        }
    }

    public void SetSmell(Vector2 Tile)
    {
        Smells[(int)Tile.x, (int)Tile.y] = BASE_SMELL;
    }

    public float GetSmell(Vector2 Tile)
    {
        return Smells[(int)Tile.x, (int)Tile.y];
    }

    public void ResetSmellMap()
    {
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                Smells[i, j] = 0;
            }
        }
    }
}
