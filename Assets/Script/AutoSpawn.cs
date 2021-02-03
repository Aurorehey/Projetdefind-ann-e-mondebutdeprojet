using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawn : MonoBehaviour
{
    public Transform Player;
    public int DistanceDeSpawn;
    public GameObject PrefabToSpawn;
    public float SpawnRate = 3f;
    public int MaxSpawn = 3;
    public bool ReSpawn;
    

    private float NextSpawn;
    private int Nr;
    private float Distance;
    private int Nb;

    
    void Update()
    {

        Distance = Vector3.Distance(transform.position, Player.position);


        if (Distance < DistanceDeSpawn&&Nr<MaxSpawn)
        {
            if(Time.time> NextSpawn)
            {
                NextSpawn = Time.time + SpawnRate;
                GameObject Go = Instantiate(PrefabToSpawn, transform.position, Quaternion.identity)as GameObject;
                Go.name = "E" + this.name;
                Nr++;
            }
            
        }
        if (ReSpawn)
        {
            Nb = 0;
            foreach (GameObject Enn in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (Enn.name == "E" + this.name)
                {
                    Nb++;
                }
            }
            if (Nb < MaxSpawn)
            {
                Nr = Nb;
            }

        }
    }
}
