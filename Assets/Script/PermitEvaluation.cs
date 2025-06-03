using UnityEngine;

public class PermitEvaluation : MonoBehaviour
{
    public bool isPermitValid;
    public bool isGearLegal;

    public GameObject illegalItemsNotice;

    public bool EvaluateFisherman()
    {
        if (!isPermitValid)
        {
            Debug.Log("Dokumen tidak sah.");
            return false;
        }

        if (!isGearLegal)
        {
            illegalItemsNotice.SetActive(true);
            Debug.Log("Peralatan penangkapan ilegal ditemukan.");
            return false;
        }

        Debug.Log("Kapal lolos inspeksi.");
        return true;
    }
}