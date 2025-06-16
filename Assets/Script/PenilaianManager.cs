using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PenilaianManager : MonoBehaviour
{
    public Toggle toggleIkanIllegal;
    public Toggle togglePeralatanIllegal;
    public Button submitButton;
    public TMP_Text stageText;

    public IkanSpawner ikanSpawner;
    public PeralatanSpawner peralatanSpawner;
    public FishermanInteraction nelayan;

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
        bool toggleIkan = toggleIkanIllegal.isOn;
        bool togglePeralatan = togglePeralatanIllegal.isOn;

        bool adaIkanIllegal = ikanSpawner.adaIkanIllegal;
        bool adaPeralatanIllegal = peralatanSpawner.adaPeralatanIllegal;

        bool jawabanIkanBenar = (adaIkanIllegal && toggleIkan) || (!adaIkanIllegal && !toggleIkan);
        bool jawabanPeralatanBenar = (adaPeralatanIllegal && togglePeralatan) || (!adaPeralatanIllegal && !togglePeralatan);

        if (jawabanIkanBenar && jawabanPeralatanBenar)
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
                Debug.Log("❌ Attempt ke: " + GameManager.Instance.attempt);
            }

            ResetStage();
        }
    }

    void ResetStage()
    {
        toggleIkanIllegal.isOn = false;
        togglePeralatanIllegal.isOn = false;

        ikanSpawner.RespawnUlang();
        peralatanSpawner.ResetPeralatan();
        nelayan.ResetInteraction();

        UpdateStageText();
    }

    void UpdateStageText()
    {
        if (stageText != null)
            stageText.text = "Stage " + currentStage;
    }
}
