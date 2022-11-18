using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    public float moveSpeed = 5;
    PlayerController controller;
    Camera viewCamera;
    GunController gunController;
    BuildingManager buildingManager;
    public GameObject callSphere;


    Queue<Helper> helpers;

    bool isCalling;

    [Header("Helper interactions")]
    public LayerMask helperMask;
    public float callSpeed = 1;
    public float callRange = 5f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        buildingManager = FindObjectOfType<BuildingManager>();
        viewCamera = Camera.main;
        helpers = new Queue<Helper>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;

        controller.Move(moveVelocity);

        // Look Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            // Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
            if ((new Vector2 (point.x, point.y) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
            {
                gunController.Aim(point);
            }
            

        }

        // Zoom input

            //Scroll up zooms in and scroll down zoom out.  PlayerController.Zoom(float)



        // Weapon Input
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }

        


        // Helper Input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (helpers.Count != 0)
            {
                if (buildingManager.PlayerWantsToBuild())
                {
                    Helper helperToSent = helpers.Dequeue();
                    helperToSent.CommandToBuild(buildingManager.currentBuildingTile);
                }
                else if (buildingManager.PlayerWantsToGather())
                {
                    Helper helperToSent = helpers.Dequeue();
                    helperToSent.CommandToGather(buildingManager.currentGatheringArea);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isCalling)
        {
            Collider[] helpersInRange = Physics.OverlapSphere(transform.position, callRange/2, helperMask);
            if (helpersInRange.Length > 0)
            {
                foreach (Collider helper in helpersInRange)
                {
                    if (!helpers.Contains(helper.GetComponent<Helper>()))
                    {
                        helper.GetComponent<Helper>().CallHelper();
                        helpers.Enqueue(helper.GetComponent<Helper>());
                    }
                }

            }

            // Cast a field for a second around you showing the range of the call method.
            StartCoroutine(animateCall());
        }

    }

    IEnumerator animateCall()
    {
        isCalling = true;
        callSphere.SetActive(true);
        float percent = 0;

        Vector3 initialScale = new Vector3(1f, 1f, 1f);

        while (percent < 1)
        {
            percent += Time.deltaTime * callSpeed;
            float interploation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float callScaling = Mathf.Lerp(0, callRange, interploation);
            callSphere.transform.localScale = initialScale * callScaling;


            yield return null;
        }

        yield return new WaitForSeconds(1f);
        isCalling = false;
    }


    public override void Die()
    {
        base.Die();

        // Death effect

        GameObject.Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, callRange / 2);
    }
}
