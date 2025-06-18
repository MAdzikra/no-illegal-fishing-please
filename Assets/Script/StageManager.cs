using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StageManager : MonoBehaviour
{
    public Transform spawnPoint;           // Titik spawn awal
    public GameObject playerRig;           // XR Origin / Player XR Rig (biasanya XR Origin atau XRRig)

    public void OnSubmitStage()
    {
        // Kalau ada logika lain untuk pindah stage, taruh di sini

        // Teleport player ke spawn point awal
        TeleportToSpawn();
    }

    private void TeleportToSpawn()
    {
        if (playerRig == null || spawnPoint == null) return;

        // Ambil kamera dari XR Origin
        Camera cam = playerRig.GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("Kamera tidak ditemukan dalam XR Origin.");
            return;
        }

        // Hitung delta rotasi yaw (Y axis)
        float currentYaw = cam.transform.eulerAngles.y;
        float targetYaw = spawnPoint.eulerAngles.y;
        float deltaYaw = targetYaw - currentYaw;

        // Putar XR Origin pada Y axis untuk menyamakan arah hadap
        playerRig.transform.Rotate(0, deltaYaw, 0);

        // Pindahkan ke posisi spawn
        playerRig.transform.position = spawnPoint.position;
    }
}
