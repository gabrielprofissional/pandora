using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Ping : MonoBehaviour
{
    public TMP_Text ping;
 
    void Update()
    {
        ping.text = "Ping: " + PhotonNetwork.GetPing();
        
    }
}
