using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDelete : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }
}
