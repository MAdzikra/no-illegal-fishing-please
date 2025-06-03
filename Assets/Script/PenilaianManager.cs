using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PenilaianManager : MonoBehaviour
{
    public Toggle toggleIkanIllegal;
    public Button submitButton;
    public TMP_Text stageText;
    public IkanSpawner ikanSpawner;

    private int currentStage = 1;
    private const int maxStage = 5;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
        UpdateStageText();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.attempt = 1;
        }
    }

    void OnSubmit()
    {
        bool toggleAktif = toggleIkanIllegal.isOn;
        bool adaIkanIllegal = ikanSpawner.adaIkanIllegal;

        bool jawabanBenar = (adaIkanIllegal && toggleAktif) || (!adaIkanIllegal && !toggleAktif);

        if (jawabanBenar)
        {
            if (currentStage < maxStage)
            {
                currentStage++;
                Debug.Log("✅ Benar! Lanjut ke Stage " + currentStage);
                ResetStage();
            }
            else
            {
                Debug.Log("🎉 Selesai! Semua stage berhasil.");
                float waktu = Object.FindAnyObjectByType<UITimer>()?.GetTimeInSeconds() ?? 0f;

                Debug.Log("⏱️ Total waktu: " + waktu.ToString("F2") + " detik");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetWaktu(waktu);
                }

                SceneManager.LoadScene("Result");
            }
        }
        else
        {
            currentStage = 1;
            Debug.Log("❌ Salah. Kembali ke Stage 1");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TambahAttempt();
                Debug.Log("❌ Salah. Attempt ke: " + GameManager.Instance.attempt);
            }
            else
            {
                Debug.Log("❌ Salah. GameManager tidak ditemukan.");
            }

            ResetStage();
        }
    }

    void ResetStage()
    {
        toggleIkanIllegal.isOn = false;
        ikanSpawner.RespawnUlang();
        UpdateStageText();
    }

    void UpdateStageText()
    {
        if (stageText != null)
            stageText.text = "Stage " + currentStage;
    }
}
