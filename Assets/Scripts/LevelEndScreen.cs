using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScreen : MonoBehaviour
{
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    [SerializeField] private Timer timer;
    public GameObject[] buttons;

    private void Start()
    {
        foreach (var button in buttons)
        {
            button.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.Instance?.RecordLevelComplete();
            ShowButtons();
        }
    }

    private void ShowButtons()
    {
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.enabled = false;
        timer.Finish();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

