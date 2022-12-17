
using UnityEngine;

public class ShootingAi : MonoBehaviour
{
    public CharacterController cc;
    public Transform player;
    public GameObject gun;

    //Movement
    public float moveSpeed, gravityMultiplier;

    //Check for Ground/Obstacles
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    public Vector2 distanceToWalkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attack Player
    public float timeBetweenAttacks;
    bool alreadyShot;

    //State machine
    public bool isPatroling, isChasing, isAttacking;
    public float sightRange, attackRange;
    public bool grounded, playerInSightRange, playerInAttackRange;

    private void Update()
    {
        StateMachine();
        Movement();
    }

    private void Movement()
    {
        //extra gravity
        cc.Move(-transform.up * Time.deltaTime * gravityMultiplier);
    }
    private void StateMachine()
    {
        //Check if Player in sightrange
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        //Check if Player in attackrange
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }
    private void Patroling()
    {
        isPatroling = true;
        isChasing = false;
        isAttacking = false;

        //Calculates DistanceToWalkPoint
        distanceToWalkPoint = new Vector2(Mathf.Abs(walkPoint.x) - Mathf.Abs(transform.position.x), Mathf.Abs(walkPoint.z) - Mathf.Abs(transform.position.z));

        if (!walkPointSet) SearchWalkPoint();

        //Calculate direction and walk to Point
        if (walkPointSet){
            Vector3 direction = walkPoint - transform.position;
            cc.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }
        //Walkpoint reached
        if (distanceToWalkPoint.x < .1f && distanceToWalkPoint.y < .1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint,-transform.up, 2,whatIsGround))
        walkPointSet = true;
    }
    private void ChasePlayer()
    {
        isPatroling = false;
        isChasing = true;
        isAttacking = false;

        Debug.Log("Chasing1");

        //Direction Calculate
        Vector3 direction = player.position - transform.position;
        cc.Move(direction.normalized * moveSpeed * Time.deltaTime);

        Debug.Log("Chasing2");
    }
    private void AttackPlayer()
    {
        isPatroling = false;
        isChasing = false;
        isAttacking = true;

        transform.LookAt(player.transform.position);

        if (!alreadyShot){
            gun.GetComponent<Gun>().Shoot();
            alreadyShot = true;
            Invoke("ResetShot", timeBetweenAttacks);
        }
    }
    private void ResetShot()
    {
        alreadyShot = false;
    }
}
