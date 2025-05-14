using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int totalJumps = 0;
    public float totalDistance = 0f;
    public int levelsCompleted = 0;

    private Transform playerTransform;
    private Vector3 lastPosition;

    private TextMeshPro statsWorldText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Load saved data
        totalJumps = PlayerPrefs.GetInt("TotalJumps", 0);
        totalDistance = PlayerPrefs.GetFloat("TotalDistance", 0f);
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);

        SceneManager.sceneLoaded += OnSceneLoaded;

        TryFindStatsText();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryFindStatsText();
    }

    private void TryFindStatsText()
    {
        GameObject statsTextGO = GameObject.Find("Stats3DText");

        if (statsTextGO != null)
        {
            statsWorldText = statsTextGO.GetComponent<TextMeshPro>();
            UpdateStatsText();
        }
        else
        {
            Debug.LogWarning("Stats3DText not found in scene.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceMoved = Vector3.Distance(playerTransform.position, lastPosition);
            totalDistance += distanceMoved;
            lastPosition = playerTransform.position;
        }

        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        if (statsWorldText != null)
        {
            statsWorldText.text = $"Jumps: {totalJumps}\nDistance: {totalDistance:F1}m\nLevels: {levelsCompleted}";
        }
    }

    public void RecordJump()
    {
        totalJumps++;
        UpdateStatsText();
    }

    public void RecordLevelComplete()
    {
        levelsCompleted++;
        SaveStats();
        UpdateStatsText();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("TotalJumps", totalJumps);
        PlayerPrefs.SetFloat("TotalDistance", totalDistance);
        PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
        PlayerPrefs.Save();
    }
}
