using UnityEngine;
using UnityEngine.UI;

public class LevelEndScreen : MonoBehaviour
{
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
            ShowButtons();
        }
    }

    private void ShowButtons()
    {
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
    }
}

