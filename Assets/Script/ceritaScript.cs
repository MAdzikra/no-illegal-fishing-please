using UnityEngine;

public class StoryCanvasManager : MonoBehaviour
{
    public GameObject[] canvases; // daftar canvas
    private int currentIndex = 0;

    void Start()
    {
        UpdateCanvasDisplay();
    }

    public void NextCanvas()
    {
        if (currentIndex < canvases.Length - 1)
        {
            currentIndex++;
            UpdateCanvasDisplay();
        }
    }

    public void PreviousCanvas() // opsional kalau mau tombol "Back"
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateCanvasDisplay();
        }
    }

    private void UpdateCanvasDisplay()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].SetActive(i == currentIndex);
        }
    }
}
