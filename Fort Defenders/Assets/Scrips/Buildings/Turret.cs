using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public Transform target;
    public float range = 15f;
    public float turnSpeed = 5f;
    public Transform partToRotate;

    [Header("Shooting")]
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;

    float nextShotTime;

    [Header("Gun behaviour")]
    public Vector2 gunKickback = new Vector2(.5f, 2);
    public Vector2 recoilAngles = new Vector2(3, 5);
    public float recoilPosSettleTime = .1f;
    public float recoilAngleSettleTime = .1f;

    float recoilAngle;
    Vector3 recoilSmoothDapVelocity;
    float recoilRotSmoothDapVelocity;

    [Header("Gun Effects")]
    public Transform shellPrefab;
    public Transform shellEjectionPoint;


    


    string enemyTag = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Finding target
    void UpdateTarget()
    {

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget > range)
            {
                target = null;
            }
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDisntance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDisntance)
            {
                shortestDisntance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDisntance <= range)
        {
            target = nearestEnemy.transform;
        }
    }

    public void LateUpdate()
    {
        if (target == null)
        {
            partToRotate.localPosition = Vector3.zero;
            return;
        }
        partToRotate.localPosition = Vector3.SmoothDamp(partToRotate.localPosition, Vector3.zero, ref recoilSmoothDapVelocity, recoilPosSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDapVelocity, recoilAngleSettleTime);
        partToRotate.localEulerAngles = partToRotate.localEulerAngles + Vector3.left * recoilAngle;
    }

    public void Update()
    {
        if (target == null)
        {
            return;
        }
        Vector3 targetPos = target.position + target.forward * 0.5f;
        Vector3 direction = targetPos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRot, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);

        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);


            Instantiate(shellPrefab, shellEjectionPoint.position, shellEjectionPoint.rotation);

            partToRotate.localPosition -= Vector3.forward * Random.Range(gunKickback.x, gunKickback.y);
            recoilAngle += Random.Range(recoilAngles.x, recoilAngles.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 50);
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
