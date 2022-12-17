using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcAngle : MonoBehaviour
{
    public Transform start, finish;
    public bool useQuaternionSlerp;
    public bool lookAtMouse;

    float realAngle;
    private void Update()
    {
        if (useQuaternionSlerp && !lookAtMouse)
        {
            //Calculate the direction
            Vector3 direction = finish.position - start.position;
            Debug.DrawRay(transform.position, direction, Color.red);
            //Calculates the angle using the inverse tangent method, returns radians!
            float angle = Mathf.Atan2(direction.y, direction.x);
            //Radians to degrees, Always -90!!
            realAngle = angle * Mathf.Rad2Deg - 90;
            Debug.Log(realAngle);
            //define the rotation along a specific axis using the angle
            Quaternion angleAxis = Quaternion.AngleAxis(realAngle, Vector3.forward /* z Axis*/);
            //slerp from our current rotation to the new specific rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
        }

        //take our current euler angles, and we just add our new angle to it
        else
            transform.eulerAngles = Vector3.forward * realAngle;

        if (lookAtMouse){
            Vector3 direction = Camera.main.ScreenToWorldPoint
            (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10))
            - transform.position;
            Debug.DrawRay(transform.position, direction, Color.red);
            //Calculates the angle using the inverse tangent method, returns radians!
            float angle = Mathf.Atan2(direction.y, direction.x);
            //Radians to degrees, Always -90!!
            realAngle = angle * Mathf.Rad2Deg - 90;
            Debug.Log(realAngle);
            //define the rotation along a specific axis using the angle
            Quaternion angleAxis = Quaternion.AngleAxis(realAngle, Vector3.forward /* z Axis*/);
            //slerp from our current rotation to the new specific rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
        }
    }
}
