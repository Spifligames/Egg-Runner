using System;
using UnityEngine;

public class Movement_Rigidbody : MonoBehaviour
{
    public Rigidbody rb;
    public Transform orientation;

    //Movement
    public float moveSpeed;
    public float maxSpeed;

    public float counterMovement = 0.175f;
    public float threshold = 0.01f;

    //Jumping
    public float jumpForce;
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public bool isReadyToJump = true;
    private float jumpCooldown = 0.25f;

    //Input
    float x, y;
    bool isJumping, isCrouching;

    //coruching
    public Vector3 playerScale = new Vector3(1, 1f, 1);
    public Vector3 crouchScale = new Vector3 (1,.5f,1);
    public float slideForce;
    public float slideCounterMovement = 0.2f;

    void Start()
    {
        //playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        MyInput();

    }

    public void MyInput()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        isJumping = Input.GetButton("Jump");
        isCrouching = Input.GetKey(KeyCode.LeftShift);

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopCrouch();
    }

    private void Movement()
    {
        //Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, whatIsGround);

        //Extra Gravity for better Ground Check
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        float multiplierX = 1f, multiplierY = 1f;

        //Air Control
        if (!isGrounded) {
            multiplierX = 0.5f;
            multiplierY = 0.25f;
        }

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //Jump
        if (isReadyToJump && isJumping) Jump();

        //Speed Cap
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        // Movement while sliding
        if (isGrounded && isCrouching) multiplierY = 0f;

        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplierX);
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplierY);
    }

    private void StartCrouch()
    {
        //Makes char smaller
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        //if already moving add slide force
        if (rb.velocity.magnitude > 0.5f)
        {
            if (isGrounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        //Makes char bigger
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            isReadyToJump = false;

            //Add jump forces
            rb.AddForce(Vector3.up * jumpForce * 1.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        //Slow down sliding
        if (isCrouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        //if (moving left or right && no Input || moving left && right Input || moving right && left Input) 
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
}
