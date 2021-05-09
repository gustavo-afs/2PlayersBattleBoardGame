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

    //BoardCreator Component
    public BoardCreator boardCreator;

    //Player1 Position
    public int player1X, player1Y;
    public GameObject player1Pfb;

    //Player2 Position
    public int player2X, player2Y;
    public GameObject player2Pfb;

    //Collectables List
    public GameObject[] collectables;

    //Player1
    GameObject player1;
    int player1Health;
    int player1Attack;
    int player1Dices;

    //Player2
    GameObject player2;
    int player2Health;
    int player2Attack;
    int player2Dices;

    int activePlayerMoves = 3;

    //Possible Moviment HighLight
    public GameObject highLightMovementPfb;
    List<GameObject> possibleList = new List<GameObject>(); 

    //false = Player 1 Turn | true = Player 2 Turn
    bool turn = false;



    void Start()
    {
        boardCreator = gameObject.GetComponent<BoardCreator>();
        tileMap = new int[tileSizeX, tileSizeY];
        boardCreator.CreateBoard(tileSizeX, tileSizeY);
        PlayerAllocation();

        //Check
        CollectablesAllocation();
        Debug.Log(PrintMap());
        CheckPlayerTurn();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.CompareTag("PossiblePosition"))
                {
                    Transform actualPosition = ActivePlayer().transform;
                    Transform intentPosition = raycastHit.transform;
  
                    BoardSetValue((int) raycastHit.transform.position.z, (int) intentPosition.position.x, tileMap[(int) actualPosition.position.z, (int) actualPosition.position.x]);
                    BoardSetValue((int) actualPosition.position.z, (int) actualPosition.position.x, 0);
                    ActivePlayer().transform.position = raycastHit.transform.position;
                    Debug.Log(PrintMap());
                    activePlayerMoves--;
                    CheckPlayerTurn();
                }
            }
        }
    }

    void PossibleMoviments(int posX, int posY)
    {
        foreach(GameObject gameObject in possibleList)
        {
            Destroy(gameObject);
        }

        possibleList.Clear();

        //Debug.Log("Actual Position tilemap: tileMap [" + posX + "][" + posY + "]: " + tileMap[posX, posY]);

        //Front Position Verification
        posX = posX + 1;
        
        if (posX< tileSizeX)
        {
            //Debug.Log("Front Verify tilemap: tileMap [" + posX + "][" + posY + "]: " + tileMap[posX, posY]);
            if ((tileMap[posX, posY] != 1 & tileMap[posX, posY] != 2))
            {
                possibleList.Add(Instantiate(highLightMovementPfb, new Vector3(posY, 0f, posX), Quaternion.identity));
            }            
        }

        //Back Position Verification
        posX = posX - 2;
        
        if (posX >= 0)
        {
            //Debug.Log("Back Verify tilemap: tileMap [" + posX + "][" + posY + "]: " + tileMap[posX, posY]);
            if ((tileMap[posX, posY] != 1 & tileMap[posX, posY] != 2))
            {
                possibleList.Add(Instantiate(highLightMovementPfb, new Vector3(posY, 0f, posX), Quaternion.identity));
            } 
        }

        //Right Position Verification
        posX = posX + 1;
        posY = posY + 1;
        
        if (posY < tileSizeY )
        {
            //Debug.Log("Right Verify tilemap: tileMap [" + posX + "][" + posY + "]: " + tileMap[posX, posY]);
            if (tileMap[posX, posY] != 1 & tileMap[posX, posY] != 2)
            {
                possibleList.Add(Instantiate(highLightMovementPfb, new Vector3(posY, 0f, posX), Quaternion.identity));
            }
        }
        //Left Position Verification
        posY = posY - 2;
        
        if (posY>= 0)
        {
            //Debug.Log("Left Verify tilemap: tileMap [" + posX + "][" + posY + "]: " + tileMap[posX, posY]);
            if ((tileMap[posX, posY] != 1 & tileMap[posX, posY] != 2))
            {
                possibleList.Add(Instantiate(highLightMovementPfb, new Vector3(posY, 0f, posX), Quaternion.identity));
            }   
        }
    }

    void BoardSetValue(int x, int y, int value)
    {
        tileMap[x, y] = value;
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

    void PlayerAllocation()
    {
        //Player1 Allocation
        player1 = Instantiate(player1Pfb, new Vector3(player1Y, 0f, player1X), Quaternion.identity);
        BoardSetValue(player1X, player1Y, 1);

        //Player2 Allocation
        player2 = Instantiate(player2Pfb, new Vector3(player2Y, 0f, player2X), Quaternion.Euler(0, 180f, 0));
        BoardSetValue(player2X, player2Y, 2);
    }

    void CollectablesAllocation()
    {
        for (int x = 0; x < tileSizeX; x++)
        {
            for (int y = 0; y < tileSizeY; y++)
            {
                if(tileMap[x,y] == 0)
                {
                    int randomCollectable = Random.Range(0, collectables.Length);
                    GameObject createdCollectable = Instantiate(collectables[randomCollectable], new Vector3(y, 0f, x), Quaternion.identity);
                    BoardSetValue(x,y,randomCollectable+3);
                }
            }
        }
    }

    GameObject ActivePlayer()
    {
        if (turn)
        {
            return player2;
        }
        else
        {
            return player1;
        }
    }


    void CheckPlayerTurn()
    {

        if(activePlayerMoves == 0)
        {
            turn = !turn;
            activePlayerMoves = 3;
            
        }
        Transform playerPosition = ActivePlayer().transform;
        PossibleMoviments((int) playerPosition.position.z, (int)playerPosition.position.x);
    }
}
