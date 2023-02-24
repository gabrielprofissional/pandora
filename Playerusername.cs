using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class Playerusername : MonoBehaviour
{
    [SerializeField] InputField usernameinput;


    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {

            
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            usernameinput.text = "Player" + Random.Range(0, 10000).ToString("0000");
            OnUsernameInputValueChange();
        }

    }


    public void OnUsernameInputValueChange()
    {
        PhotonNetwork.NickName = usernameinput.text;
        PlayerPrefs.SetString("username", usernameinput.text);
    }
}
