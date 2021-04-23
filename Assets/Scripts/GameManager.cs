using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float _spawnRate = 1.0f;
    public static TextMeshProUGUI scoreText;
    private static int _score;
    public static TextMeshProUGUI gameOverText;
    public static bool isGameActive = true;
    public static Button restartButton;
    void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        gameOverText.gameObject.SetActive(false);
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.gameObject.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(_spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public static void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        scoreText.text = "Score : " + _score.ToString();
    }

    public static void GameOver(bool gameover)
    {
        gameOverText.gameObject.SetActive(gameover);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameActive = true;
        _score = 0;
    }
}
