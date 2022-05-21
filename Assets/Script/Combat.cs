using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    
    public int joueurhp = 40;
    public int ennemiehp = 20;

    public int joueurattaque = 5;
    public int ennemieattaque = 7;

    public int quantitedesoin = 5;
    public string fin;
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    public void JeQuitteLeJeu()
    {

        SceneManager.LoadScene(fin);
    }

    // Update is called once per frame
    void Update()
    {
        if (joueurhp == 0)
        {
            Debug.Log("Fin");
            JeQuitteLeJeu();

        }
    }

   
    
        

    
        

    
}
