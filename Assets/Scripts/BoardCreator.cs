using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    //Default Tile Game Object
    public GameObject defaultTile;

    GameObject mapParent;

    //Collectables List
    public GameObject[] collectables;

    private void Awake()
    {
        mapParent = new GameObject();
    }

    public int[,] CreateBoard(int[,] map, Transform player1, Transform player2)
    {
        mapParent.name = "MapParent";
        mapParent.transform.parent = gameObject.transform;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                GameObject createdTile = Instantiate(defaultTile, new Vector3(y, 0f, x), Quaternion.identity);
                createdTile.name = (x + "," + y);
                createdTile.transform.parent = mapParent.transform;
            }
        }
        map[(int) player1.position.z, (int) player1.position.x] = 1;
        map[(int) player2.position.z, (int) player2.position.x] = 2;

        return CollectablesAllocation(map);
    }

    public int[,] CollectablesAllocation(int[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == 0)
                {
                    int randomCollectable = Random.Range(0, collectables.Length);
                    GameObject createdCollectable = Instantiate(collectables[randomCollectable], new Vector3(y, 0f, x), Quaternion.identity);
                    createdCollectable.transform.parent = mapParent.transform;
                    map[x, y] = randomCollectable + 3;
                }
            }
        }
        return map;
    }
}
