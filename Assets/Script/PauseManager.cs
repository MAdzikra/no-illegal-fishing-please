using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public TutorialManager tutorialManager; // Assign di Inspector


    // Assign ini dari Inspector
    public DynamicMoveProvider moveProvider;
    public Transform cameraTransform; // Drag MainCamera (VR camera) ke sini

    public float distanceFromCamera = 5.0f;
    public Vector3 canvasOffset = Vector3.zero;

    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P)) // Tombol Start
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Vector3 forward = cameraTransform.forward;
        Vector3 targetPos = cameraTransform.position + forward * distanceFromCamera + canvasOffset;
        pauseCanvas.transform.position = targetPos;

        // Menghadap ke kamera
        pauseCanvas.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        pauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (moveProvider != null) moveProvider.enabled = !isPaused;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        TogglePause();
        //moveProvider.enabled = true;
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        //moveProvider.enabled = true;
        SceneManager.LoadScene("StoryScene");
    }
    public void BacktoMenu()
    {
        Time.timeScale = 1;
        //moveProvider.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenTutorial()
    {
        pauseCanvas.SetActive(false); // Sembunyikan menu pause
        tutorialManager.OpenTutorial(); // Tampilkan tutorial panel
    }

}
