using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int spawnQueueSize = 5;
    public static GameManager main;

    public GameConfig configurations;
    public Player player;
    public GameBoard board;

    private Queue<Piece> spawnQueue;
    private int score;

    #region Getters and Setters

    public GameBoard Board
    {
        get
        {
            if (board == null) board = FindObjectOfType<GameBoard>();
            return board;
        }
    }

    #endregion

    #region Execution

    public void Awake()
    {
        if (main == null) main = this;
        else if (main != null) Destroy(gameObject);
    }

    public void Start()
    {
        StartGame();
    }

    #endregion

    public void StartGame()
    {
        configurations.Restore();

        board.Clear();
        board.SetLineClearMode(GameBoard.LineClearTypes.ColorSticky);

        CreateSpawnList();
        SpawnPiece();
        player.Activate();

        score = 0;
        GameUI.main.SetScore(score);
    }

    public void SpawnPiece()
    {
        Piece nextPiece = spawnQueue.Dequeue();
        spawnQueue.Enqueue(configurations.GetPiece());

        Piece nextPieceInstance = Board.SpawnPiece(nextPiece);
        nextPieceInstance.Color = configurations.GetColor();
        nextPieceInstance.onReachBottom = OnPieceReachBottom;
        player.SetActivePiece(nextPieceInstance);
    }

    private async void OnPieceReachBottom(Piece piece)
    {
        player.RemoveActivePiece();
        Board.DismantlePiece(piece);

        if (isGameOver())
        {
            SetGameOver();
        }
        else
        {
            await Board.CheckRows(OnLineCleared);
            SpawnPiece();
        }
    }

    private void OnLineCleared()
    {
        score += 100;
        GameUI.main.SetScore(score);
    }

    private void CreateSpawnList()
    {
        spawnQueue = new Queue<Piece>();

        for (int i = 0; i < spawnQueueSize; i++)
        {
            spawnQueue.Enqueue(configurations.GetPiece());
        }
    }

    private bool isGameOver()
    {
        return board.MaxHeight > GameBoard.height;
    }

    private void SetGameOver()
    {
        player.Deactivate();
        GameUI.main.GameOverUI.DisplayGameOverScreen();
        GameUI.main.GameOverUI.SetScore(score);
    }
}
