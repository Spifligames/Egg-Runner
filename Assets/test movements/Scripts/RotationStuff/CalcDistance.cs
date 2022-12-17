using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcDistance : MonoBehaviour
{
    public Transform start, finish;
    public bool isNormalized;

    private void Update()
    {
        //Direction = Destination - StartPoint
        Vector3 direction = finish.position - start.position;
        Debug.Log("Yes: " + direction.magnitude);
        Debug.Log(Mathf.Sqrt((4f * 4f) + (4f * 4f)));
        Debug.DrawRay(start.position, direction, Color.red);

        if (isNormalized)
            transform.Translate(direction.normalized * Time.deltaTime * 8);
        else
        transform.Translate(direction * Time.deltaTime * 2);
    }
}
