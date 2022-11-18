using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode { Auto, Burst, Single, Shotgun};
    public FireMode firemode;


    public Transform[] muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;
    public int burstFireCount;
    public int projectilesMag;

    [Header("Gun behaviour")]
    public Vector2 gunKickback = new Vector2(.5f, 2);
    public Vector2 recoilAngles = new Vector2(3, 5);
    public float recoilPosSettleTime = .1f;
    public float recoilAngleSettleTime = .1f;

    public float reloadTime = .4f;


    [Header("Gun Effects")]
    public Transform shellPrefab;
    public Transform shellEjectionPoint;

    float nextShotTime;

    bool TriggerReleasedSinceLastShot;
    int projectilesRemainingInMagasine;
    bool isReloading;

    Vector3 recoilSmoothDapVelocity;
    float recoilRotSmoothDapVelocity;
    float recoilAngle;
    

    private void Start()
    {
        projectilesRemainingInMagasine = projectilesMag;
        TriggerReleasedSinceLastShot = true;
    }

    public void LateUpdate()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDapVelocity, recoilPosSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDapVelocity, recoilAngleSettleTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (!isReloading && projectilesRemainingInMagasine <= muzzle.Length)
        {
            Reload();
        }
    }
       

    void Shoot()
    {
        
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMagasine > 0)
        {
            if (firemode == FireMode.Burst)
            {

                nextShotTime = Time.time + msBetweenShots / 1000;

                if (!TriggerReleasedSinceLastShot)
                {
                    return;
                }
                TriggerReleasedSinceLastShot = false;
                StartCoroutine(ShootBurst());
                return;

            }
            else if (firemode == FireMode.Single)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                
                if (!TriggerReleasedSinceLastShot)
                {
                    return;
                }
                TriggerReleasedSinceLastShot = false;
            }
            else if (firemode == FireMode.Shotgun)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
            }
            else if (firemode == FireMode.Auto)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
            }

            for (int i = 0; i < muzzle.Length; i++)
            {
                Projectile newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
                projectilesRemainingInMagasine--;
            }

            

            Instantiate(shellPrefab, shellEjectionPoint.position, shellEjectionPoint.rotation);

            transform.localPosition -= Vector3.forward * Random.Range(gunKickback.x, gunKickback.y);
            recoilAngle += Random.Range(recoilAngles.x, recoilAngles.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 50);
        }

    }
    IEnumerator ShootBurst()
    {

        for (int i = 0; i < burstFireCount; i++)
        {
            Projectile newProjectile = Instantiate(projectile, muzzle[0].position, muzzle[0].rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
            projectilesRemainingInMagasine--;
            yield return new WaitForSeconds(.05f);
        }

    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMagasine != projectilesMag)
        {
            StartCoroutine(animateReload());
        }
        
    }

    IEnumerator animateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float percent = 0;
        float reloadSpeed = 1/reloadTime;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30f;

        while (percent<1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interploation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interploation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;


            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMagasine = projectilesMag;
    }

    public void Aim(Vector3 aimpoint)
    {
        
        transform.LookAt(aimpoint);
    }



    public void OnTriggerHold()
    {
        Shoot();
    }
    public void OnTriggerReleased()
    {
        TriggerReleasedSinceLastShot = true;
    }
}
