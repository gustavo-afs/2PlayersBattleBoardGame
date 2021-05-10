using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public BoardController boardController;

    public GameObject[] player1Dices = new GameObject[4];
    public GameObject[] player2Dices = new GameObject[4];
    bool activePlayer;
    
    int[] player1DiceResults = new int[4];
    int[] player2DiceResults = new int[4];
    
    Vector3 value = new Vector3(302.2f, 130.5f, 354.6f);

    List<Vector3> DiceRotation = new List<Vector3>();

    private void Awake()
    {
        //Dice on Rotation 1
        DiceRotation.Add(new Vector3(274.9f, 131.0f, 48.4f));

        //Dice on Rotation 2
        DiceRotation.Add(new Vector3(1.9f, 90.8f, 173.2f));

        //Dice on Rotation 3
        DiceRotation.Add(new Vector3(2.0f, 179.8f, 280.4f));

        //Dice on Rotation 4
        DiceRotation.Add(new Vector3(7.2f, 179.0f, 98.2f));

        //Dice on Rotation 5
        DiceRotation.Add(new Vector3(356.5f, 359.4f, 354.2f));

        //Dice on Rotation 6
        DiceRotation.Add(new Vector3(80.8f, 303.8f, 213.3f));
    }

    public IEnumerator StartBattle(bool player1xDice, bool player2xDice, bool currentPlayer)
    {
        activePlayer = currentPlayer;
        for (int i = 0; i < 3; i++)
        {
            player1DiceResults[i] = UnityEngine.Random.Range(1, 6);
            player1Dices[i].SetActive(true);
            StartCoroutine(RotateDices(player1Dices[i], player1DiceResults[i]));

            player2DiceResults[i] = UnityEngine.Random.Range(1, 6);
            player2Dices[i].SetActive(true);
            StartCoroutine(RotateDices(player2Dices[i], player2DiceResults[i]));
        }
        //Checking if Player1 has an extra Dice
        if (player1xDice)
        {
            player1Dices[3].SetActive(true);
            player1DiceResults[3] = UnityEngine.Random.Range(1, 6);
            StartCoroutine(RotateDices(player1Dices[3], player1DiceResults[3]));
        }
        else
        {
            player1DiceResults[3] = 0;
        }

        //Checking if Player2 has an extra Dice
        if (player2xDice)
        {
            player2Dices[3].SetActive(true);
            player2DiceResults[3] = UnityEngine.Random.Range(1, 6);
            StartCoroutine(RotateDices(player2Dices[3], player2DiceResults[3]));
        }
        else
        {
            player2DiceResults[3] = 0;
        }

        SortingArrays();
        yield return new WaitForSeconds(4);
        player1Dices[3].SetActive(false);
        player2Dices[3].SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            player1Dices[i].transform.eulerAngles = DiceRotation[player1DiceResults[i] - 1];
            player2Dices[i].transform.eulerAngles = DiceRotation[player2DiceResults[i] - 1];
        }
        yield return new WaitForSeconds(3);
        int pointsCounter = 0;
        for (int i = 0; i < 3; i++)
        {
            if (player1DiceResults[i] > player2DiceResults[i] || (!activePlayer & player1DiceResults[i] == player2DiceResults[i]))
            {
                player2Dices[i].SetActive(false);
                pointsCounter++;
            }
            else
            {
                player1Dices[i].SetActive(false);
                pointsCounter--;
            }
        }

        if (pointsCounter > 0 || (!activePlayer & pointsCounter == 0))
        {
            boardController.winner = false;
        }
        else
        {
            boardController.winner = true;
        }
        yield return new WaitForSeconds(3);
        Debug.Log("Battle Completed");
        boardController.isBattleCompleted = true;
        for (int i = 0; i < 3; i++)
        {
            player1Dices[i].SetActive(false);
            player2Dices[i].SetActive(false);
        }
    }

    IEnumerator RotateDices(GameObject dice, int Result)
    {
        float startTime = Time.time;
        
        while (Time.time < startTime + 3)
        {
            dice.transform.Rotate(2, 4, 2, Space.Self);
            yield return null;
        }
        dice.transform.eulerAngles = DiceRotation[Result-1];
        yield return null;
    }

    void SortingArrays()
    {
        Array.Sort(player1DiceResults);
        Array.Reverse(player1DiceResults);
        Array.Sort(player2DiceResults);
        Array.Reverse(player2DiceResults);
    }
}
