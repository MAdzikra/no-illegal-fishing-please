using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UITimer : MonoBehaviour
{
    public TMP_Text timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        // Format ke menit:detik
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StopAndSaveTimer()
    {
        isRunning = false;
        GameManager.Instance.waktu = elapsedTime;
    }


    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "00:00";
    }

    public float GetTimeInSeconds()
    {
        return elapsedTime;
    }
}

