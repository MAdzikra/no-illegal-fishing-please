using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; // Panel UI yang berisi teks + tombol tutup
    public TMP_Text dialogText;    // Komponen TMP_Text untuk isi dialog

    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }
}
