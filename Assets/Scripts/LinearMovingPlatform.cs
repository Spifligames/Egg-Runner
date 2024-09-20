using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingPlatform : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] private List<Vector3> movePositions = new List<Vector3>();
    [SerializeField] private float moveTime;
    private int movePosCounter = 0;

    private void OnEnable()
    {
        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {        
        float timeElapsed = 0;
        Vector3 startingPos = gameObject.transform.position;
        
        while (timeElapsed < moveTime)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, movePositions[movePosCounter], timeElapsed / moveTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = movePositions[movePosCounter];
        SetParams();
    }

    private void SetParams()
    {
        movePosCounter++;
        Debug.Log(movePositions.Count + ", " + movePosCounter);
        if (movePosCounter >= movePositions.Count)
        {
            movePosCounter = 0;
        }
        StartCoroutine(MovePlatform());
    }

    private void OnTriggerEnter(Collider other)
    {
        Player.transform.parent = transform;
    }
    private void OnTriggerExit(Collider other)
    {
        Player.transform.parent = null;
    }
}