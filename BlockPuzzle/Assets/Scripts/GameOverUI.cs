using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    private GameObject canvas;
    private TextMeshProUGUI finalScoreMesh;

    public void Start()
    {
        canvas = transform.Find("Canvas").gameObject;
        finalScoreMesh = transform.Find("Canvas/Panel/Score Panel/Score").GetComponent<TextMeshProUGUI>();

        CloseGameOverScreen();
    }

    public void DisplayGameOverScreen()
    {
        if (!canvas) return;

        canvas.SetActive(true);
    }

    public void CloseGameOverScreen()
    {
        if (!canvas) return;

        canvas.SetActive(false);
    }

    public void OnClickPlayAgain()
    {
        GameManager.main.StartGame();
        CloseGameOverScreen();
    }

    public void OnClickBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetScore(int score)
    {
        if (finalScoreMesh)
            finalScoreMesh.text = Utils.SetZerosColor(score.ToString("00000000"), "#1112");
    }
}
