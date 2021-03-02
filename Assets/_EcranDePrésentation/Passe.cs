using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Passe : MonoBehaviour
{
    public string jepasse;

    public void mapagedeprésentation()
    {
        SceneManager.LoadScene(jepasse);
    }
}

