using UnityEngine;

public class DDOLManager : MonoBehaviour
{
    public float level1Time;
    public float level2Time;
    public float level3Time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveScore(float time, string sceneName)
    {
        switch (sceneName)
        {
            case "Level1":
                level1Time = time;
                break;
            case "Level2":
                level2Time = time;
                break;
            case "Level3":
                level3Time = time;
                break;

        }
    }
}
