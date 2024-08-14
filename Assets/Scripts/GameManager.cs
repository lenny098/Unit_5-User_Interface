using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Dictionary<Difficulty, float> difficultySpawnRate = new Dictionary<Difficulty, float>()
    {
        { Difficulty.Easy, 3 },
        { Difficulty.Medium, 2 },
        { Difficulty.Hard, 1 },
    };

    public List<GameObject> targets;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    public GameObject startUI;
    public GameObject gameOverUI;
    public GameObject pauseUI;

    private bool isGameOver = false;
    private bool isGamePaused = false;
    private float spawnRate = 1;
    private int score = 0;
    private int lives = 3;

    IEnumerator SpawnTarget()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(spawnRate);

            int targetIndex = Random.Range(0, targets.Count);
            Instantiate(targets[targetIndex]);
        }
    }

    void UpdateScore(int change = 0)
    {
        score += change;
        scoreText.text = $"Score: {score}";
    }

    void UpdateLives()
    {
        livesText.text = $"Lives: {lives}";
    }

    public void GoodTargetDestroyed()
    {
        UpdateScore(5);
    }

    public void BadTargetDestroyed()
    {
        UpdateScore(-10);
    }

    public void DeductLives()
    {
        lives--;
        UpdateLives();

        if (lives < 1)
        {
            GameOver();
        }
    }

    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
    }

    void GameOver()
    {
        isGameOver = true;

        gameOverUI.SetActive(true);
    }

    public void StartGame(Difficulty difficulty)
    {
        spawnRate = difficultySpawnRate[difficulty];
        Debug.Log($"spawnRate: {spawnRate}");

        StartCoroutine(SpawnTarget());

        startUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsGamePaused
    {
        get
        {
            return isGamePaused;
        }
    }

    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
        UpdateLives();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
