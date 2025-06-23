using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StageManager : MonoBehaviour
{
    public Transform spawnPoint;   // Titik spawn yang diinginkan
    public GameObject playerRig;   // XR Origin (biasanya GameObject utama dari XR rig)

    public void OnSubmitStage()
    {
        TeleportToSpawn();
    }

    private void TeleportToSpawn()
    {
        if (playerRig == null || spawnPoint == null) return;

        // Ambil kamera utama dari XR Origin
        Camera cam = playerRig.GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("Kamera tidak ditemukan dalam XR Origin.");
            return;
        }

        // Hitung offset posisi kamera relatif terhadap XR Origin
        Vector3 cameraOffset = cam.transform.position - playerRig.transform.position;

        // Koreksi posisi XR Origin agar kamera berada tepat di posisi spawnPoint
        Vector3 correctedPosition = spawnPoint.position - new Vector3(cameraOffset.x, 0, cameraOffset.z);
        playerRig.transform.position = correctedPosition;

        // Hitung delta rotasi yaw (sumbu Y)
        float currentYaw = cam.transform.eulerAngles.y;
        float targetYaw = spawnPoint.eulerAngles.y;
        float deltaYaw = targetYaw - currentYaw;

        // Putar XR Origin agar kamera menghadap sesuai rotasi spawnPoint
        playerRig.transform.Rotate(0, deltaYaw, 0);
    }
}
