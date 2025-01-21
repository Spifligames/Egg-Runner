using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    public string sceneName;
    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneName);
    }

}
