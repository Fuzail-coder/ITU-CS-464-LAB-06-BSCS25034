using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lecture 06's GameManager, with lives, a respawn and a way to lose.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    int score;

    void Awake() => Instance = this;
    void Start() => UpdateUI();

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        if (score >= targetScore) Win();
    }

    public void UpdateUI()
    {
        scoreText.text = $"Score: {score} / {targetScore}";
        livesText.text = $"Lives: {lives}   Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
    }

    public void PlayerDied()
    {
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

   
    void Lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()   // the win and lose panels' button
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    [SerializeField] TMP_Text winTitle;   // the "Congratulations! You Win" text in WinPanel

    void Win()
    {
        bool lastLevel = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
        if (lastLevel && winTitle != null)
            winTitle.text = "You finished the game!";

        winPanel.SetActive(true);
        Time.timeScale = 0f;
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