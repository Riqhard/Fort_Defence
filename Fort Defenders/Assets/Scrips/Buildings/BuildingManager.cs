using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public BuildingTile currentBuildingTile;
    public GatheringArea currentGatheringArea;
    PlayerStats stats;

    public void Start()
    {
        currentBuildingTile = null;
        currentGatheringArea = null;
        stats = FindObjectOfType<PlayerStats>();
    }




    // BUILDING AREAS

    public void SpawnBuilding(Transform buildingToSpawn, Vector3 location, float rotation)
    {
        Instantiate(buildingToSpawn, location, Quaternion.Euler(0, rotation, 0));
    }
    public void PlayerInBuildingZone(BuildingTile buildingTile)
    {
        currentBuildingTile = buildingTile;
    }
    public void PlayerLeavesBuildingZone()
    {
        currentBuildingTile = null;
    }
    public bool PlayerWantsToBuild()
    {
        if (currentBuildingTile != null)
        {
            if (currentBuildingTile.buildingState == BuildingState.Building)
            {
                if (currentBuildingTile.buildersBuilding < currentBuildingTile.maxBuilders)
                {
                    return true;
                }
            }
            else if (stats.playerGold >= currentBuildingTile.costToBuild)
            {
                currentBuildingTile.BuildBuilding();
                return true;
            }
            
        }

        return false;
    }






    // GATHERING AREAS

    public bool PlayerWantsToGather()
    {
        if (currentGatheringArea != null)
        {
            if (currentGatheringArea.workersAmount < currentGatheringArea.maxWorkersAmount)
            {
                return true;
            }
        }
        return false;
    }

    public void PlayerInFoodZone(GatheringArea gatheringTile)
    {
        currentGatheringArea = gatheringTile;
    }
    public void PlayerLeavesFoodZone()
    {
        currentGatheringArea = null;
    }

}
