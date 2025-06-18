using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportToSpawn : MonoBehaviour
{
    public GameObject xrOrigin;      // Drag XR Origin GameObject (yang punya kamera) ke sini
    public Transform spawnPoint;     // Empty GameObject sebagai target spawn

    public void TeleportXR()
    {
        if (xrOrigin == null || spawnPoint == null) return;

        // Ambil kamera dari XR Origin
        Camera cam = xrOrigin.GetComponentInChildren<Camera>();
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
        xrOrigin.transform.Rotate(0, deltaYaw, 0);

        // Pindahkan ke posisi spawn
        xrOrigin.transform.position = spawnPoint.position;
    }
}
