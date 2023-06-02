using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavior : MonoBehaviour
{
    public GameObject mchild;
    public GameObject fchild;

    //Characteristics
    public float[] meters;

    public float animalSpeed;
    public float lineofSightRadius;
    public float senseRadius;
    public float lineOfSightDegrees;
    public float matingDuration;
    public float reproductiveSuccessRate;

    public float bodyTemperature;
    public float sleepThreshold;
    public float sleepDuration;
    public bool coldBlooded;
    public bool sleeping;

    private Vector3 randomTarget;

    //Rates
    public float basicHungerRate;
    public float basicThirstRate; //should be higher than hungerRate
    public float reproductiveRate; //how much the reproductiveEnergyDepletes per second
    public float basicTireRate;
    public float temperatureChangeRate;

    public float stressMultiplier;
    public float timeBasedSleepMultiplier;
    public float age;

    public float mateDistance;

    //Sensors
    public DayNight environment;

    public string[] eat_tags; //what this animal eats
    public string animalTag;
    public int male;
    
    public Consumable myEatabilty;

    //Movement
    public NavMeshAgent agent;
    [SerializeField] Vector3 target;

    /*
     * Meters work by doing the action that it is the most immediate of the necessary behaviors... the lowest meter
     * 
     * 0 = energyHunger; when 0, animal dies 
     * 1 = energyThirst; when 0, animal dies
     * 2 = reproductiveEnergy; when 0, animal is stressed
     * 3 = sleepNeed, increases when animal is sleeping. when 0, animal is stressed
     * 4 = 
     * 
     * 
     *
     * 
     *
     * 
     */

    //Actions
    public string currentAction;
    [SerializeField] string collidedWith;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = animalSpeed;
        StartCoroutine(randomRotation());
        mchild.GetComponent<AnimalBehavior>().environment = environment;
        fchild.GetComponent<AnimalBehavior>().environment = environment;
    }

    IEnumerator randomRotation()
    {
        while (true)
        {
            float seconds = Random.Range(2f, 5f);
            randomTarget = new Vector3(Random.Range(transform.position.x - lineofSightRadius, transform.position.x + lineofSightRadius), 0f, Random.Range(transform.position.z - lineofSightRadius, transform.position.z + lineofSightRadius));
            yield return new WaitForSeconds(seconds);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

        age += Time.deltaTime;
        DecreaseNeed();

        int choose = decideMeters();
        bool foundTarget = false;
        if(choose == 0)
        {
            target = findFood();
            if(target != new Vector3(0f, -200f, 0f))
            {
                foundTarget = true;
                currentAction = "Looking for food...";
            }
        }
        else if(choose == 1)
        {
            target = findWater();
            if (target != new Vector3(0f, -200f, 0f))
            {
                foundTarget = true;
                currentAction = "Looking for water...";
            }
        }
        else if(choose == 2)
        {
            target = findMateSexual();
            if (target != new Vector3(0f, -200f, 0f))
            {
                foundTarget = true;
                currentAction = "Looking for mate...";
            }
        }
        else if(choose == 3)
        {
            target = transform.position;
            currentAction = "Wanting to sleep...";

            if (meters[3] <= sleepThreshold)
            {
                sleeping = true;
            }

            
        }


        if (!foundTarget && !sleeping)
        {
            target = randomTarget;
            currentAction = "Looking around randomly";
        }

        Vector3 danger = findDanger();

        if (!sleeping)
        {
            if (danger != new Vector3(0f, -200f, 0f) && meters[0] >= 5f && meters[1] >= 5f)
            {
                currentAction = "Running Away!";
                //away from location
                Vector3 direction = (danger - transform.position).normalized * lineofSightRadius;
                target = transform.position - danger;
            }
        }


        if (agent.enabled)
        {
            agent.SetDestination(target);
        }
        
    }

    void DecreaseNeed()
    {
        if (!sleeping)
        {
            agent.enabled = true;
            meters[0] -= basicHungerRate * (1 + stressMultiplier) * Time.deltaTime;
            meters[1] -= basicThirstRate * (1 + stressMultiplier) * Time.deltaTime;
            meters[2] -= reproductiveRate * Time.deltaTime;
            meters[3] -= basicTireRate * (1 + timeBasedSleepMultiplier) * Time.deltaTime;
        }
        else
        {
            agent.enabled = false;
            meters[0] -= basicHungerRate * 0.9f * Time.deltaTime;
            meters[1] -= basicThirstRate * 0.9f * Time.deltaTime;
            meters[2] += 0;
            meters[3] += basicTireRate * (1 + timeBasedSleepMultiplier) * Time.deltaTime;
            
            currentAction = "Sleeping...";
            
            //Wake Up
            if (meters[3] > sleepThreshold + sleepDuration || meters[0] <= 10f || meters[1] <= 10f)
            {
                sleeping = false;
            }

        }





        bodyTemperature += (environment.temperature - bodyTemperature) * Time.deltaTime * temperatureChangeRate;
        age += Time.deltaTime;

        if(meters[0] < 0 || meters[1] < 0)
        {
            myEatabilty.alive = false;
        }

        if (meters[2] < 0 || meters[3] < 0)
        {
            stressMultiplier += Time.deltaTime * 0.01f;
        }
    }

    int decideMeters()
    {
        int index = 0;
        float minValue = meters[0];

        for(int i = 1; i < meters.Length; i++)
        {
            if(meters[i] < minValue)
            {
                minValue = meters[i];
                index = i;
            }
        }

        return index;
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Consumable prey = collision.gameObject.GetComponent<Consumable>();

        if (prey != null)
        {
            if (isPrey(prey))
            {
                meters[0] += prey.foodEnergy;
                meters[1] += prey.thirstEnergy;
                prey.alive = false;
            }
        }
    }
    */

    private void OnTriggerEnter(Collider collision)
    {
        Consumable prey = collision.gameObject.GetComponent<Consumable>();
        collidedWith = collision.gameObject.ToString();

        if (prey != null)
        {
            if (isPrey(prey))
            {
                meters[0] += prey.foodEnergy;
                meters[1] += prey.thirstEnergy;
                prey.alive = false;
                currentAction = "Eating " + prey.c_tag;
            }
        }
    }

    public bool isPrey(Consumable prey)
    {
        bool isPrey = false;
        foreach (string tag in eat_tags)
        {
            if (prey.c_tag == tag)
            {
                isPrey = true;
            }
        }

        return isPrey;
    }

    Vector3 sleepLocation()
    {
        return transform.position;
    }

    Vector3 findMateSexual()
    {
        Collider[] objectsOfInterests = Physics.OverlapSphere(transform.position, lineofSightRadius);

        float closestDistance = float.MaxValue;
        Vector3 location = new Vector3(0f, -200f, 0f);

        foreach (Collider c in objectsOfInterests)
        {
            AnimalBehavior mate = c.GetComponent<AnimalBehavior>();
            if (mate != null)
            {
                if (mate.animalTag == animalTag && mate.male == (1 - male))
                {
                    float distance = Vector3.Distance(transform.position, c.transform.position);                
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        location = c.transform.position;
                    }
                }
            }
        }

        if(Vector3.Distance(location, transform.position) < mateDistance)
        {
            StartCoroutine(mating(matingDuration));
        }

        return location;
    }

    public IEnumerator mating(float seconds)
    {
        meters[2] += 10f;
        yield return new WaitForSeconds(seconds);
        float a = Random.Range(0f, 1f);
        if(a < reproductiveSuccessRate && (male == 1 || male == 2))
        {
            int malle = Random.Range(0, 2);
            if(male == 0)
            {
                GameObject kin = Instantiate(mchild, transform.position, transform.rotation, transform.parent);
            }
            else
            {
                GameObject kin = Instantiate(fchild, transform.position, transform.rotation, transform.parent);
            }  
        }

    }

    Vector3 findFood()
    {
        Collider[] objectsOfInterests = Physics.OverlapSphere(transform.position, lineofSightRadius);

        float closestDistance = float.MaxValue;
        Vector3 location = new Vector3(0f, -200f, 0f);

        foreach (Collider c in objectsOfInterests)
        {
            if(c.tag == "water")
            {
                continue;
            }

            Consumable prey = c.GetComponent<Consumable>();
            if(prey != null)
            {
                if (isPrey(prey))
                {
                    float distance = Vector3.Distance(transform.position, c.transform.position);
                    
                    if((Mathf.Abs(Vector3.Angle(transform.forward, c.transform.position - transform.position)) < lineOfSightDegrees && distance <= lineofSightRadius) || distance < senseRadius)
                    {
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            location = c.transform.position;
                        }
                    }

                }
            }
        }

        return location;
    }

    Vector3 findDanger()
    {
        Collider[] objectsOfInterests = Physics.OverlapSphere(transform.position, lineofSightRadius);

        float closestDistance = float.MaxValue;
        Vector3 location = new Vector3(0f, -200f, 0f);

        foreach (Collider c in objectsOfInterests)
        {
            AnimalBehavior predator = c.GetComponent<AnimalBehavior>();
            if (predator != null)
            {
                if (predator.isPrey(myEatabilty))
                {
                    float distance = Vector3.Distance(transform.position, c.transform.position);

                    if ((Mathf.Abs(Vector3.Angle(transform.forward, c.transform.position - transform.position)) < lineOfSightDegrees && distance <= lineofSightRadius) || distance < senseRadius)
                    {
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            location = c.transform.position; //+ c.transform.forward*agent.speed;
                        }
                    }

                }
            }
        }

        return location;
    }

    Vector3 findWater()
    {
        Collider[] objectsOfInterests = Physics.OverlapSphere(transform.position, lineofSightRadius);

        float closestDistance = float.MaxValue;
        Vector3 location = new Vector3(0f, -200f, 0f);

        foreach (Collider c in objectsOfInterests)
        {
            
            if (c.tag == "water")
            {

                float distance = Vector3.Distance(transform.position, c.transform.position);
                
                if ((Mathf.Abs(Vector3.Angle(transform.forward, c.transform.position - transform.position)) < lineOfSightDegrees && distance <= lineofSightRadius) || distance < senseRadius)
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        location = c.transform.position;
                    }
                }
   
            }
        }


        return location;
    }

}
