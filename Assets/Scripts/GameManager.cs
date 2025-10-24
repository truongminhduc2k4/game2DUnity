using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 000000000;
    [SerializeField] private TextMeshProUGUI ScorePoint;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false;
    private bool isGameWin = false;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score = score + points;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        ScorePoint.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);  
    }    
    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
    
    public  void Menu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }    
    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }    
}
