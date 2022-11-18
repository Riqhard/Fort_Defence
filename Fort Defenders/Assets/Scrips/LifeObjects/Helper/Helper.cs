using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Helper : LivingEntity
{
    public enum State { Idle, Following, Building, Attacking, Gathering };
    State currentState;

    NavMeshAgent pathfinder;

    Transform player;
    Transform target;
    BuildingTile buildingTile;
    GatheringArea gatheringTile;

    Vector3 idleStartPos;

    float myCollisionRadius;
    float targetRange = 0;

    bool hasTarget = false;

    [Header("Idle")]
    public float maxIdleRange;
    public float minIdleWaitTime;
    public float maxIdleWaitTime;

    float idleWaitTimer;
    Vector3 nextIdleCoord;
    bool waiting = false;
    bool movingToNextWaitPoint = false;

    public override void Start()
    {
        base.Start();

        pathfinder = GetComponent<NavMeshAgent>();
        GoToIdle();
        player = FindObjectOfType<Player>().transform;        
        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
    }


    public void Update()
    {
        if (currentState == State.Idle)
        {
            
            if (movingToNextWaitPoint)
            {
                pathfinder.SetDestination(nextIdleCoord);
                movingToNextWaitPoint = false;
                idleWaitTimer = Random.Range(minIdleWaitTime, maxIdleWaitTime);
                nextIdleCoord = GenerateNextIdlePos(idleStartPos, maxIdleRange);
            }
            else if (!waiting)
            {
                StartCoroutine(StartWaitTimer());
            }

        }
    }



    IEnumerator StartWaitTimer()
    {
        waiting = true;
        yield return new WaitForSeconds(idleWaitTimer);

        movingToNextWaitPoint = true;
        waiting = false;
    }


    public void ProtecWall()
    {
        // When commanded to go to a wall
        // Repairs when no enemies in range
        // Attacks enemies when they are in range

    }
    
    public void CallHelper()
    {
        // Helper stops for a moment and a ! mark pops above their head before starts heading toward target
        StopCoroutine(StartWaitTimer());


        if (currentState == State.Building)
        {
            buildingTile.RemoveBuilder(this);
            buildingTile = null;
        }
        else if (currentState == State.Gathering)
        {
            gatheringTile.RemoveWorker(this);
            gatheringTile = null;
        }



        currentState = State.Following;
        target = player;
        hasTarget = true;
        StartCoroutine(UpdatePath());
    }
    public void CommandToBuild(BuildingTile tile)
    {
        // Walk to the building
        // If the building was full -> back to follow


        currentState = State.Building;
        buildingTile = tile;
        target = tile.transform;
        hasTarget = true;
        tile.AddBuilder(this);
    }
    public void CommandToGather(GatheringArea tile)
    {
        // Walk to the gathering Spot
        // If the building was full -> back to follow


        currentState = State.Gathering;
        gatheringTile = tile;
        tile.AddWorker(this);
    }

    public void GiveTarget(Transform targetTransform, float range = 0)
    {
        targetRange = range;
        target = targetTransform;
        hasTarget = true;
    }

    public void GoToIdle()
    {
        idleStartPos = transform.position;
        idleWaitTimer = Random.Range(minIdleWaitTime, maxIdleWaitTime);
        nextIdleCoord = GenerateNextIdlePos(idleStartPos, maxIdleRange);

        
        hasTarget = false;
        

        currentState = State.Idle;
    }


    IEnumerator UpdatePath()
    {
        float refreshRate = 0.33f;

        while (hasTarget)
        {
            if (currentState == State.Following)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + 1);

                pathfinder.SetDestination(targetPosition);
            }
            else if (currentState == State.Building)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + 2);

                pathfinder.SetDestination(targetPosition);
            }
            else if (currentState == State.Gathering)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetRange);

                pathfinder.SetDestination(targetPosition);

                // Start whacking against target like they would be attacking it.
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
