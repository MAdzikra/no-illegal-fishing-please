using UnityEngine;

public class FishermanInteraction : MonoBehaviour
{
    public DialogManager dialogManager; // Drag dari scene
    public Transform playerHead;        // Drag XR camera (biasanya Main Camera)

    private bool hasPermit;
    private bool alreadyClicked = false;

    void Start()
    {
        RandomizePermit();
        Debug.Log("[Nelayan] Surat izin diacak: " + (hasPermit ? "✅ Punya" : "❌ Tidak punya"));
    }

    public void Interact()
    {
        Debug.Log("[Nelayan] Interact dipanggil.");

        if (alreadyClicked)
        {
            Debug.Log("[Nelayan] Sudah diklik sebelumnya, abaikan.");
            return;
        }

        alreadyClicked = true;

        string message;
        if (hasPermit)
        {
            message = "Nelayan: Saya punya surat izin.";
            Debug.Log("[Nelayan] Status: ✅ Punya surat izin.");
        }
        else
        {
            message = "Nelayan: Maaf, saya tidak punya surat izin.";
            Debug.Log("[Nelayan] Status: ❌ Tidak punya surat izin.");
        }

        dialogManager.ShowDialog(message, playerHead);
    }

    public void ResetInteraction()
    {
        alreadyClicked = false;
        RandomizePermit();
        Debug.Log("[Nelayan] Reset: Status klik direset. Surat izin: " + (hasPermit ? "✅ Punya" : "❌ Tidak punya"));
    }

    void RandomizePermit()
    {
        hasPermit = Random.value > 0.5f;
    }

    public bool HasPermit() => hasPermit;
    public bool AlreadyClicked() => alreadyClicked;
}
