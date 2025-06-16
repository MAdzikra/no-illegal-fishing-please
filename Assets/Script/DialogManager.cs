using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    private void Start()
    {
        dialogPanel.SetActive(false);
    }
    public void ShowDialog(string message, Transform playerHead = null)
    {
        Debug.Log("[DialogManager] Menampilkan dialog: " + message);
        dialogPanel.SetActive(true);
        dialogText.text = message;

        if (playerHead != null)
        {
            // Posisikan di depan kepala pemain (opsional)
            Vector3 targetPos = playerHead.position + playerHead.forward * 1.5f;
            dialogPanel.transform.position = targetPos;
            dialogPanel.transform.LookAt(playerHead);
            dialogPanel.transform.Rotate(0, 180, 0); // agar panel tidak terbalik
        }
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }
}
