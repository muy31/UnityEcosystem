using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsexualReproduction : MonoBehaviour
{
    public float timeDuration;
    public float timeRange;

    public float growthDistance;
    public Terrain terrain;

    public bool reproductionEnabled;

    // Start is called before the first frame update
    void Start()
    {
        if(reproductionEnabled)
            StartCoroutine(Produce());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Produce()
    {
        yield return new WaitForSeconds(timeDuration + Random.Range(0f, timeRange));

        
        Vector3 randomLocationNearby = new Vector3(transform.position.x + Random.Range(-growthDistance, growthDistance), 0f, transform.position.z + Random.Range(-growthDistance, growthDistance));
        while(Mathf.Abs(randomLocationNearby.x) > 500f || Mathf.Abs(randomLocationNearby.z) > 500f)
        {
            randomLocationNearby = new Vector3(transform.position.x + Random.Range(-growthDistance, growthDistance), 0f, transform.position.z + Random.Range(-growthDistance, growthDistance));
        }


        GameObject ntree = Instantiate(gameObject, new Vector3(randomLocationNearby.x, terrain.SampleHeight(randomLocationNearby), randomLocationNearby.z), Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f), transform.parent);
    }
}
