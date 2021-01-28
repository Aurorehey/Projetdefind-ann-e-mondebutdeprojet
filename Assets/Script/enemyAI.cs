using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour{

    public Transform Target;
    public UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    agent.destination = Target.position;
        
        
    }
}
