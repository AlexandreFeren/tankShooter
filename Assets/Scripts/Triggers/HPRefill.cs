using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRefill : MonoBehaviour
{
    // Start is called before the first frame update

    public static Vector3 refillWaypoint;
    float originalY;
    public float floatStrength = .25f; // You can change this in the Unity Editor to 
                                       // change the range of y positions that are possible.

    GameManager manager;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.originalY = this.transform.position.y;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Mathf.Sin(Time.time * 2) * floatStrength),
            transform.position.z);
        transform.Rotate(0.0f, 1.0f, 0.0f, Space.Self);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
        {
            TankHealth script = other.GetComponent<TankHealth>();
            //Debug.Log(script.m_CurrentHealth);
            if (script.m_CurrentHealth < 100)
            {
                script.m_CurrentHealth += Mathf.Max(.15f, Mathf.Pow((100-script.m_CurrentHealth)/100, 2));
            }
            else
            {
                script.m_CurrentHealth = 100;
            }
            
            script.SetHealthUI();

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
        {
            int waypointNumber = Random.Range(0, manager.AIWaypoints.Count);
            if (manager.AIWaypoints[waypointNumber].transform.position == AmmoRefill.refillWaypoint)
            {
                waypointNumber = (waypointNumber + 1) % manager.AIWaypoints.Count;
            }
            refillWaypoint = manager.AIWaypoints[waypointNumber].transform.position;
            this.transform.position = manager.AIWaypoints[waypointNumber].position;
        }
    }
}
