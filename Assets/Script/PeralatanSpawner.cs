using System.Net.Mail;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PeralatanSpawner : MonoBehaviour
{
    public GameObject[] semuaPeralatanIllegal;   // Prefab peralatan (bom, jaring, dll)
    public Transform[] spawnPoints;              // Lokasi spawn tetap

    [HideInInspector] public bool adaPeralatanIllegal = false;
    [HideInInspector] public string namaPeralatanTerpasang = "";

    // Nama prefab ilegal untuk pengecekan
    public string[] namaPeralatanIllegal = { "bom", "trawl" };

    void Start()
    {
        SpawnPeralatanRandom();
    }

    public void SpawnPeralatanRandom()
    {
        adaPeralatanIllegal = false;
        namaPeralatanTerpasang = "";

        // 50% kemungkinan spawn
        bool spawn = Random.value < 0.5f;
        if (!spawn || semuaPeralatanIllegal.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.Log("❌ Peralatan tidak muncul di stage ini");
            return;
        }

        // Pilih prefab dan spawn point acak
        int indexPrefab = Random.Range(0, semuaPeralatanIllegal.Length);
        int indexPoint = Random.Range(0, spawnPoints.Length);

        GameObject prefabDipilih = semuaPeralatanIllegal[indexPrefab];
        Transform spawnPoint = spawnPoints[indexPoint];

        GameObject instance = Instantiate(prefabDipilih, spawnPoint.position, spawnPoint.rotation);
        instance.transform.parent = transform;
        instance.name = prefabDipilih.name;

        // Cek apakah ini peralatan ilegal
        foreach (string nama in namaPeralatanIllegal)
        {
            if (instance.name.ToLower().Contains(nama))
            {
                adaPeralatanIllegal = true;
                namaPeralatanTerpasang = nama;
                Debug.Log("⚠️ Peralatan ilegal muncul: " + nama);
                break;
            }
        }

        // Tambahkan Rigidbody
        if (instance.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = instance.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = 1f;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Tambahkan MeshCollider (convex wajib)
        if (instance.GetComponent<Collider>() == null)
        {
            MeshFilter meshFilter = instance.GetComponentInChildren<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                MeshCollider meshCol = instance.AddComponent<MeshCollider>();
                meshCol.sharedMesh = meshFilter.sharedMesh;
                meshCol.convex = true;
            }
            else
            {
                instance.AddComponent<BoxCollider>(); // fallback
                Debug.LogWarning("⚠️ MeshCollider gagal, fallback ke BoxCollider");
            }
        }

        // Tambahkan XRGrabInteractable
        if (instance.GetComponent<XRGrabInteractable>() == null)
        {
            XRGrabInteractable grab = instance.AddComponent<XRGrabInteractable>();

            GameObject attach = new GameObject("AttachPoint");
            attach.transform.SetParent(instance.transform);
            attach.transform.localPosition = Vector3.zero;
            grab.attachTransform = attach.transform;
            grab.farAttachMode = UnityEngine.XR.Interaction.Toolkit.Attachment.InteractableFarAttachMode.Near;
        }
    }

    public void ResetPeralatan()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        SpawnPeralatanRandom();
    }
}
