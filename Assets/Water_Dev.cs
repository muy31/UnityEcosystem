using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Dev : MonoBehaviour
{
    public GameObject water;
    public Terrain ground;
    public int waterAmount;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < waterAmount; i++)
        {
            Vector3 location = new Vector3(Random.Range(-500f, 500f), 0f, Random.Range(-500f, 500f));
            GameObject w = Instantiate(water, new Vector3(location.x, ground.SampleHeight(location) + 30f, location.z), Quaternion.Euler(0f, 0f, 0f), transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount < waterAmount)
        {
            for (int i = 0; i < (waterAmount - transform.childCount); i++)
            {
                Vector3 location = new Vector3(Random.Range(-500f, 500f), 0f, Random.Range(-500f, 500f));
                GameObject w = Instantiate(water, new Vector3(location.x, ground.SampleHeight(location) + 30f, location.z), Quaternion.Euler(0f, 0f, 0f), transform);
            }
        }

    }
}
