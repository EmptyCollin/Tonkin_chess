using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public enum GameState {Welcome,InGame,EndGame};
    public GameState gameState;
    public bool isMultiPlayer;

    public GameObject selectedChess;
    public enum Players { Player1, Player2 };
    public Players currentPlayer;


    /*---------------------------------------------------------*/
    private GameObject board;
    private GameObject initializer;
    private GameObject notification;
    private GameObject ingame;
    /*---------------------------------------------------------*/
    // Start is called before the first frame update
    void Start()
    {
        selectedChess = null;
        currentPlayer = Players.Player1;
        board = GameObject.Find("Board");
        initializer = GameObject.Find("Initializer");
        notification = GameObject.Find("Notification");
        ingame = GameObject.Find("InGame");

        initializer.GetComponent<Initializer>().InitializeWelcomeScreen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart(bool multi) {
        isMultiPlayer = multi;
        gameState = GameState.InGame;
        currentPlayer = Players.Player1;
        initializer.GetComponent<Initializer>().InitializeInGameScreen();
        initializer.GetComponent<Initializer>().InitializeChess();
        initializer.GetComponent<Initializer>().InitializeBoard();
        notification.GetComponent<Notification>().Prompt("- - - -Place a chess- - - -", currentPlayer);
        
        //GameObject.Find("InGame").GetComponent<InGameInput>().EnableBackKey();
    }

    public void GameEnd()
    {
        gameState = GameState.EndGame;

        ingame.GetComponent<InGameInput>().EnableBackKey();

        notification.GetComponent<Notification>().ShowWinner(currentPlayer);

        initializer.GetComponent<Initializer>().RemoveUnboardedChess();
    }

    public void BackToWelcome() {
        initializer.GetComponent<Initializer>().InitializeWelcomeScreen();
        initializer.GetComponent<Initializer>().RemoveAllChess();
    }

    public void ChangeTurn() {

        if(board.GetComponent<Board>().CheckWinner(currentPlayer))
        {
            GameEnd();
            return;
        }
        currentPlayer = currentPlayer == Players.Player1 ? Players.Player2 : Players.Player1;

        if (!isMultiPlayer && currentPlayer == Players.Player2)
        {
            board.GetComponent<Board>().AIOperation(currentPlayer);
            return;
        }

        if (board.GetComponent<Board>().AllChesesOnBoard())
            notification.GetComponent<Notification>().Prompt("- - - -Move a chess- - - -", currentPlayer);
        else notification.GetComponent<Notification>().Prompt("- - - -Place a chess- - - -", currentPlayer);
    }
}
