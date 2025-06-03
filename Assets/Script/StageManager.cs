using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public PermitEvaluation evaluation;

    public void SubmitEvaluation()
    {
        bool passed = evaluation.EvaluateFisherman();

        if (passed)
        {
            GoToNextStage();
        }
        else
        {
            RestartFromBeginning();
        }
    }

    void GoToNextStage()
    {
        Debug.Log("Lanjut ke kapal berikutnya...");
        // Ganti scene atau atur ulang objek untuk stage selanjutnya
    }

    void RestartFromBeginning()
    {
        Debug.Log("Inspeksi gagal, kembali ke stage 1.");
        // Load ulang dari stage 1
        SceneManager.LoadScene("Stage1");
    }
}