using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public GameObject defaultTile;

    public void CreateBoard(int sizeX, int sizeY)
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                GameObject createdTile = Instantiate(defaultTile, new Vector3(y, 0f, x), Quaternion.identity);
                createdTile.name = (x + "," + y);
                createdTile.transform.parent = gameObject.transform;
            }
        }
    }
}
