using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerWall : LivingEntity
{
    

    public override void Start()
    {
        base.Start();
    }

   
    public override void Die()
    {
        base.Die();

        LivingEntity[] LES = GetComponentsInChildren<LivingEntity>();


        for (int i = 0; i < LES.Length; i++)
        {
            if (i != 0)
            {
                LES[i].Die();
            }
        }
        
        
        // Death effect

        GameObject.Destroy(gameObject);
    }
}
