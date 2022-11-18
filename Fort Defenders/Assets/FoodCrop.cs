using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCrop : MonoBehaviour
{
    public enum WorkingState { Idle, Gathering};
    [HideInInspector]
    public WorkingState workingState;

    public float gatherTime;

    public Helper workingHelper;
    FoodCatheringArea foodCatheringArea;

    // Start is called before the first frame update
    void Start()
    {
        workingState = WorkingState.Idle;
        foodCatheringArea = GetComponentInParent<FoodCatheringArea>();
    }

    // Update is called once per frame
    void Update()
    {

        if (workingState == WorkingState.Idle && workingHelper != null)
        {
            float distance = Vector3.Distance(transform.position, workingHelper.transform.position);
            if (distance <= 1)
            {
                StartGathering();
            }
        }
        

        if (workingState == WorkingState.Gathering)
        {
            gatherTime -= Time.deltaTime;

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

        if (workingHelper != null)
        {
            foodCatheringArea.FinishedWork(this);
        }
        // Drops food on the ground not give it straight to player.
        FindObjectOfType<PlayerStats>().GiveFood(1);
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
