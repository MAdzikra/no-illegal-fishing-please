

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Diperlukan untuk XR Interaction Toolkit
using System.Collections; // Diperlukan untuk Coroutine

// Script ini menangani interaksi kotak otomatis (buka/tutup) berdasarkan kedekatan pemain,
// serta memicu spawn ikan melalui interaksi XR setelah kotak terbuka.
public class BoxInteractionHandler : MonoBehaviour
{
    [Header("Pengaturan Komponen Kotak")]
    [Tooltip("Seret GameObject yang mewakili PENUTUP kotak (misalnya, 'BoxLid') ke sini. Ini akan dianimasikan untuk membuka/menutup.")]
    public GameObject boxLid;

    [Tooltip("Rotasi LOKAL penutup kotak saat TERBUKA (misalnya, -90 pada X untuk membuka ke bawah).")]
    public Vector3 openRotation = new Vector3(-90f, 0f, 0f);

    [Tooltip("Rotasi LOKAL penutup kotak saat TERTUTUP (biasanya 0,0,0).")]
    public Vector3 closedRotation = new Vector3(0f, 0f, 0f);

    [Tooltip("Kecepatan animasi pembukaan/penutupan penutup kotak.")]
    public float animationSpeed = 2f;

    [Header("Deteksi Pemain")]
    [Tooltip("Seret SphereCollider yang mendeteksi kedekatan pemain (harus 'Is Trigger') ke sini. Kotak akan terbuka saat pemain masuk area ini.")]
    public SphereCollider playerDetectionTrigger;

    [Tooltip("Tag GameObject pemain Anda (biasanya 'Player' atau 'XR Origin'). Pastikan XR Origin atau Main Camera memiliki tag ini.")]
    public string playerTag = "Player";

    [Header("Fungsionalitas Spawn Ikan")]
    [Tooltip("Seret GameObject yang memiliki script IkanSpawner.cs ke sini. Ini harus sudah ada di scene Anda.")]
    public IkanSpawner ikanSpawner;

    [Tooltip("Seret GameObject di DALAM kotak (yang memiliki komponen XRSimpleInteractable) ke sini. Pemain akan berinteraksi dengan ini untuk spawn ikan.")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable fishSpawnInteractable;

    [Tooltip("Durasi jeda (cooldown) setelah setiap spawn ikan (dalam detik).")]
    public float spawnCooldown = 1.0f;

    [Header("Umpan Balik Haptik")]
    [Tooltip("Amplitudo (kekuatan) getaran haptik (0-1).")]
    [Range(0, 1)]
    public float hapticAmplitude = 0.5f;

    [Tooltip("Durasi getaran haptik (dalam detik).")]
    public float hapticDuration = 0.1f;

    // --- Variabel Internal ---
    private bool isBoxOpen = false;
    private bool playerIsNearby = false; // Saat ini tidak digunakan secara langsung untuk logika open/close, tapi bisa untuk kondisi lain.
    private bool canSpawnFish = true;
    private Coroutine currentAnimationCoroutine;

    // Dipanggil saat script dimuat, bahkan sebelum Start.
    void Awake()
    {
        // Otomatis menambahkan Collider ke fishSpawnInteractable jika belum ada, dan menjadikannya trigger.
        if (fishSpawnInteractable != null && fishSpawnInteractable.gameObject.GetComponent<Collider>() == null)
        {
            Debug.LogWarning("Fish Spawn Interactable (" + fishSpawnInteractable.name + ") membutuhkan komponen Collider. Menambahkan BoxCollider dan mengaturnya sebagai trigger.", fishSpawnInteractable);
            fishSpawnInteractable.gameObject.AddComponent<BoxCollider>().isTrigger = true;
        }
        else if (fishSpawnInteractable != null && fishSpawnInteractable.gameObject.GetComponent<Collider>() != null)
        {
            // Jika sudah ada Collider, pastikan itu trigger
            fishSpawnInteractable.gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }

    void Start()
    {
        // --- Pemeriksaan Referensi Penting (Untuk Debugging Awal) ---
        if (boxLid == null)
        {
            Debug.LogError("ERROR: 'Box Lid' GameObject tidak ditugaskan di BoxInteractionHandler. Script akan dinonaktifkan.", this);
            enabled = false;
            return;
        }
        if (playerDetectionTrigger == null)
        {
            Debug.LogError("ERROR: 'Player Detection Trigger' (SphereCollider) tidak ditugaskan di BoxInteractionHandler. Script akan dinonaktifkan.", this);
            enabled = false;
            return;
        }
        if (ikanSpawner == null)
        {
            Debug.LogError("ERROR: Referensi 'Ikan Spawner' tidak ditugaskan di BoxInteractionHandler. Script akan dinonaktifkan.", this);
            enabled = false;
            return;
        }
        if (fishSpawnInteractable == null)
        {
            Debug.LogError("ERROR: 'Fish Spawn Interactable' tidak ditugaskan di BoxInteractionHandler. Script akan dinonaktifkan.", this);
            enabled = false;
            return;
        }

        // Pastikan SphereCollider untuk deteksi pemain adalah trigger
        playerDetectionTrigger.isTrigger = true;

        // Mendaftarkan fungsi OnSpawnFishActivated ke event 'activated' dari XRSimpleInteractable.
        // Ini akan terpanggil saat pemain "mengaktifkan" interactable (misalnya, menekan trigger kontroler).
        fishSpawnInteractable.activated.AddListener(OnSpawnFishActivated);

        // Pastikan kotak tertutup dan interactable ikan tidak aktif di awal permainan.
        boxLid.transform.localRotation = Quaternion.Euler(closedRotation);
        fishSpawnInteractable.gameObject.SetActive(false);
    }

    // Dipanggil saat GameObject lain (dengan Collider) memasuki area pemicu (trigger) ini.
    void OnTriggerEnter(Collider other)
    {
        // Deteksi jika objek yang masuk adalah pemain (berdasarkan tag).
        if (other.CompareTag(playerTag))
        {
            playerIsNearby = true;
            Debug.Log("Pemain mendekat. Membuka kotak...", this);
            OpenBox(); // Panggil fungsi untuk membuka kotak
        }
    }

    // Dipanggil saat GameObject lain (dengan Collider) keluar dari area pemicu (trigger) ini.
    void OnTriggerExit(Collider other)
    {
        // Deteksi jika objek yang keluar adalah pemain (berdasarkan tag).
        if (other.CompareTag(playerTag))
        {
            playerIsNearby = false;
            Debug.Log("Pemain menjauh. Menutup kotak...", this);
            CloseBox(); // Panggil fungsi untuk menutup kotak
        }
    }

    // Fungsi untuk membuka penutup kotak
    private void OpenBox()
    {
        if (isBoxOpen) return; // Jika kotak sudah dalam kondisi terbuka, abaikan.
        isBoxOpen = true;
        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine); // Hentikan animasi sebelumnya jika ada
        currentAnimationCoroutine = StartCoroutine(AnimateLid(openRotation)); // Mulai animasi membuka
        
        // Aktifkan GameObject XRSimpleInteractable untuk spawn ikan
        fishSpawnInteractable.gameObject.SetActive(true);
    }

    // Fungsi untuk menutup penutup kotak
    private void CloseBox()
    {
        if (!isBoxOpen) return; // Jika kotak sudah dalam kondisi tertutup, abaikan.
        isBoxOpen = false;
        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine); // Hentikan animasi sebelumnya jika ada
        currentAnimationCoroutine = StartCoroutine(AnimateLid(closedRotation)); // Mulai animasi menutup
        
        // Nonaktifkan GameObject XRSimpleInteractable untuk spawn ikan
        fishSpawnInteractable.gameObject.SetActive(false);
    }

