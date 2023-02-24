using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class EnemyBE : MonoBehaviourPunCallbacks
{
    public Transform player;
    public NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void move()
    {
        agent.SetDestination(player.transform.position);
    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        move();
    }


    private void OnCollisionEnter(Collision other)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


}


