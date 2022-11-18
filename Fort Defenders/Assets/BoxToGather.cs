using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxToGather : MonoBehaviour
{
    public enum WorkingState { Idle, Gathering };
    [HideInInspector]
    public WorkingState workingState;

    public float gatherTime;

    public Helper workingHelper;
    BoxGatheringArea boxGatheringArea;

    // Start is called before the first frame update
    void Start()
    {
        workingState = WorkingState.Idle;
        boxGatheringArea = GetComponentInParent<BoxGatheringArea>();
    }

    // Update is called once per frame
    void Update()
    {

        if (workingState == WorkingState.Idle && workingHelper != null)
        {
            float distance = Vector3.Distance(transform.position, workingHelper.transform.position);
            if (distance <= 1.5f)
            {
                StartGathering();
            }
        }


        if (workingState == WorkingState.Gathering)
        {
            gatherTime -= Time.deltaTime;

            // Wiggle transform

            if (gatherTime <= 0)
            {
                // Finished building
                FinishGathering();

            }
            else
            {
                // Show building timer

            }
        }
    }

    public void FinishGathering()
    {
        // Give food
        workingState = WorkingState.Idle;

        // Drops tools on the ground not give it straight to player.
        FindObjectOfType<PlayerStats>().GiveGold(1);
        boxGatheringArea.FinishedWork(this);



        Destroy(gameObject);
    }
    public void StartGathering()
    {
        workingState = WorkingState.Gathering;
    }
    public void StopGathering()
    {
        workingHelper = null;
        workingState = WorkingState.Idle;

    }
}
