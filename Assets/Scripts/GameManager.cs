using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum Difficulty { Easy, Medium, Hard }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int lives = 3;
    [SerializeField] int goodTargetScore;
    [SerializeField] int badTargetScore;

    [Header("Spawn Forces")]
    [SerializeField] float upwardForceMin;
    [SerializeField] float upwardForceMax;
    [SerializeField] float TorqueForceMin;
    [SerializeField] float TorqueForceMax;

    [Header("Spawn Rates")]
    [SerializeField] float easySpawnRate;
    [SerializeField] float mediumSpawnRate;
    [SerializeField] float hardSpawnRate;

    [Header("Prefabs")]
    [SerializeField] List<GameObject> targets;

    [Header("References")]
    [SerializeField] GameObject spawnBoundsObject;

    [SerializeField] GameObject startUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject pauseUI;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;

    public bool IsGamePaused { get; private set; } = false;

    int score = 0;
    float spawnRate;

    Dictionary<Difficulty, float> difficultySpawnRate;
    Bounds spawnBounds;

    Vector3 RandomTorque()
    {
        float RandomAxisTorque() { return Random.Range(TorqueForceMin, TorqueForceMax); }

        return new Vector3(RandomAxisTorque(), RandomAxisTorque(), RandomAxisTorque());
    }

    void SpawnTarget()
    {
        GameObject target = targets[Random.Range(0, targets.Count)];

        float spawnX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        Vector3 position = new Vector3(spawnX, spawnBounds.center.y);

        Rigidbody rigidbody = Instantiate(target, position, target.transform.rotation).GetComponent<Rigidbody>();

        rigidbody.AddForce(Vector3.up * Random.Range(upwardForceMin, upwardForceMax), ForceMode.Impulse);
        rigidbody.AddTorque(RandomTorque(), ForceMode.Impulse);
    }

    /* Game Flow */

    public void StartGame(Difficulty difficulty)
    {
        spawnRate = difficultySpawnRate[difficulty];
        InvokeRepeating("SpawnTarget", 0, spawnRate);

        startUI.SetActive(false);
    }

    void GameOver()
    {
        CancelInvoke("SpawnTarget");

        GameObject[] inGameTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject inGameTarget in inGameTargets)
        {
            inGameTarget.GetComponent<Target>().DestroyByGameOver();
        }

        gameOverUI.SetActive(true);
    }

    void PauseGame()
    {
        IsGamePaused = true;

        Time.timeScale = 0;

        pauseUI.SetActive(true);
    }

    void ResumeGame()
    {
        IsGamePaused = false;

        Time.timeScale = 1;

        pauseUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* Game UI */

    void UpdateScoreUI(){ scoreText.text = $"Score: {score}"; }

    void UpdateLivesUI(){ livesText.text = $"Lives: {lives}"; }

    /* Game Score */

    public void GoodTargetDestroyed()
    {
        score += goodTargetScore;
        UpdateScoreUI();
    }

    public void BadTargetDestroyed()
    {
        score += badTargetScore;
        UpdateScoreUI();
    }

    public void GoodTargetMissed()
    {
        lives--;
        UpdateLivesUI();

        if (lives < 1) GameOver();
    }

    /* Lifecycle */

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
        difficultySpawnRate = new Dictionary<Difficulty, float>()
        {
            { Difficulty.Easy, easySpawnRate },
            { Difficulty.Medium, mediumSpawnRate },
            { Difficulty.Hard, hardSpawnRate },
        };

        spawnBounds = spawnBoundsObject.GetComponent<Renderer>().bounds;

        UpdateScoreUI();
        UpdateLivesUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
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
