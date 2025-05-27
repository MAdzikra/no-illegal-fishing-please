using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IkanSpawner : MonoBehaviour
{
    public GameObject[] semuaJenisIkan;     // Semua prefab ikan
    public Transform[] spawnPoints;         // Lokasi tetap

    void Start()
    {
        SpawnIkanRandomDiSpawnPointTetap();
    }

    void SpawnIkanRandomDiSpawnPointTetap()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            int randomIndex = Random.Range(0, semuaJenisIkan.Length);
            GameObject ikanDipilih = semuaJenisIkan[randomIndex];

            GameObject ikanInstance = Instantiate(ikanDipilih, spawnPoint.position, spawnPoint.rotation);
            //ikanInstance.transform.localScale = Vector3.one * 0.1f;

            Camera[] cameras = ikanInstance.GetComponentsInChildren<Camera>(true);
            foreach (Camera cam in cameras)
            {
                cam.enabled = false;
            }

            Transform ikanTransform = ikanInstance.transform;
            if (ikanInstance.name.Contains("sardine") || ikanInstance.name.Contains("avoli 1"))
            {
                ikanTransform.Rotate(new Vector3(90f, 0f, 0f)); // koreksi agar tidak berdiri
            }

            // Tambahkan Rigidbody jika belum ada
            if (ikanInstance.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = ikanInstance.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 1f;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            // Tambahkan Collider jika belum ada
            if (ikanInstance.GetComponent<Collider>() == null)
            {
                ikanInstance.AddComponent<BoxCollider>(); // Bisa diganti SphereCollider/MeshCollider sesuai bentuk ikan
            }

            // Tambahkan XRGrabInteractable jika belum ada
            if (ikanInstance.GetComponent<XRGrabInteractable>() == null)
            {
                ikanInstance.AddComponent<XRGrabInteractable>();
            }
        }
    }
}
