using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAction: MonoBehaviour
{
    public bool needItem = false;
    [Header("si Item est Vrai")]
    public string itemType;
    public string itemID;// let null insector if not necessary
    public string textwithoutItem;//texte afficher sans item en main
    public string textwithoutRightIDItem;//texte afficher sans le bon identifiant item


    public void DoActionNow()
    {
        gameObject.SetActive(false);
    }

}
