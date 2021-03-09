using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject Spotlight;
    public bool isOn = false;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
            isOn = !isOn;
        { 
            if (isOn == true)
            {
                Spotlight.SetActive(true);
            }
            if (isOn == false)
            {
                Spotlight.SetActive(false);
            }
        }
        


        
        
    }
}
