using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TongSpawner : MonoBehaviour
{
    public GameObject[] semuaJenisIkan;
    public Transform spawnPoint;
    public string[] namaIkanIllegal;
    public int maxSpawn = 4;

    private int currentSpawn = 0;
    public bool adaIkanIllegal = false;

    private List<GameObject> ikanSpawned = new List<GameObject>();
    public bool sudahSelesai => currentSpawn >= maxSpawn && SemuaIkanSudahDiambil();


    public void OnTongSelected(SelectEnterEventArgs args)
    {
        if (currentSpawn >= maxSpawn)
        {
            Debug.Log("Tong sudah mencapai jumlah maksimal spawn ikan.");
            return;
        }

        SpawnIkan();
        currentSpawn++;
    }

    private void SpawnIkan()
    {
        int randomIndex = Random.Range(0, semuaJenisIkan.Length);
        GameObject ikanDipilih = semuaJenisIkan[randomIndex];

        GameObject ikan = Instantiate(ikanDipilih, spawnPoint.position, Quaternion.identity);
        ikan.transform.parent = transform; // Supaya mudah dibersihkan bareng tong, kalau perlu

        // Tandai ikan ilegal jika perlu
        foreach (string illegalName in namaIkanIllegal)
        {
            if (ikan.name.ToLower().Contains(illegalName.ToLower()))
            {
                adaIkanIllegal = true;
                Debug.Log($"Ikan ilegal ter-spawn: {ikan.name}");
                break;
            }
        }

        // Tambahkan komponen penting jika belum ada
        if (ikan.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = ikan.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = 1f;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        if (ikan.GetComponent<Collider>() == null)
        {
            ikan.AddComponent<BoxCollider>();
        }

        if (ikan.GetComponent<XRGrabInteractable>() == null)
        {
            XRGrabInteractable grab = ikan.AddComponent<XRGrabInteractable>();

            GameObject attach = new GameObject("AttachPoint");
            attach.transform.SetParent(ikan.transform);
            attach.transform.localPosition = Vector3.zero;

            grab.attachTransform = attach.transform;
        }

        ikanSpawned.Add(ikan);
    }

    public void ResetSpawn()
    {
        currentSpawn = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        adaIkanIllegal = false;
        ikanSpawned.Clear();
    }

    private bool SemuaIkanSudahDiambil()
    {
        foreach (GameObject ikan in ikanSpawned)
        {
            if (ikan != null && ikan.transform.parent == transform)
            {
                return false; // masih ada ikan di tong
            }
        }
        return true;
    }

}
