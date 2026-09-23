using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [Header("Tracked Stats")]
    private float totalDistanceTraveled;
    private int totalJumps;
    private int timesCompleted;

    private Vector3 lastPosition;
    private bool isTracking = false;
    private Transform playerTransform;
    private TextMeshPro ui3DText;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeOnStartup()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("StatsManager (Automated)");
            Instance = obj.AddComponent<StatsManager>();
            DontDestroyOnLoad(obj);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadStats();
    }

    private void Start()
    {
        FindPlayerAndUI();
    }

    private void Update()
    {
        TrackDistance();
    }

    private void OnLevelWasLoaded(int level)
    {
        FindPlayerAndUI();
    }

    private void FindPlayerAndUI()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            lastPosition = playerTransform.position;
            isTracking = true;
        }
        else
        {
            isTracking = false;
            playerTransform = null;
        }

        GameObject uiObj = GameObject.Find("Stats3DText");
        if (uiObj != null)
        {
            ui3DText = uiObj.GetComponent<TextMeshPro>();
        }
        else
        {
            ui3DText = null;
        }

        UpdateUI();
    }

    private void TrackDistance()
    {
        if (!isTracking || playerTransform == null)
        {
            return;
        }

        Vector3 currentPosition = playerTransform.position;
        float distanceThisFrame = Vector2.Distance(new Vector2(currentPosition.x, currentPosition.z), new Vector2(lastPosition.x, lastPosition.z));

        totalDistanceTraveled += distanceThisFrame;
        lastPosition = currentPosition;

        UpdateUI();
    }

    public void AddJump()
    {
        totalJumps++;
        UpdateUI();
    }

    public void AddCompletion()
    {
        timesCompleted++;
        UpdateUI();
        SaveStats();
    }

    private void UpdateUI()
    {
        if (ui3DText != null)
        {
            ui3DText.text = $"Distance: {totalDistanceTraveled:F1}m\n" +
            $"Jumps: {totalJumps}\n" +
            $"Completions: {timesCompleted}";
        }
    }

    public void SaveStats()
    {
        PlayerPrefs.SetFloat("TotalDistance", totalDistanceTraveled);
        PlayerPrefs.SetInt("TotalJumps", totalJumps);
        PlayerPrefs.SetInt("TimesCompleted", timesCompleted);
        PlayerPrefs.Save();
    }

    private void LoadStats()
    {
        totalDistanceTraveled = PlayerPrefs.GetFloat("TotalDistance", 0f);
        totalJumps = PlayerPrefs.GetInt("TotalJumps", 0);
        timesCompleted = PlayerPrefs.GetInt("TimesCompleted", 0);
    }

    private void OnApplicationQuit()
    {
        SaveStats();
    }

    private void OnDisable()
    {
        SaveStats();
    }
}
