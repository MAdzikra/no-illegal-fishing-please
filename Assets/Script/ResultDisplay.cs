using UnityEngine;
using TMPro;

public class ResultSceneDisplay : MonoBehaviour
{
    public TMP_Text waktuText;
    public TMP_Text attemptText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            float waktu = GameManager.Instance.waktu;
            int attempt = GameManager.Instance.attempt;

            waktuText.text = $"Waktu: {waktu:F2} detik";
            attemptText.text = $"Percobaan: {attempt}";
        }
    }
}
