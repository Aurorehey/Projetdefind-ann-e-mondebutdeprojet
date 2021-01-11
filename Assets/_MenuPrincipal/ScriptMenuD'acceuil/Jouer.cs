using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jouer : MonoBehaviour
{
    public string MapACharger;
    public void JeLanceLeJeu()
    {
        SceneManager.LoadScene(MapACharger);
    }
}
