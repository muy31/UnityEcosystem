using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public DayNight environment;
    public GameObject[] animalsToSpawn;
    public int[] numberOfObjects;
    public Terrain ground;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < animalsToSpawn.Length; i++)
        {
            for (int j = 0; j < numberOfObjects[i]; j++)
            {
                Vector3 location = new Vector3(Random.Range(-500f, 500f), 0f, Random.Range(-500f, 500f));
                GameObject obj = Instantiate(animalsToSpawn[i], new Vector3(location.x, ground.SampleHeight(location), location.z), Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f), transform);

                AnimalBehavior animal = obj.GetComponent<AnimalBehavior>();
                if (animal != null)
                {
                    animal.environment = environment;
                }

            }
        }
    }
}
