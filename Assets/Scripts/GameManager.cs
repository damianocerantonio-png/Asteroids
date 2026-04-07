using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Asteroidi")]
    [SerializeField] private GameObject[] bigAsteroidsPrefab;
    [SerializeField] private GameObject[] mediumAsteroidPrefab;
    [SerializeField] private GameObject[] smallAsteroidPrefab;

    public Dictionary<Asteroid.Type, GameObject[]> asteroidsPrefab = new Dictionary<Asteroid.Type, GameObject[]>();

    [SerializeField] private int initialAsteroidCount = 5;
    [SerializeField] private float spawnRadius = 8f;

    private List<GameObject> activeAsteroids = new List<GameObject>();

    [Header("UI")]
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI livesText;
    private GameObject gameOverPanel;
    private Button restartButton;

    [Header("Game Data")]
    public int score = 0;
    public int lives = 3;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void SetupAsteroids()
    {
        asteroidsPrefab.Clear();

        asteroidsPrefab.Add(Asteroid.Type.Big, bigAsteroidsPrefab);
        asteroidsPrefab.Add(Asteroid.Type.Medium, mediumAsteroidPrefab);
        asteroidsPrefab.Add(Asteroid.Type.Small, smallAsteroidPrefab);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        var allTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var t in allTexts)
        {
            if (t.name == "ScoreText") scoreText = t;
            if (t.name == "LivesText") livesText = t;
        }

        var allObjects = FindObjectsOfType<Transform>(true);
        foreach (var obj in allObjects)
        {
            if (obj.name == "GameOverPanel")
                gameOverPanel = obj.gameObject;

            if (obj.name == "RestartButton")
                restartButton = obj.GetComponent<Button>();
        }

       
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

      
        score = 0;
        lives = 3;
        activeAsteroids.Clear();

        SetupAsteroids();
        SpawnInitialAsteroids();
        UpdateUI();
    }

    void Update()
    {
        if (activeAsteroids.Count <= 0)
        {
            SpawnInitialAsteroids();
        }
    }

    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < initialAsteroidCount; i++)
        {
            SpawnAsteroid(GetRandomSpawnPosition(), Asteroid.Type.Big);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomAngle = Random.Range(0f, 360f);

        return new Vector3(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad) * spawnRadius,
            Mathf.Sin(randomAngle * Mathf.Deg2Rad) * spawnRadius,
            0
        );
    }

    public void SpawnAsteroid(Vector3 position, Asteroid.Type type)
    {
        if (!asteroidsPrefab.ContainsKey(type)) return;

        GameObject asteroid = Instantiate(
            asteroidsPrefab[type][Random.Range(0, asteroidsPrefab[type].Length)],
            position,
            Quaternion.identity
        );

        activeAsteroids.Add(asteroid);
    }

    public void RemoveAsteroid(GameObject asteroid)
    {
        if (activeAsteroids.Contains(asteroid))
            activeAsteroids.Remove(asteroid);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            GameOver();
        }

        UpdateUI();
    }

    void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }
}