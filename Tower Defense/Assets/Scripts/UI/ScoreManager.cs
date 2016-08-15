using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public int lives = 20;
    public float money = 100f;

    public Text moneyText;
    public Text livesText;

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

    void Update()
    {
        // FIXME: This doesn't actually need to update the text every frame.
        moneyText.text = "Money: $" + money.ToString();
        livesText.text = "Lives: " + lives.ToString();


    }

}
