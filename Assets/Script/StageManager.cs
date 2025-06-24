using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StageManager : MonoBehaviour
{
    public Transform spawnPoint;       // Titik tujuan spawn
    public GameObject playerRig;       // XR Origin (biasanya GameObject utama)

    public void OnSubmitStage()
    {
        TeleportToSpawn();
    }

    private void TeleportToSpawn()
    {
        if (playerRig == null || spawnPoint == null)
        {
            Debug.LogWarning("Player Rig atau Spawn Point belum diset.");
            return;
        }

        // Ambil komponen XROrigin
        XROrigin xrOrigin = playerRig.GetComponent<XROrigin>();
        if (xrOrigin == null)
        {
            Debug.LogWarning("XROrigin tidak ditemukan di Player Rig.");
            return;
        }

        // Teleportasi player (kamera akan otomatis disesuaikan)
        xrOrigin.MoveCameraToWorldLocation(spawnPoint.position);

        // Rotasi manual player rig ke arah spawnPoint
        Vector3 rigEuler = playerRig.transform.eulerAngles;
        rigEuler.y = spawnPoint.eulerAngles.y;
        playerRig.transform.eulerAngles = rigEuler;

        Debug.Log("Teleport berhasil ke posisi spawn.");
    }
}
