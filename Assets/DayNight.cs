using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    //how many seconds is a day
    public float dayLength;
    public Text timeText;

    //second counter
    public float timeSecs = 0f;

    //day counter
    public int day = 0;

    //The current time as a decimal between 0 and 1
    public float timeOfDay;

    //Environment characteristics
    public float temperature;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Time updating
        timeSecs += Time.deltaTime;
        day = (int) (timeSecs / dayLength) + 1;

        timeOfDay = (timeSecs % dayLength) / dayLength;

        float hour = Mathf.Floor(timeOfDay * 24);
        float minutes = Mathf.Floor((timeOfDay * 24 - hour) * 60);

        string foot = "AM";
        if(timeOfDay >= 0.5)
        {
            foot = "PM";
        }

        float hourString = hour % 12;
        if(hourString == 0)
        {
            hourString = 12f;
        }

        timeText.text = hourString + ":" + String.Format("{0:00.}", minutes) + " " + foot;


        //Light Rotation
        transform.rotation = Quaternion.Euler((-timeOfDay * 360) - 90f, 90f, 0f);

        //Skybox Rotation
    }
}
