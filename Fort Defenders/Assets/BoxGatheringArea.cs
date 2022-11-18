using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGatheringArea : GatheringArea
{

    int currentBoxAmount;

    public float spawningRange;
    public int amountToSpawnBoxes;

    Queue<BoxToGather> boxes;
    private List<BoxToGather> gatheredBoxes = new List<BoxToGather>();
    public GameObject boxPrefab;

    private List<Helper> helpers = new List<Helper>();

    public override void Start()
    {
        base.Start();
        boxes = new Queue<BoxToGather>();

        for (int i = 0; i < amountToSpawnBoxes; i++)
        {
            SpawnBoxes();
        }

        maxWorkersAmount = currentBoxAmount;
    }

    public void SpawnBoxes()
    {
        
        Vector3 randomSpawnPos = new Vector3(Random.Range(-spawningRange, spawningRange), -0.5f, Random.Range(-spawningRange, spawningRange));
        GameObject newFood = Instantiate(boxPrefab, transform.position + randomSpawnPos, transform.rotation, transform);
        boxes.Enqueue(newFood.GetComponent<BoxToGather>());
        currentBoxAmount++;
    }


    public void FinishedWork(BoxToGather box)
    {
        Helper helper = box.workingHelper;
        gatheredBoxes.Remove(box);
        currentBoxAmount--;

        if (boxes.Count > 0)
        {
            // Go to next Food 
            BoxToGather curBox = boxes.Dequeue();
            curBox.workingHelper = helper;
            gatheredBoxes.Add(curBox);

            // Set helper target to curFood
            helper.GiveTarget(curBox.transform, 0.5f);

        }
        else
        {
            // Go to Idle
            helper.GoToIdle();
            helpers.Remove(helper);
            workersAmount--;
        }

        if (currentBoxAmount == 0)
        {
            Destroy(gameObject);
        }
    }



    public override void AddWorker(Helper helper)
    {
        if (boxes.Count == 0)
        {
            // Helper goes to transform.
            helper.GiveTarget(transform);
            // Helper goes back to follow player
            helper.GoToIdle();
            return;
        }
        helpers.Add(helper);
        workersAmount++;

        BoxToGather curBox = boxes.Dequeue();
        curBox.workingHelper = helper;
        gatheredBoxes.Add(curBox);

        // Set helper target to curFood
        helper.GiveTarget(curBox.transform, 0.5f);
    }

    public override void RemoveWorker(Helper helperToRemove)
    {

        if (helpers.Count != 0)
        {
            if (helpers.Contains(helperToRemove))
            {
                helpers.Remove(helperToRemove);
                workersAmount--;

                BoxToGather itemToRemove = null;

                foreach (BoxToGather item in gatheredBoxes)
                {
                    if (item.workingHelper == helperToRemove)
                    {
                        item.StopGathering();
                        boxes.Enqueue(item);
                        itemToRemove = item;

                    }

                }

                gatheredBoxes.Remove(itemToRemove);
            }
        }
    }
}
