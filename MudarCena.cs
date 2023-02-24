using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{

    public string nomedacena;

    public void ChangeS()
    {
        SceneManager.LoadSceneAsync(nomedacena);
    }
    public void Sair()
    {
        Application.Quit();
        Debug.Log("IS EXIT");

    }

}
