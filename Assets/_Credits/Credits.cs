using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public string ACharger;
    public void JeLanceLesCredits()
    {
        SceneManager.LoadScene(ACharger);
    }
   
}
