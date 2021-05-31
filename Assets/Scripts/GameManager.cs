using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Gameシーンの制御 + Spawn, UI, Sound Manager の役割も担ってる
public class GameManager : MonoSingleton<GameManager>
{
    public List<GameObject> targets;
    private float _spawnRate = 1.0f;
    private static int _score;                  // スコア
    public static TextMeshProUGUI scoreText;

    public int lives = 3;                       // ライフ
    public TextMeshProUGUI livesText;

    public static TextMeshProUGUI gameOverText;
    public bool isGameActive;
    public static Button restartButton;
    public GameObject titleScreen;

    public AudioSource BGMAud;                  // BGM
    public Slider BGMVolumeSlider;              // 音量を制御するスライダー
    private static float bgmVolume = 0.7f;      // 音量

    bool isGamePause = false;                   // Pause している最中か
    public GameObject pauseScreen;              // Pause 画面

    protected override void Awake()
    {
        base.Awake();
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        livesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        gameOverText.gameObject.SetActive(false);
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.gameObject.SetActive(false);
        BGMVolumeSlider.value = bgmVolume;
        pauseScreen.SetActive(false);
    }
    void Start()
    {
        UpdateLives(0);
        BGMAud.volume = bgmVolume;
        BGMVolumeSlider.onValueChanged.AddListener(HandleBGMVolumeChange);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            // ポーズ
            PauseGame();
        }
    }

    // 難易度選択ボタンを押された時、実行される処理 (Easy = 1, Medium = 2, Hard = 3)
    public void StartGame(int difficulty)
    {
        isGameActive = true;
        _spawnRate /= difficulty;       // ターゲットを生み出す間隔を難易度選択によって変える
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.SetActive(false);
    }

    // ターゲットを生み出す routine
    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(_spawnRate);
            // ランダムに生成
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    // スコアの更新
    public static void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        // text を スコアに合わせる
        scoreText.text = "Score : " + _score.ToString();
    }

    // Lives の更新
    public void UpdateLives(int livesToAdd)
    {
        lives += livesToAdd;
        livesText.text = "Lives : " + lives.ToString();

        // ゲームオーバーの処理
        if(lives <= 0)
        {
            GameOver(true);
        }
    }

    // ゲームオーバーになった時の処理
    public void GameOver(bool gameover)
    {
        gameOverText.gameObject.SetActive(gameover);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    // ゲームの再開（現在のシーンを再読み込み・初期化処理）
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // 初期化処理
        isGameActive = true;
        _score = 0;
        GameOver(false);
    }

    // BGM の制御
    void HandleBGMVolumeChange(float bgmVolumeToChange)
    {
        bgmVolume = bgmVolumeToChange;
        BGMAud.volume = bgmVolume;
    }

    // PAUSE の制御
    public void PauseGame()
    {
        // Pause状態を切り替え
        isGamePause = isGamePause ? false : true ;
        // Pause == true の時, 停止
        Time.timeScale = isGamePause ? 0 : 1.0f;
        // Pause 画面の表示切り替え
        pauseScreen.SetActive(isGamePause);
    }
}
