using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTimeText;
    public Color winColor = Color.yellow;
    private float startTime;
    private bool finished = false;
    private float bestTime;

    private string sceneName; // Variable to store the current scene name

    void Start()
    {
        startTime = Time.timeSinceLevelLoad;
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Load the best time for the current scene from PlayerPrefs
        bestTime = PlayerPrefs.GetFloat(sceneName + "BestTime", 0f);
        UpdateBestTimeDisplay();
    }

    void Update()
    {
        if (finished)
            return;

        float currentTime = Time.timeSinceLevelLoad - startTime;

        if (!finished)
        {
            string minutes = ((int)currentTime / 60).ToString();
            string seconds = (currentTime % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;
        }
    }

    public void Finish()
    {
        if (finished)
            return;

        finished = true;
        timerText.color = winColor;

        float currentTime = Time.timeSinceLevelLoad - startTime;

        if (currentTime < bestTime || bestTime == 0f)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat(sceneName + "BestTime", bestTime);
            PlayerPrefs.Save();
            UpdateBestTimeDisplay();
        }
    }

    private void UpdateBestTimeDisplay()
    {
        bestTimeText.text = "Best Time: " + bestTime.ToString("F2");
    }
}
