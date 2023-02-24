using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;


public class Healthbarmob : MonoBehaviourPunCallbacks, IDamageable
{


    [SerializeField] Image healthbarImage;
    [SerializeField] const float maxHealth = 100f;
    [SerializeField] float currentHealth = maxHealth;
    public GameObject mob; 




    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            DieMob();
        }
    }
    void RPC_TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            DieMob();
        }
    }

    void DieMob()
    {
        Destroy(mob);
    } 
}
