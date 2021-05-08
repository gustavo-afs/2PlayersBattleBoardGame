using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    //Variables related to the Size Map
    public int tileSizeX = 16, 
               tileSizeY = 16;

    //Tile Map Array
    public int[,] tileMap;

    //Default Tile
    public GameObject defaultTile;

    // Start is called before the first frame update
    void Start()
    {
        
        tileMap = new int[tileSizeX, tileSizeY];
        BoardCreator();
        Debug.Log(PrintMap());

    }

    void BoardCreator()
    {
        for (int x = 0; x < tileSizeX; x++)
        {
            for (int y = 0; y < tileSizeY; y++)
            {
                GameObject createdTile = Instantiate(defaultTile, new Vector3(y, 0f, x), Quaternion.identity);
                createdTile.name = ("[" + x + "] " + "[" + y + "]");
            }
            //map += ("\n");
        }
    }
    string PrintMap()
    {

        string map = "\n";
        for (int x = 0; x < tileSizeX; x++)
        {
            for(int y = 0; y <tileSizeY; y++ )
            {
                map += ("[" + tileMap[x,y] + "]");
            }
            map += ("\n");
        }
        return map;
    }
}
