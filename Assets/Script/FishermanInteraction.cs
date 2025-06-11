using UnityEngine;

public class FishermanInteraction : MonoBehaviour
{
    public DialogManager dialogManager; // Assign via inspector atau dari GameManager
    private bool hasPermit;
    private bool alreadyClicked = false;

    void Start()
    {
        RandomizePermit();
    }

    void OnMouseDown()
    {
        if (alreadyClicked) return;

        alreadyClicked = true;

        if (hasPermit)
            dialogManager.ShowDialog("Nelayan: Saya punya surat izin.");
        else
            dialogManager.ShowDialog("Nelayan: Maaf, saya tidak punya surat izin.");
    }

    public void ResetInteraction()
    {
        alreadyClicked = false;
        RandomizePermit();
    }

    void RandomizePermit()
    {
        hasPermit = Random.value > 0.5f; // true/false secara acak
    }
}
