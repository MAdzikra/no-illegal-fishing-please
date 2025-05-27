using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject MenuPanel;
    void Start()
    {
        MenuPanel.SetActive(true);
        Debug.Log("Script Loaded");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Ingame");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void OptionMenu()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void TutorialMenu()
    {
        SceneManager.LoadScene("TutorialFirstScene");
    }

    public void TutorialMenuNext()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "TutorialFirstScene")
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else if (currentScene == "TutorialScene")
        {
            SceneManager.LoadScene("TutorialLastScene");
        }
    }
    
    public void TutorialMenuPrev()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "TutorialScene")
        {
            SceneManager.LoadScene("TutorialFirstScene");
        }
        else if (currentScene == "TutorialLastScene")
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("StoryScene");
    }
}
