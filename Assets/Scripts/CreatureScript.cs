using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureScript : MonoBehaviour
{
    // Start is called before the first frame update
    public FoodItem.FoodType Diet;
    public Transform Mouth;
    public float FoodDist = 0.5f;
    public LayerMask FoodMask;
    public LayerMask CreatureMask;
    private GameObject food;
    private FoodItem.FoodType foodtype;
    public GameObject poop;

    public bool hungry;
    private float foodcooldown = 20.0f;
    private float timeStamp;
    private float wandertime;

    bool CanEat;
    bool TouchingFood;
    void Start()
    {
        hungry = false;
        timeStamp = Time.time + 5.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (hungry)
        {
            Debug.Log("Hungry!");
            RaycastHit hit;
            Vector3 p1 = Mouth.position;
            // Cast a sphere wrapping character controller 10 meters forward
            // to see if it is about to hit anything.
            Collider[] arr;
            if ((arr=Physics.OverlapSphere(p1, 10,FoodMask)).Length>0)
            {
                Debug.Log("Food Detected!");
                float step = 1.0f * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, arr[0].gameObject.transform.position, step);

            }
            else {
                if (wandertime > 0)
                {
                    transform.Translate(Vector3.forward * 0.005f);
                    wandertime -= Time.deltaTime;
                }
                else
                {
                    wandertime = Random.Range(2.0f, 10.0f);
                    Wander();
                }

            }
            FoodCheck();
        }
        else
        {
            Debug.Log("Not Hungry!");
            if (wandertime > 0)
            {
                transform.Translate(Vector3.forward * 0.005f);
                wandertime -= Time.deltaTime;
            }
            else
            {
                wandertime = Random.Range(2.0f, 10.0f);
                Wander();
            }
            if (timeStamp <= Time.time)
            {
                hungry = true;
               // Debug.Log("Became Hungry");
            }
        }
    }
    void Wander()
    {
        transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
    }
    void LookAround()
    {
        if (transform.eulerAngles.y < 360) {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 0.1f, 0); 
        } else { transform.eulerAngles = new Vector3(0, 0, 0); }
        
    }
    void FoodCheck()
    {
        TouchingFood = Physics.CheckSphere(Mouth.position, FoodDist, FoodMask);
        if (TouchingFood && (food!=null))
        {
            foodtype = food.GetComponent<FoodItem>().foodGroup;
            Debug.Log(foodtype);
            Debug.Log(Diet);
            CanEat = (foodtype == Diet);
        }
        else { CanEat = false; }

        if (TouchingFood && CanEat)
        {
            Destroy(food);
            Instantiate(poop, transform.position + (transform.up * gameObject.GetComponent<Collider>().bounds.max.y), transform.rotation);
            hungry = false;
            timeStamp = Time.time + foodcooldown;
            Debug.Log("Became Full");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<FoodItem>())
        {
            Debug.Log("FOOD TOUCHING");
            food = collision.gameObject;
        }
        else { food = null; }
    }
}
