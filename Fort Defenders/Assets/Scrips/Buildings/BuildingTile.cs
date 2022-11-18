using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingTile : MonoBehaviour
{
    [HideInInspector]
    public BuildingState buildingState;

    public GameObject interactionUI;

    public GameObject prefabToPlace;
    public int costToBuild;
    public TextMeshProUGUI costText;

    public float buildTime;
    float buildTimer;
    public Vector3 offset;

    BuildingManager buildingManager;
    Player player;
    PlayerStats playerStats;
    [HideInInspector]
    public int buildersBuilding;

    public event System.Action OnBuildingFinished;

    public int maxBuilders;

    private List<Helper> helpers = new List<Helper>();

    void Start()
    {
        player = FindObjectOfType<Player>();
        buildingState = BuildingState.Empty;
        interactionUI.SetActive(false);
        buildingManager = FindObjectOfType<BuildingManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        costText.text = "" + costToBuild;
    }
    public void Update()
    {
        if (buildingState == BuildingState.Building)
        {
            buildTime -= Time.deltaTime * buildersBuilding;

            if (buildTime <= 0)
            {
                // Finished building
                FinishedBuilding();
                
            }
            else
            {
                // Show building timer

            }
        }
    }

    public void FinishedBuilding()
    {
        // Find how far player it

        // If in range set helper target to them and set them to follow target.

        // else
        foreach (Helper helper in helpers)
        {
            helper.GoToIdle();
        }

        
        if (OnBuildingFinished != null)
        {
            OnBuildingFinished();
        }


        Instantiate(prefabToPlace, transform.position + offset, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (buildingState != BuildingState.Building)
            {
                buildingState = BuildingState.Showing;
                interactionUI.SetActive(true);

                // Show building hologram.

            }
            else
            {
                // show how many builders does the construction has.

            }
            buildingManager.PlayerInBuildingZone(this);

        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (buildingState != BuildingState.Building)
            {
                buildingState = BuildingState.Empty;
            }
            interactionUI.SetActive(false);            
            buildingManager.PlayerLeavesBuildingZone();
        }
    }

    public void AddBuilder(Helper helper)
    {
        if (!helpers.Contains(helper))
        {
            helpers.Add(helper);
        }
        buildersBuilding++;
    }

    public void RemoveBuilder(Helper helperToRemove)
    {
        
        if (helpers.Count != 0)
        {

            if (helpers.Contains(helperToRemove))
            {
                helpers.Remove(helperToRemove);
            }

            buildersBuilding--;
        }
    }

    public void BuildBuilding()
    {
        if (buildingState != BuildingState.Building)
        {
            // Start building
            buildingState = BuildingState.Building;
            playerStats.TakeGold(costToBuild);
            interactionUI.SetActive(false);

            // Show building progress.
        }  
    }
}
public enum BuildingState { Empty, Building, Showing };


