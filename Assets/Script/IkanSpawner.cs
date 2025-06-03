using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IkanSpawner : MonoBehaviour
{
    public GameObject[] semuaJenisIkan;     // Semua prefab ikan
    public Transform[] spawnPoints;         // Lokasi tetap
    public bool adaIkanIllegal = false;     // Apakah ada ikan ilegal di stage ini

    public string[] namaIkanIllegal = { "sardine", "avoli" };

    void Start()
    {
        SpawnIkanRandomDiSpawnPointTetap();
    }

    public void SpawnIkanRandomDiSpawnPointTetap()
    {
        adaIkanIllegal = false;

        foreach (Transform spawnPoint in spawnPoints)
        {
            int randomIndex = Random.Range(0, semuaJenisIkan.Length);
            GameObject ikanDipilih = semuaJenisIkan[randomIndex];

            GameObject ikanInstance = Instantiate(ikanDipilih, spawnPoint.position, spawnPoint.rotation);
            ikanInstance.transform.parent = transform; // supaya bisa dihapus bareng saat respawn

            // Nonaktifkan kamera dari prefab (jika ada)
            Camera[] cameras = ikanInstance.GetComponentsInChildren<Camera>(true);
            foreach (Camera cam in cameras)
                cam.enabled = false;

            // Koreksi rotasi
            if (ikanInstance.name.ToLower().Contains("sardine") || ikanInstance.name.ToLower().Contains("avoli"))
            {
                ikanInstance.transform.Rotate(new Vector3(90f, 0f, 0f));
            }

            // Cek apakah ikan ini ilegal
            foreach (string illegalName in namaIkanIllegal)
            {
                if (ikanInstance.name.ToLower().Contains(illegalName))
                {
                    adaIkanIllegal = true;
                    break;
                }
            }

            // Tambahkan komponen penting jika belum ada
            if (ikanInstance.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = ikanInstance.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 1f;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            if (ikanInstance.GetComponent<Collider>() == null)
            {
                ikanInstance.AddComponent<BoxCollider>();
            }

            if (ikanInstance.GetComponent<XRGrabInteractable>() == null)
            {
                var grab = ikanInstance.AddComponent<XRGrabInteractable>();

                // Buat attach transform di tengah ikan
                GameObject attachPoint = new GameObject("AttachPoint");
                attachPoint.transform.SetParent(ikanInstance.transform);
                attachPoint.transform.localPosition = Vector3.zero;
                grab.attachTransform = attachPoint.transform;
                grab.farAttachMode = UnityEngine.XR.Interaction.Toolkit.Attachment.InteractableFarAttachMode.Near;

            }
        }
    }

    public void RespawnUlang()
    {
        // Hapus semua ikan dari stage
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        adaIkanIllegal = false;
        SpawnIkanRandomDiSpawnPointTetap();
    }
}
