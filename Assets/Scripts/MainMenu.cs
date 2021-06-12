using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the MainMenu of the game
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsScreen;

    private void OnEnable()
    {
        optionsScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Opens the OptionsMenu screen
    /// </summary>
    public void OpenOptions() => optionsScreen.SetActive(true);

    /// <summary>
    /// Close the OptionsMenu screen
    /// </summary>
    public void CloseOptions() => optionsScreen.SetActive(false);

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
