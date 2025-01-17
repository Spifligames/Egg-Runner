using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneLevel1 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()) ReloadScene();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
