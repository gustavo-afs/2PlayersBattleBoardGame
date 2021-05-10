using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //Player1 Stats
    GameObject player1;
    int player1Health = 2;
    int player1Attack = 3;
    int player1Dices = 0;

    //Player2 Stats
    GameObject player2;
    int player2Health = 2;
    int player2Attack = 3;
    int player2Dices = 0;

    Transform activePlayerPosition;

    //Moviments left
    int activePlayerMoves = 3;

    //Possible Moviment HighLight
    public GameObject highLightMovementPfb;
    List<GameObject> possibleList = new List<GameObject>(); 

    //false = Player 1 Turn | true = Player 2 Turn
    bool turn = false;

    //Count of collectables
    int collectablesMax, collectablesQnty;
    
    //Battle Variables
    bool battleRequested = false;
    public bool isBattleCompleted = true;
    public bool winner;
    public BattleSystem battleSystem;

    //UI Variables
    public Text p1Hp;
    public Text p2Hp;
    public Text p1Atk;
    public Text p2Atk;
    public Text p1Dice;
    public Text p2Dice;
    public Text movesLeft;
    public Text victoryText;

    void UpdateUIStats()
    {
        p1Hp.text   = player1Health.ToString();
        p2Hp.text   = player2Health.ToString();
        p1Dice.text = player1Dices.ToString();
        p2Dice.text = player2Dices.ToString();
        p1Atk.text  = player1Attack.ToString();
        p2Atk.text  = player2Attack.ToString();
        movesLeft.text = activePlayerMoves.ToString();
    }

    void Start()
    {
        collectablesQnty = (tileSizeX * tileSizeY) - 2;
        collectablesMax = collectablesQnty;
        boardCreator = gameObject.GetComponent<BoardCreator>();
        tileMap = new int[tileSizeX, tileSizeY];
        PlayerAllocation();
        tileMap = boardCreator.CreateBoard(tileMap, player1.transform, player2.transform);
        Debug.Log(PrintMap(tileMap));
        CheckPlayerTurn();
    }

    private void Update()
    {
        if (!battleRequested & isBattleCompleted)
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
                        CollectItem((int)intentPosition.position.z, (int)intentPosition.position.x);
                        BoardSetValue((int)raycastHit.transform.position.z, (int)intentPosition.position.x, tileMap[(int)actualPosition.position.z, (int)actualPosition.position.x]);
                        BoardSetValue((int)actualPosition.position.z, (int)actualPosition.position.x, 0);
                        ActivePlayerGmo().transform.position = raycastHit.transform.position;
                        Ray collectableRay = new Ray(ActivePlayerGmo().transform.position, Vector3.up);
                        RaycastHit collectableHit;
                        if (Physics.Raycast(collectableRay, out collectableHit))
                        {

                            if (collectableHit.transform.CompareTag("Button"))
                            {
                                Destroy(collectableHit.transform.parent.gameObject);
                                collectablesQnty--;
                            }
                        }
                        CheckCollectables();
                        activePlayerMoves--;
                        UpdateUIStats();
                        if (!CheckBattle((int)activePlayerPosition.position.z, (int)activePlayerPosition.position.x))
                        {
                            CheckPlayerTurn();
                        }
                        
                    }
                }
            }
        } else if(battleRequested & isBattleCompleted)
        {
            if (winner)
            {
                player1Health = Mathf.Max(0, player1Health - player2Attack);
                player2Attack = 3;
            }
            else
            {
                player2Health = Mathf.Max(0, player2Health - player1Attack);
                player1Attack = 3;
            }

            UpdateUIStats();

            if (player2Health == 0)
            {
                victoryText.text = "Player1 Wins!";
                return;
            }

            if (player1Health == 0)
            {
                victoryText.text = "Player2 Wins!";
                return;
            }

            battleRequested = false;
            ChangeVisibility(true);
            CheckPlayerTurn();
        }
    }

    void RequestBattle()
    {
        bool p1 = false, p2 = false;
        if(player1Dices > 0)
        {
            p1 = true;
            player1Dices--;
        }
        if (player2Dices > 0)
        {
            p2 = true;
            player2Dices--;
        }

        ChangeVisibility(false);
        battleRequested = true;
        ClearPossibleMovesEffects();
        StartCoroutine(battleSystem.StartBattle(p1, p2, turn));
    }

    void ChangeVisibility(bool value)
    {
        isBattleCompleted = value;
        player1.SetActive(value);
        player2.SetActive(value);
        transform.GetChild(0).gameObject.SetActive(value);
    }

    bool CheckBattle(int posX, int posY)
    {
        //Front Position Verification
        if (posX + 1 < tileSizeX)
        {
            
            if ((tileMap[posX + 1, posY] == 1 || tileMap[posX + 1, posY] == 2))
            {
                RequestBattle();
                return true;
            }
        }

        if (posX - 1 >= 0)
        {
            if ((tileMap[posX - 1, posY] == 1 || tileMap[posX - 1, posY] == 2))
            {
                RequestBattle();
                return true;
            }
        }
        if (posY + 1 < tileSizeY)
        {
            if (tileMap[posX, posY + 1] == 1 || tileMap[posX, posY + 1] == 2)
            {
                RequestBattle();
                return true;
            }
        }
        if (posY - 1 >= 0)
        {
            if ((tileMap[posX, posY - 1] == 1 || tileMap[posX, posY - 1] == 2))
            {
                RequestBattle();
                return true;
            }
        }
        return false;
    }

    void PossibleMoviments(int posX, int posY)
    {
        ClearPossibleMovesEffects();

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
        activePlayerPosition = ActivePlayerGmo().transform;
        PossibleMoviments((int)activePlayerPosition.position.z, (int)activePlayerPosition.position.x);
        UpdateUIStats();
    }

    void CheckCollectables()
    {
        
        if(collectablesQnty < collectablesMax*0.1)
        {
            tileMap = boardCreator.CollectablesAllocation(tileMap);
            collectablesQnty = collectablesMax;
        }
    }

    void ClearPossibleMovesEffects()
    {
        foreach (GameObject gameObject in possibleList)
        {
            Destroy(gameObject);
        }

        possibleList.Clear();
    }


}