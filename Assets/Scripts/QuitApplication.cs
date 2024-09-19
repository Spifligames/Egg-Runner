using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitApplication : MonoBehaviour {

    void OnTriggerEnter(Collider other)
	{
        UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
	}

}
