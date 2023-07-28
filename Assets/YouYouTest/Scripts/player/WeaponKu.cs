using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKu : MonoBehaviour
{
    public GameObject[] weapons;
    // Start is called before the first frame update
    

    
    void Start() 
    {
        //foreach all weapons,and if it has mesh renderer,then set it to false
        foreach (GameObject weapon in weapons)
        {
            if (weapon.GetComponent<MeshRenderer>())
            {
                weapon.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
