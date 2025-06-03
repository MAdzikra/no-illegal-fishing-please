using UnityEngine;

public class FishermanInteraction : MonoBehaviour
{
    public GameObject dialogPanel;
    public GameObject fishingPermit;
    public GameObject fishingGearCabinet;

    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ShowDialog();
        }
    }

    private void ShowDialog()
    {
        dialogPanel.SetActive(true);
        // Simulasi pemberian surat izin
        fishingPermit.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}