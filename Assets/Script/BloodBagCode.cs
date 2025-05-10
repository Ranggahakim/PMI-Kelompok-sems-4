using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBagCode : MonoBehaviour
{
    [SerializeField] PlayerStats myPlayerStats;
    // Start is called before the first frame update
    void Start()
    {
        myPlayerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        myPlayerStats.totalCollectedBody++;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            myPlayerStats.ObtainCollectedBody();
            Destroy(gameObject);
        }
    }
}