    // Coroutine (fungsi yang berjalan seiring waktu) untuk menganimasikan rotasi penutup kotak.
    IEnumerator AnimateLid(Vector3 targetEulerRotation)
    {
        Quaternion startRotation = boxLid.transform.localRotation; // Rotasi awal penutup
        Quaternion endRotation = Quaternion.Euler(targetEulerRotation); // Rotasi target penutup
        float time = 0;

        while (time < 1) // Loop selama animasi belum selesai
        {
            // Interpolasi (peralihan mulus) rotasi antara awal dan akhir
            boxLid.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, time);
            time += Time.deltaTime * animationSpeed; // Maju dalam waktu animasi
            yield return null; // Tunggu satu frame Unity
        }
        boxLid.transform.localRotation = endRotation; // Pastikan rotasi tepat di posisi akhir
    }

    // Handler untuk event 'activated' dari fishSpawnInteractable
    private void OnSpawnFishActivated(ActivateEventArgs arg0)
    {
        // Hanya izinkan spawn jika kotak terbuka dan tidak dalam cooldown
        if (isBoxOpen && canSpawnFish)
        {
            Debug.Log("Memicu spawn ikan!", this);
            // Memanggil fungsi SpawnIkanRandomDiSpawnPointTetap dari script IkanSpawner.
            // Pastikan IkanSpawner.cs sudah diisi dengan prefabs ikan dan spawn points.
            ikanSpawner.SpawnIkanRandomDiSpawnPointTetap();
            
            StartCoroutine(SpawnCooldownCoroutine()); // Mulai cooldown

            // Mengirim umpan balik haptik (getaran) ke kontroler yang berinteraksi.
            // Ini akan membuat kontroler bergetar saat ikan di-spawn.
            if (arg0.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
            {
                controllerInteractor.SendHapticImpulse(hapticAmplitude, hapticDuration);
                Debug.Log("Haptik dikirim ke kontroler: " + controllerInteractor.name, this);
            }
        }
        else if (!isBoxOpen)
        {
            Debug.LogWarning("Kotak tidak terbuka, tidak bisa spawn ikan.", this);
        }
        else if (!canSpawnFish)
        {
            Debug.LogWarning("Cooldown spawn ikan masih aktif. Mohon tunggu.", this);
        }
    }

    // Coroutine untuk mengelola waktu jeda (cooldown) antar spawn ikan.
    IEnumerator SpawnCooldownCoroutine()
    {
        canSpawnFish = false; // Nonaktifkan spawn sementara
        Debug.Log("Cooldown spawn ikan dimulai...", this);
        yield return new WaitForSeconds(spawnCooldown); // Tunggu selama durasi cooldown
        canSpawnFish = true; // Aktifkan kembali spawn
        Debug.Log("Cooldown spawn ikan selesai. Bisa spawn lagi.", this);
    }
}