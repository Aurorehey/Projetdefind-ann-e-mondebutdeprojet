using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class FPCSupport : MonoBehaviour
{
    public GameObject playerCam;
    private UnityStandardAssets.ImageEffects.Blur blur;//pour rendre la page flou
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsComp;//pour controler les mouvements du joueur.Le rendre fixe.

    public Text infoDisplay;//acceder au composant d'info
    private bool infoCoroutineIsRunning = false;

    public float pickupRange = 3.0f;
    private GameObject objectInteract;


    [Header("Button List")] //titre pour les boutons.
    public string InventoryButton;
    public string InteractButton;
    [Header("Tag List")]
    public string ItemTag = "item";
    public string doActionTag = "DoAction";

    [Header("Crosshair's data")]
    public string layerInteract = "Interact";
    public GameObject crosshairDisplay;
    public int defaultSize = 30;
    public int specialSize = 50;
    public Sprite defaulTexture;//texture par defaut.
    public Sprite interactTexture;// texture d'interaction.
    private bool useSpecialTexture = false;


    [Header("inventory's Data")]
    public GameObject inventoryCanvas;//acceder à l'inventaire en lui meme.regroupe les elements de l'inventaire
    [HideInInspector] public bool inventoryOn = false;//variable caché dans l'inspecteur.pas utile priver. il peut etre remplace par private. cette variable permet de savoir si l'inventaire est activé ou non grace à une fonction.
    public Transform itemPrefab;
    public Transform inventorySlots;
    public int slotCount = 16;
    // Start is called before the first frame update
    private bool holdingItem = false;
    private GameObject itemObjectold;
    private string itemTypeold;
    private string itemIDold;
    private bool itemReutilisableold;
    public GameObject InventoryItemOptions;

    [Header("dialogue")]
    private Dialogue dialogue;
    public GameObject DialogueBox;

    public string Fin;
    public bool finito=false;
    

    void Start()
    {


        if (playerCam == null)
        {
            playerCam = GameObject.FindWithTag("MainCamera");
        }//securiter pour la camera

        blur = playerCam.GetComponent<UnityStandardAssets.ImageEffects.Blur>();//acceder au script blur
        blur.enabled = false;//blur bien désactivé au démarage.Juste un composant on utilise enabled.
        if (infoDisplay == null)
        {
            infoDisplay = GameObject.Find("infoDisplay").GetComponent<Text>();//crosshairdisplay pas renseigner.

        }
        infoDisplay.text = "";
        fpsComp = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();//acceder aux composants de fps.
        if (crosshairDisplay == null)
        {
            crosshairDisplay = GameObject.Find("Crosshair");//crosshairdisplay pas renseigner.

        }
        crosshairDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(defaultSize, defaultSize);

        if (inventoryCanvas == null)
        {
            inventoryCanvas = GameObject.Find("Inventory Panel");
        }//inventory canvas bien renseigné.
        inventoryCanvas.SetActive(false);//quand c'est un game object on utilise setActive.
        if (InventoryItemOptions == null)
        {
            InventoryItemOptions = GameObject.Find("Inventory_Items_Options");
        }
        InventoryItemOptions.SetActive(false);


        if (DialogueBox == null)
        {
            DialogueBox = GameObject.Find("DialogueBox");
        }
        DialogueBox.SetActive(true);

        

        
    }
   

//public void OnTriggerEnter(Collider other)
//{

//   JeQuitteLeJeu();
//}

public void JeQuitteLeJeu()
    {

        SceneManager.LoadScene(Fin);
    }

    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(InventoryButton))
        {
            ShowOnHideInventory(); //fonction appeller à chaque fois qu'on appuis sur le bouton et qui va simplifier le travaille de recherche si l'inventaire est activé.
            if (holdingItem)
            {
                StopoldingItem();
            }
        }
        //controler si on appuye sur inventoryButton.
        
        if (Input.GetButtonDown(InteractButton)&& !inventoryOn)
        {
         
            if (infoCoroutineIsRunning)
            {
                infoDisplay.text = "";
                infoCoroutineIsRunning = false;
            }
        
           
            if (holdingItem)
            {
                TryToUse();

            }
            else
            {
                TryToInteract();//lance la fonction TryToInteract.
                
            }
            

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            
        }

       



   
        if (!useSpecialTexture)
        {
            Ray ray = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));//lancer un rayon devant le joueur de la distance pickupRnage si il y a pas un item devant lui.
            RaycastHit hit;//hit point d'impact du rayon avec un colider

            if (Physics.Raycast(ray, out hit, pickupRange))//est ce que le rayon touche un collider si oui on a le 1er if
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(layerInteract)) //2 eme if va verifier si le collider qu'on a toucher appartient à un objet avec lequel on peut interagir = item,doaction,.....
                {
                    crosshairDisplay.GetComponent<Image>().sprite = interactTexture; //cela va aller changer la sorce d'image dans l'inspector.
                }
                else//si on ne peut interagir avec l'objet en question.(un mur par exemple).
                {
                    crosshairDisplay.GetComponent<Image>().sprite = defaulTexture;
                }
            }
            else//si le rayon ne touche rien par rapport au collider.
            {
                crosshairDisplay.GetComponent<Image>().sprite = defaulTexture;
            }
        }
    }

    

    void TryToInteract()
    {
        Ray ray = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));//lancer un rayon devant le joueur de la distance pickupRnage si il y a pas un item devant lui.
        RaycastHit hit;//hit point d'impact du rayon avec un colider

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            objectInteract = hit.collider.gameObject;
            if (objectInteract.tag == ItemTag)
            {
              
                //pick up 
                //verifier si l'inventaire est complet
                if (inventorySlots.childCount == slotCount)
                {
                    //Debug.Log("L'inventaire est complet!");
                    infoDisplay.text = "Inventaire est complet!";
                    StartCoroutine(WaitAndEraseInfo());
                    
                }

                //hoever
                else
                {
                    //faire disparaitre  l'objet 
                    objectInteract.SetActive(false);
                    
                    //integrer notre nouvelle item dans l'inventaire.
                    Transform newItem;
                    newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity) as Transform;//si on laisse comme ça le new item va apparaitre à une possition aléatoire.
                    newItem.SetParent(inventorySlots, false);//si on lui donne un parent est ce que l'objet va garder ça possition oui = true le parent va réévaluaer la position pour etre ajuster au parent.

                    //telecharger les informations de slot à l'inventaire.
                    ItemSlots itemInventory = newItem.GetComponent<ItemSlots>();
                    ItemVariables itemScene = objectInteract.GetComponent<ItemVariables>();
                    itemInventory.itemType=itemScene.itemType;
                    itemInventory.itemID=itemScene.itemID;
                    itemInventory.itemSprite= itemScene.itemSprite;
                    itemInventory.itemDescription=itemScene.itemDescription;
                    itemInventory.itemReutilisable=itemScene.itemReutilisable;
                }

            }
            if (objectInteract.tag == doActionTag)
            {
                if(!objectInteract.GetComponent<DoAction>().needItem)
                {
                    objectInteract.GetComponent<DoAction>().DoActionNow();
                    
                }
                else
                {
                    //Debug.Log("Vous ne pouvez pas faire ça sans item!");
                    infoDisplay.text = objectInteract.GetComponent<DoAction>().textwithoutItem;
                    StartCoroutine(WaitAndEraseInfo());
                }
               
            }  
        }
    }
    void TryToUse()
    {
        Ray ray = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));//lancer un rayon devant le joueur de la distance pickupRnage si il y a pas un item devant lui.
        RaycastHit hit;//hit point d'impact du rayon avec un colider

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            objectInteract = hit.collider.gameObject;
            if (objectInteract.tag == doActionTag && objectInteract.GetComponent<DoAction>().needItem)
            {
                // bon item alors on fait l'action(if right item do it)
                if (itemTypeold == objectInteract.GetComponent<DoAction>().itemType)
                {
                    if (itemIDold == objectInteract.GetComponent<DoAction>().itemID || objectInteract.GetComponent<DoAction>().itemID == null)
                    {
                        // bon item(right item)
                         objectInteract.GetComponent<DoAction>().DoActionNow();
                        if (!itemReutilisableold)
                        {
                            Destroy(itemObjectold);
                        }  
                    }
                    else
                    {
                        //Debug.Log("Ce n'est pas le bon identifiant de l'objet!");//pas verifier : pas bon identifiant de l'object
                        infoDisplay.text = objectInteract.GetComponent<DoAction>().textwithoutRightIDItem;
                        StartCoroutine(WaitAndEraseInfo());
                    }
                }
                else
                {
                    //Debug.Log("Ce n'est pas le bon type d'objet!");//si on n'as pas le bon item utiliser alors erreur. Creer un lien entre itemslots et fpcSupport(if wrong item error)
                    infoDisplay.text = "Vous ne pouvez pas utiliser cette objet ici!";
                    StartCoroutine(WaitAndEraseInfo());
                }
            }    
        }
        StopoldingItem();
    }
    public void  YouAreoldingItem(GameObject itemObject,string itemType,string itemID,Sprite itemSprite,bool itemReutilisable)//type,ID,gameobject en lui mê qui est l'item.
    {
        holdingItem = true;//pourquoi si l'item est utilisé il faudra le detruire
        //quitter automatiquement l'inventaire.
        ShowOnHideInventory();
        //stockages des données importées.
        itemObjectold = itemObject;
        itemTypeold = itemType;
        itemIDold = itemID;
        itemReutilisableold = itemReutilisable;
        //modification du curseur prendre la  forme de cle marteau ou briquet
        useSpecialTexture = true;
        crosshairDisplay.GetComponent<Image>().sprite = itemSprite;
        crosshairDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(specialSize, specialSize);
    }

    void StopoldingItem()
    {
        //vous utilisé l'objet pour qu'il ne soit plus entre vos mains(l'objet reviens dans l'inventaire ou il est detruit).
        holdingItem = false;
        //et on reset le riticule.
        useSpecialTexture = false;
        crosshairDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(defaultSize, defaultSize);
    }
    public void ShowOnHideInventory()
    {
        //gere l'inventaire et le joueur.
        inventoryCanvas.SetActive(!inventoryOn);
        DialogueBox.SetActive(inventoryOn);
        
        blur.enabled = !inventoryOn;
        fpsComp.enabled = inventoryOn; //fonctionne de façon désinchroniser car il est true au debut et il deveindra false après.
        // gere les options de l'inventaire je veux que quands l'inventaire s'eteind les options de l'inventaire aussi et pas l'inverse.
        
        if (inventoryOn)
        {
            InventoryItemOptions.SetActive(false);
            
        }
        //gere le curseur.
        Cursor.visible = !inventoryOn;
        if (inventoryOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else { Cursor.lockState = CursorLockMode.None; }
        crosshairDisplay.SetActive(inventoryOn);

       


        inventoryOn = !inventoryOn; //avec le ! cela veut dire que si inventoryOn est = a false alors il deviendra true et inversement.


    }
   
    public void ActiviteItemOptions(GameObject itemSelected)
    {
        InventoryItemOptions.SetActive(true);
        itemObjectold = itemSelected;
        //Acceder au(x) button(s)
        Transform buttonOptions = InventoryItemOptions.transform.GetChild(0);
        //placement des bouttons(options)
        buttonOptions.position = Input.mousePosition;
    }
    public void DisableItemOptions()
    {
        InventoryItemOptions.SetActive(false);
    }
    public void DropItem()
    {
        Destroy(itemObjectold);
        InventoryItemOptions.SetActive(false);
    }
    IEnumerator WaitAndEraseInfo()
    {
        infoCoroutineIsRunning = true;
        //attendre une certain temps 
        yield return new WaitForSeconds(5);
        //apres avoir attendue 
        if (infoCoroutineIsRunning)
        {
            infoDisplay.text = "";
            infoCoroutineIsRunning = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "fin")
        {
            Debug.Log("Fin");
            JeQuitteLeJeu();

        }
        
    }


}