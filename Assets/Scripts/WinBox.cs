using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
	public int levelNumber;
    private Timer timerScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			Debug.Log("Player entered WinBox collider");
	
            timerScript = other.GetComponent<Timer>();

			if (timerScript != null)
        	{
            	timerScript.Finish();
        	}
        	else
        	{
           	 Debug.Log("Timer component not found on Player GameObject");
        	}

            if (timerScript != null)
            {
                timerScript.Finish();
            }
        }
    }
}
