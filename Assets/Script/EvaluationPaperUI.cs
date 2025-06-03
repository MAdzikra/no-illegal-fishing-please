using UnityEngine;
using UnityEngine.UI;

public class EvaluationPaperUI : MonoBehaviour
{
    public PermitEvaluation evaluationLogic;
    public StageManager stageManager;
    public Button submitButton;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitEvaluation);
    }

    void OnSubmitEvaluation()
    {
        bool passed = evaluationLogic.EvaluateFisherman();

        if (passed)
        {
            stageManager.GoToNextStage();
        }
        else
        {
            stageManager.RestartFromBeginning();
        }
    }
}