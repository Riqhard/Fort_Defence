using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringArea : MonoBehaviour
{
    public GameObject interactionUI;

    [HideInInspector]
    public int workersAmount;
    [HideInInspector]
    public int maxWorkersAmount;

    BuildingManager buildingManager;

    public virtual void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
        interactionUI.SetActive(false);
    }

    public virtual void AddWorker(Helper helper)
    {
        
    }

    public virtual void RemoveWorker(Helper helperToRemove)
    {

        
    }



    // TRIGGER HANDLING
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {

            buildingManager.PlayerInFoodZone(this);
            interactionUI.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            buildingManager.PlayerLeavesFoodZone();
            interactionUI.SetActive(false);
        }
    }
}
