using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public Transform[] boids;
    public float separation;
    public float alignment;
    public float centripetal;
    public float radiusSight;
    public float degreesSight;

    public float moveSpeed;
    public float turnSpeed;

    // Update is called once per frame
    void Update()
    {
        foreach(Transform boid in boids)
        {
            
        }
    }
}
