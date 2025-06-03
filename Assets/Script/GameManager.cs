using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float waktu;
    public int attempt = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TambahAttempt()
    {
        attempt++;
    }

    public void SetWaktu(float time)
    {
        waktu = time;
    }
}
