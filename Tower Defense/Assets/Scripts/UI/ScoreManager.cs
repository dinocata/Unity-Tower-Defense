using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float levelCooldown = 25f;

    public int lives = 20;
    public int level = 1;
    public int money = 100;

    public Text moneyText;
    public Text livesText;
    public Text cooldownText;
    public Text levelText;

    public void LoseLife(int l = 1)
    {
        lives -= l;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        // TODO: Send the player to a game-over screen instead!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void updateScore()
    {
        moneyText.text = "$" + money.ToString();
        livesText.text = "Lives: " + lives.ToString();
    }

    void Start()
    {
        moneyText.text = "Money: $" + money.ToString();
        livesText.text = "Lives: " + lives.ToString();
        levelText.text = "LEVEL " + level.ToString();
        InvokeRepeating("updateScore", 0f, 0.2f);
    }
}
