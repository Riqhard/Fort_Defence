using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State { Idle, Chasing, Attacking};
    State currentState;

    public ParticleSystem deathEffect;

    public LayerMask enemyTargetMask;

    public float damage = 1;
    public int goldAmount;

    NavMeshAgent pathfinder;
    Transform target;
    Transform attackTarget;
    LivingEntity targetEntity;
    Material skinMateria;

    Color originalColor;

    [Header("Attacking")]

    public float attackDistance = 1f;
    public float timeBetweenAttacks = 1f;
    public float attackSpeed = 3;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        skinMateria = GetComponent<Renderer>().material;
        originalColor = skinMateria.color;

        if (GameObject.FindGameObjectWithTag("Forth") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Forth").transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            //targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }


    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= health)
        {
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.main.startLifetime.constant);
        }

        base.TakeHit(damage, hitPoint, hitDirection);
    }




    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }


    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Idle)
        {
            if (GameObject.FindGameObjectWithTag("Forth") != null)
            {
                target = GameObject.FindGameObjectWithTag("Forth").transform;
                targetEntity = target.GetComponent<LivingEntity>();

                pathfinder.enabled = true;
                currentState = State.Chasing;
                hasTarget = true;
            }
        }
        
        // Switch from ray hitting a wall to a float range          (float seekRange)
        // Add helpers to the enemyTargetMask
        // Add player to the enemyTargetMask
        // If target goes further away than float range             (float chaseRange)
        // Go to idle

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, enemyTargetMask, QueryTriggerInteraction.Collide))
        {
            if (Time.time > nextAttackTime)
            {
                currentState = State.Attacking;
                pathfinder.enabled = false;

                attackTarget = hit.transform;
                targetEntity = attackTarget.GetComponent<LivingEntity>();
                targetEntity.OnDeath += OnTargetDeath;
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
            return;
        }

        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                // Finding distance to the target
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistance + myCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }



    }


    IEnumerator Attack()
    {
        LivingEntity currentLivingEntity = targetEntity;
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = transform.position + transform.forward;

        
        float percent = 0;

        skinMateria.color = Color.red;
        bool hasAppliedDamage = false;


        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                currentLivingEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        
        skinMateria.color = originalColor;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.33f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + attackDistance/2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }      
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public override void Die()
    {
        base.Die();

        FindObjectOfType<PlayerStats>().GiveGold(goldAmount);
        // Death effect

        GameObject.Destroy(gameObject);
    }
}
