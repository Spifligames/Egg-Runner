using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingPlatform : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] private bool loopPos = true;
    [SerializeField] private bool loopRot = true;
    [SerializeField] private List<Vector3> movePositions = new List<Vector3>();
    [SerializeField] private List<Quaternion> moveRotations = new List<Quaternion>();
    [SerializeField] private float moveTime;
    [SerializeField] private float rotateTime;
    private int movePosCounter = 0;
    private int moveRotCounter = 0;
    

    private void OnEnable()
    {
        if (movePositions.Count != 0) StartCoroutine(MovePlatform());
        if (moveRotations.Count != 0) StartCoroutine(RotatePlatform());
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
        SetParams(0);
    }

    private IEnumerator RotatePlatform()
    {
        float timeElapsed = 0;
        Quaternion startingRot = gameObject.transform.rotation;

        while (timeElapsed < rotateTime)
        {
            gameObject.transform.rotation =
                Quaternion.Lerp(startingRot, moveRotations[moveRotCounter], timeElapsed / rotateTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.rotation = moveRotations[moveRotCounter];
        SetParams(1);
    }

    private void SetParams(int mode)
    {
        switch (mode)
        {
            case 0:
                movePosCounter++;
                switch (loopPos)
                {
                    case true:
                        if (movePosCounter >= movePositions.Count) movePosCounter = 0;
                        StartCoroutine(MovePlatform());
                        break;
                    case false:
                        break;
                }
                break;
            case 1:
                moveRotCounter++;
                switch (loopRot)
                {
                    case true:
                        if (moveRotCounter >= moveRotations.Count) moveRotCounter = 0;
                        StartCoroutine(RotatePlatform());
                        break;
                    case false:
                        break;
                }
                break;
        }
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