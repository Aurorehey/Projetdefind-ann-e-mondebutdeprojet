using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retour : MonoBehaviour
{
    public string jecharge;
    public void mapagedacceuil()
    {
        SceneManager.LoadScene(jecharge);
    }
}
