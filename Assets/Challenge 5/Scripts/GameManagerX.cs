using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GameOverText;
    public GameObject TitleScreen;
    public Button RestartButton; 
    public List<GameObject> TargetPrefabs;

    [Header("Game State")]
    [SerializeField] private int score;
    public bool IsGameActive;
    [SerializeField] private float spawnRate = 4f;

    [Header("Positioning")]
    [SerializeField] private static int maxSquareIndex = 4;
    [SerializeField] private static float spaceBetweenSquares = 2.5f;
    [SerializeField] private static float minValueX = -3.75f; //  x value of the center of the left-most square
    [SerializeField] private static float minValueY = -3.75f; //  y value of the center of the bottom-most square
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        IsGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        UpdateScore(score);
        TitleScreen.SetActive(false);
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (IsGameActive)
        {
            yield return new WaitForSeconds(spawnRate);

            if (IsGameActive)
            {
                int index = Random.Range(0, TargetPrefabs.Count);
                Instantiate(TargetPrefabs[index], RandomSpawnPosition(), TargetPrefabs[index].transform.rotation);
            }
            
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    public static Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    public static int RandomSquareIndex()
    {
        return Random.Range(0, maxSquareIndex);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        ScoreText.text = $"Score: {score}";
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        GameOverText.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
        IsGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
