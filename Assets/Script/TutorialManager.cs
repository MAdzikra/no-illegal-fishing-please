using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;         // Panel utama container
    public GameObject[] tutorialSteps;       // Panel1, Panel2, Panel3
    public Button nextButton;
    public Button prevButton;
    public PauseManager pauseManager; // Assign dari Inspector
    public Transform cameraTransform; // Drag MainCamera (VR camera) ke sini
    public float distanceFromCamera = 1.0f;
    public Vector3 canvasOffset = Vector3.zero;

    private int currentStep = 0;

    void Start()
    {
        tutorialPanel.SetActive(false);
        UpdateStep();
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        currentStep = 0;
        PositionInFrontOfCamera();
        UpdateStep();
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        pauseManager.TogglePause();
    }

    public void NextStep()
    {
        if (currentStep < tutorialSteps.Length - 1)
        {
            currentStep++;
            UpdateStep();
        }
        else
        {
            // Di step terakhir, tutup tutorial dan buka pause menu
            CloseTutorial();
            pauseManager.TogglePause();
        }

    }

    public void PrevStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            UpdateStep();
        }
        else
        {
            // Di step terakhir, tutup tutorial dan buka pause menu
            CloseTutorial();
            pauseManager.TogglePause();
        }
    }

    void UpdateStep()
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            tutorialSteps[i].SetActive(i == currentStep);
        }

        prevButton.interactable = currentStep > 0;
        nextButton.interactable = currentStep < tutorialSteps.Length - 1;
    }

    void PositionInFrontOfCamera()
    {
        if (cameraTransform != null && tutorialPanel != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 targetPos = cameraTransform.position + forward * distanceFromCamera + canvasOffset;
            tutorialPanel.transform.position = targetPos;
            tutorialPanel.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
}
