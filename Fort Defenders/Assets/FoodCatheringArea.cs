using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCatheringArea : GatheringArea
{
    public float spawningRange;
    public int workerAllowedToWorkHere;
    [HideInInspector]
    public int currentFoodAmount;
    public int maxFoodAmount;
    Queue<FoodCrop> foods;
    private List<FoodCrop> gatheredFoods = new List<FoodCrop>();
    public GameObject foodPrefab;

    private List<Helper> helpers = new List<Helper>();

    public float foodRefreshRate;
    [HideInInspector]
    public float foodTimer;

    public GameObject connectionObject;

    bool fieldDestroyed = false;

    public override void Start()
    {
        base.Start();

        maxWorkersAmount = workerAllowedToWorkHere;

        foods = new Queue<FoodCrop>();
        foodTimer = foodRefreshRate;

        for (int i = 0; i < maxFoodAmount; i++)
        {
            SpawnFood();
        }

        if (connectionObject != null){
            connectionObject.GetComponent<BuildingTile>().OnBuildingFinished += DestroyThisField;
        }
    }
    public void Update()
    {
        // Spawn food every 1min

        foodTimer -= 1f * Time.deltaTime;

        if (foodTimer <= 0)
        {
            foodTimer = foodRefreshRate;
            SpawnFood();
        }
    }
    public void SpawnFood()
    {
        if (currentFoodAmount >= maxFoodAmount)
        {
            return;
        }
        Vector3 randomSpawnPos = new Vector3(Random.Range(-spawningRange, spawningRange), 0, Random.Range(-spawningRange, spawningRange));
        GameObject newFood = Instantiate(foodPrefab, transform.position + randomSpawnPos, transform.rotation, transform);
        foods.Enqueue(newFood.GetComponent<FoodCrop>());
        currentFoodAmount++;
    }



    public void DestroyThisField()
    {
        fieldDestroyed = true;
        foreach (FoodCrop item in foods)
        {
            item.FinishGathering();
        }
        foreach (FoodCrop item in gatheredFoods)
        {
            item.FinishGathering();
        }
        Destroy(gameObject);
    }




    // BUILDERS

    public void FinishedWork(FoodCrop foodCrop)
    {
        
        Helper helper = foodCrop.workingHelper;

        if (fieldDestroyed)
        {
            helper.GoToIdle();
            return;
        }
        gatheredFoods.Remove(foodCrop);
        currentFoodAmount--;

        if (foods.Count > 0)
        {
            // Go to next Food 
            FoodCrop curFood = foods.Dequeue();
            curFood.workingHelper = helper;
            gatheredFoods.Add(curFood);

            // Set helper target to curFood
            helper.GiveTarget(curFood.transform);

        }
        else
        {
            // Go to Idle
            helper.GoToIdle();
            helpers.Remove(helper);
            workersAmount--;
        }
    }

    public override void AddWorker(Helper helper)
    {
        if (foods.Count == 0)
        {
            // Helper goes to transform.
            helper.GiveTarget(transform);
            // Helper goes back to follow player
            helper.GoToIdle();
            return;
        }
        helpers.Add(helper);
        workersAmount++;

        FoodCrop curFood = foods.Dequeue();
        curFood.workingHelper = helper;
        gatheredFoods.Add(curFood);

        // Set helper target to curFood
        helper.GiveTarget(curFood.transform);
    }

    public override void RemoveWorker(Helper helperToRemove)
    {

        if (helpers.Count != 0)
        {
            if (helpers.Contains(helperToRemove))
            {
                helpers.Remove(helperToRemove);
                workersAmount--;

                FoodCrop itemToRemove = null;

                foreach (FoodCrop item in gatheredFoods)
                {
                    if (item.workingHelper == helperToRemove)
                    {
                        item.StopGathering();
                        foods.Enqueue(item);
                        itemToRemove = item;
                        
                    }
                    
                }

                gatheredFoods.Remove(itemToRemove);
            }
        }
    }



    
}
