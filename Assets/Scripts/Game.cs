using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the Game scene
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary>
    /// Custom tag of the Asteroid objects
    /// </summary>
    public static string asteroidTag = "Asteroid";
    /// <summary>
    /// Custom Tag of the Shots objects
    /// </summary>
    public static string shotTag = "Shot";
    /// <summary>
    /// Custom Tag of the asteroid drop objects
    /// </summary>
    public static string asteroidDropTag = "Drop";

    public GameObject gameOverScreen;

    private static Game instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameOverScreen.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Example console debugs that will be written on file
        Debug.Log("Game Start!!");
        Debug.LogWarning("Warning Start!");
        Debug.LogError("Error Start!");
    }

    /// <summary>
    /// Class that spawn all the asteroids when game starts in a random distance range from the player
    /// </summary>
    public static void SpawnAsteroids()
    {
        for (int i=0; i<SpawnPool.instance.spawnedObjects.Count; i++)
        {
            GameObject asteroid = SpawnPool.instance.GetPoolObject(asteroidTag);
            if (asteroid != null)
            {
                Vector3 playerPos = PlayerController.GetPlayerPosition();

                Vector3 randomPos = new Vector3(playerPos.x + 30 * Random.Range(-10f, 10f),
                                                playerPos.y + 30 * Random.Range(-10f, 10f),
                                                playerPos.z + 30 * Random.Range(-10f, 10f));
                asteroid.transform.position = randomPos;
                asteroid.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Class that spawns a single asteroid in a distance range from the player if there is one available in the spawn pool
    /// </summary>
    public static void SpawnAsteroid()
    {
        GameObject asteroid = SpawnPool.instance.GetPoolObject(asteroidTag);
        if (asteroid != null)
        {
            Vector3 playerPos = PlayerController.GetPlayerPosition();

            Vector3 randomPos = new Vector3(playerPos.x + 100 * Random.Range(-2f, 2f),
                                            playerPos.y + 100 * Random.Range(-2f, 2f),
                                            playerPos.z + 100 * Random.Range(-2f, 2f));

            asteroid.transform.position = randomPos;
            asteroid.SetActive(true);
        }
    }

    /// <summary>
    /// Function called when you die and ends the game
    /// </summary>
    public static void GameOver()
    {
        Time.timeScale = 0f;
        instance.gameOverScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Go back to the main menu of the game
    /// </summary>
    public static void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
