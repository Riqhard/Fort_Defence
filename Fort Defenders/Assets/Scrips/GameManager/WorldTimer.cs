using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTimer : MonoBehaviour
{
    // In minutes
    public float dayTime = 6;

    int dayNumber;
    bool timeIsDay;
    public Image dayCycle;
    Spawner spawner;

    public event System.Action<int> OnNewDay;
    public event System.Action OnNightTime;

    public void Start()
    {
        dayCycle.fillAmount = 1;
        timeIsDay = true;

    }

    private void Update()
    {
        if (timeIsDay)
        {
            dayCycle.fillAmount -= (1f / dayTime * Time.deltaTime) / 60;
            if (dayCycle.fillAmount <= 0)
            {
                StartNightTime();
            }
        }
    }

    public void StartNightTime()
    {
        Debug.Log("Change to night");
        timeIsDay = false;

        if (OnNightTime != null)
        {
            OnNightTime();
        }
    }

    public void StartANewDay()
    {
        dayCycle.fillAmount = 1;
        timeIsDay = true;
        dayNumber++;

        if (OnNewDay != null)
        {
            OnNewDay(dayNumber);
        }
    }
}
