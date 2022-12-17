using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    public float speed;

    public bool moveRight;
    public bool moveLeft;
    public bool moveForward;
    public bool moveBack;
    public bool moveUp;
    public bool moveDown;
    void Update()
    {
        if (moveRight)
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (moveLeft)
            transform.Translate(Vector3.right * -speed * Time.deltaTime);

        if (moveForward)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (moveBack)
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);

        if (moveUp)
            transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (moveDown)
            transform.Translate(Vector3.up * -speed * Time.deltaTime);
    }
}
