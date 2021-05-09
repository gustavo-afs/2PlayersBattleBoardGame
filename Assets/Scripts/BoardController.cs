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

    

    //Player1
    GameObject player1;
    int player1Health;
    int player1Attack;
    int player1Dices;

    //Player2
    GameObject player2;
    int player2Health;
    int player2Attack = 2;
    int player2Dices = 1;

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
        PlayerAllocation();
        tileMap = boardCreator.CreateBoard(tileMap, player1.transform, player2.transform);
        //Debug.Log(PrintMap(tileMap));
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
                    Transform actualPosition = ActivePlayerGmo().transform;
                    Transform intentPosition = raycastHit.transform;
                    CollectItem((int) intentPosition.position.z, (int) intentPosition.position.x);
                    BoardSetValue((int) raycastHit.transform.position.z, (int) intentPosition.position.x, tileMap[(int) actualPosition.position.z, (int) actualPosition.position.x]);
                    BoardSetValue((int) actualPosition.position.z, (int) actualPosition.position.x, 0);
                    ActivePlayerGmo().transform.position = raycastHit.transform.position;
                    activePlayerMoves--;
                    CheckPlayerTurn();

                    Ray collectableRay = new Ray(ActivePlayerGmo().transform.position, Vector3.up);
                    RaycastHit collectableHit;
                    if (Physics.Raycast(collectableRay, out collectableHit))
                    {
                        
                        if (collectableHit.transform.CompareTag("Button"))
                        {
                            Destroy(collectableHit.transform.parent.gameObject);
                        }
                    }
                }
            }
        }
    }
    void PossibleMoviments(int posX, int posY)
    {
        Debug.Log("Available Moves: " + activePlayerMoves);
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

    void CollectItem(int x, int y)
    {
        int ItemId = tileMap[x, y];
        if (ItemId == 3)
        {
            activePlayerMoves++;
        }
        else
        {
            if (turn)
            {
                switch (ItemId)
                {
                    case 4:
                        player2Attack++;
                        break;
                    case 5:
                        player2Health++;
                        break;
                    case 6:
                        player2Dices++;
                        break;
                }
            } else
            {
                switch (ItemId)
                {
                    case 4:
                        player1Attack++;
                        break;
                    case 5:
                        player1Health++;
                        break;
                    case 6:
                        player1Dices++;
                        break;
                }
            }
        }
    }

    void BoardSetValue(int x, int y, int value)
    {
        tileMap[x, y] = value;
    }

    string PrintMap(int[,] map)
    {

        string mapPrint = "\n";
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for(int y = 0; y < map.GetLength(1); y++ )
            {
                mapPrint += ("[" + map[x,y] + "]");
            }
            mapPrint += ("\n");
        }
        return mapPrint;
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

    GameObject ActivePlayerGmo()
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
        Transform playerPosition = ActivePlayerGmo().transform;
        PossibleMoviments((int) playerPosition.position.z, (int)playerPosition.position.x);
        if(turn)
        
        Debug.Log("Player2: \n Attack Points: " + player2Attack + "\n Health Points: " + player2Health + "\n Dices: " + player2Dices);

        else
            Debug.Log("Player1: \n Attack Points: " + player1Attack + "\n Health Points: " + player1Health + "\n Dices: " + player1Dices);
    }


}
