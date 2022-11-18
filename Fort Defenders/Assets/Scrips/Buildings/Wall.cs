using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : LivingEntity
{

    public GameObject brokeWallGFX;
    public GameObject wallGFX;
    bool deadWall;

    public override void Start()
    {
        base.Start();
        deadWall = false;
    }

    public void Rebuild()
    {
        health = startingHealth;
        wallGFX.SetActive(true);
        deadWall = false;
        dead = false;
        gameObject.layer = 7;
    }

    public override void TakeDamage(float damage)
    {
        if (!deadWall)
        {
            base.TakeDamage(damage);
        }
    }

    public override void Die()
    {
        base.Die();
        deadWall = true;
        // Death effect

        gameObject.layer = 12;
        wallGFX.SetActive(false);
    }

}
