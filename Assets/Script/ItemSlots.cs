using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

    
public class ItemSlots : MonoBehaviour
{
    public Text textItem;
    public GameObject textDisplay;


    [Header("Item's Datas")]
    public string itemType;
    public string itemID;
    public Sprite itemSprite;
    public string itemDescription;
    public bool itemReutilisable = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = itemSprite; //modification de l'image dans image(script) dans unity
        textItem.text = itemDescription;
    }
    private void OnEnable()
    {
        DisableText(); //appeller a chaque fois que l'oject en question est activer.inventaire activer la fonction Enable appeller.
    }
    public void ActiveText()//activer la description 
    {
        textDisplay.SetActive(true);
    }
    public void DisableText()//désactiver la description
    {
        textDisplay.SetActive(false);
    }
    public void Click(BaseEventData bed)
    {
        PointerEventData ped = (PointerEventData)bed;// ou = bed as PointerEventData. en gros on veut la meme chose que baseEventData mais du point de vue du pointer.
        //Debug.Log("Button : " + ped.button);
        if(ped.button == PointerEventData.InputButton.Left)
        {
            TakeItem();
        }
        else if(ped.button == PointerEventData.InputButton.Right)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<FPCSupport>().ActiviteItemOptions(this.gameObject);
        }
    }
     void TakeItem()//peut enlever le publique car plus appeler par eventtrigger
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<FPCSupport>().YouAreoldingItem(this.gameObject, itemType,itemID,itemSprite,itemReutilisable);
    }

}
