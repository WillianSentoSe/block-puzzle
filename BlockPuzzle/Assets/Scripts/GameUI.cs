using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI main;

    private GameOverUI gameOverUI;
    private TextMeshProUGUI scoreTextMesh;

    public GameOverUI GameOverUI { get { return gameOverUI; } }

    public void Awake()
    {
        if (main == null) main = this;
        else if (main != this) Destroy(gameObject);
    }

    public void Start()
    {
        gameOverUI = transform.GetComponentInChildren<GameOverUI>();
        scoreTextMesh = transform.Find("Canvas/Score").GetComponent<TextMeshProUGUI>();
    }

    public void SetScore(int score)
    {
        if (scoreTextMesh)
        {
            scoreTextMesh.text = Utils.SetZerosColor(score.ToString("00000000"), "#fff2");
        }
    }
}
