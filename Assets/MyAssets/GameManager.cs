using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lecture 06's GameManager, with lives, a respawn, a way to lose and a level timer.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] int maxLives = 5;
    [SerializeField] int targetScore = 10;
    [SerializeField] int lives = 3;
    [SerializeField] float respawnDelay = 0.6f;
    [SerializeField] float spawnProtection = 1.5f;
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] TMP_Text winTitle;   // the "Congratulations! You Win" text in WinPanel

    [Header("Timer")]
    [SerializeField] float timeLimit = 60f;   // seconds; set 0 for no timer
    [SerializeField] TMP_Text timerText;
    [Header("Heart (spawns when 1 life is left)")]
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Transform[] heartSpawnPoints;
    GameObject heart;
    int score;
    float timeLeft;
    bool finished;

    void Awake() => Instance = this;

    void Start()
    {
        timeLeft = timeLimit;
        UpdateUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (finished || timeLimit <= 0f) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateTimerUI();
            Lose();   // time ran out
            return;
        }
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        if (timeLimit <= 0f) { timerText.text = ""; return; }

        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
        timerText.color = timeLeft <= 10f ? Color.red : Color.white;   // warning in the last 10 seconds
    }

    public void AddScore(int amount)
    {
        if (finished) return;

        score += amount;
        UpdateUI();
        if (score >= targetScore) Win();
    }

    public void UpdateUI()
    {
        scoreText.text = $"Score: {score} / {targetScore}";
        livesText.text = $"Lives: {lives}   Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
    }
    void CheckHeart()
    {
        if (lives != 1 || heart != null) return;   // only at 1 life, and only one heart at a time
        if (heartPrefab == null || heartSpawnPoints.Length == 0) return;

        Transform p = heartSpawnPoints[Random.Range(0, heartSpawnPoints.Length)];
        heart = Instantiate(heartPrefab, p.position, Quaternion.identity);
    }
    public bool AddLife(int amount)
    {
        if (finished || lives >= maxLives) return false;

        lives = Mathf.Min(lives + amount, maxLives);
        UpdateUI();
        return true;
    }
    public void PlayerDied()
    {
        if (finished) return;

        lives--;
        UpdateUI();
        if (lives <= 0) Lose();
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;
        player.Restore(spawnProtection);
    }

    void Win()
    {
        if (finished) return;
        finished = true;

        bool lastLevel = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
        if (lastLevel && winTitle != null)
            winTitle.text = "You finished the game!";

        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        if (finished) return;
        finished = true;

        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()   // the win and lose panels' button
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(0);   // after the final screen, back to Level 1
    }
}