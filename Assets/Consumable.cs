using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public GameObject deadBody;
    public float foodEnergy;
    public float thirstEnergy;
    public string c_tag;
    public bool alive;

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
        {
            Die();
        }
    }

    void Die()
    {
        if(deadBody != null)
        {
            Instantiate(deadBody, transform.position, transform.rotation);
        }


        Destroy(gameObject);
    }
}
