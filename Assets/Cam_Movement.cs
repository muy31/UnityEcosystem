using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cam_Movement : MonoBehaviour
{
    public Camera camera;
    public Transform follow;
    public Terrain terrain;

    private float baseSpeed;

    public float fdistance;
    public float mSpeed;
    public float degSpeed;
    public int camState = 0;
    public float cameraAngle;

    //UI Elements
    public Image background;
    public AnimalBehavior currentAnimal;
    public Text animalName;
    public Text animalDescription;
    public Text animalMeters;

    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = mSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            camState = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if(hit.transform != null && hit.collider.tag != "terrain")
            {
                AnimalBehavior animal = hit.transform.gameObject.GetComponent<AnimalBehavior>();
                currentAnimal = animal;
                follow = hit.transform;
                camState = 1;
            }
            else
            {
                currentAnimal = null;
            }
        }

        if (currentAnimal != null)
        {
            background.color = new Color(184f / 255f, 255f / 255f, 147f / 255f, 147f / 255f);
            animalName.text = currentAnimal.animalTag;
            animalDescription.text = currentAnimal.currentAction;
            animalMeters.text = "Food: " + currentAnimal.meters[0] + "\nWater: " + currentAnimal.meters[1] + "\nReproduction: " + currentAnimal.meters[2]+ "\nEnergy: " + currentAnimal.meters[3] + "\nStress: " + currentAnimal.stressMultiplier + "\nAge: " + currentAnimal.age;
        }
        else
        {
            background.color = new Color(184f / 255f, 255f / 255f, 147f / 255f, 0f);
            animalName.text = "";
            animalDescription.text = "";
            animalMeters.text = "";
        }

        if (camState == 0){

            transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            if (Input.GetKey(KeyCode.Z))
            {
                cameraAngle += Time.deltaTime*degSpeed;
            }

            if (Input.GetKey(KeyCode.X))
            {
                cameraAngle -= Time.deltaTime * degSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                mSpeed = 2.5f * baseSpeed;
            }
            else
            {
                mSpeed = baseSpeed;
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * Time.deltaTime * mSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * Time.deltaTime * mSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * mSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(-Vector3.left * Time.deltaTime * mSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                fdistance += mSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                fdistance -= mSpeed * Time.deltaTime;
            }


            transform.position = new Vector3(transform.position.x, fdistance + terrain.SampleHeight(transform.position), transform.position.z);
        }
        else if (camState == 1)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                fdistance += mSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                fdistance -= mSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(follow.position, Vector3.up, degSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(follow.position, Vector3.up, -degSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.RotateAround(follow.position, transform.right, degSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.RotateAround(follow.position, transform.right, -degSpeed * Time.deltaTime);
            }

            

            transform.LookAt(follow);
            transform.position = follow.position - transform.forward * fdistance;
        }

    }

}
