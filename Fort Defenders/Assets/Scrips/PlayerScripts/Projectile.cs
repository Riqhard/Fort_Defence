using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 10f;
    float lifeTime = 3f;
    public LayerMask collisionMask;
    public Color trailColor;
    float damage = 1;
    float skinWidth = .1f;

    public void Start()
    {
        Destroy(gameObject, lifeTime);

        // Getting array of all the colliders our projectile is inside with
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
    }

    public void SetSpeed(float newspeed)
    {
        speed = newspeed;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }
    public void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }
    
    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }

        GameObject.Destroy(gameObject);
    }
}
