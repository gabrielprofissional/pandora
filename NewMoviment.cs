using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;


public class NewMoviment : MonoBehaviourPunCallbacks, IDamageable
{
    [Header("DEPENDENCIAS")]
    GameManager GameManager;
    PhotonView pv; 

    [Header("CD")]
    public float cooldownTime = 2;
    private float nextFireTime = 0;

    [Header("INTERFACES")]
    public GameObject PausePanel;
    public GameObject InventarioEE;
    private bool isPaused;

    [Header("HEALTH")]
    [SerializeField] Image healthbarImage;
    [SerializeField] float currentHealth = maxHealth;
    [SerializeField] GameObject ui;
    [SerializeField] const float maxHealth = 100f;
    private float bossDamage = 0.5f;

    [Header("ITENS")]
    [SerializeField] Item[] itens;
    int ItemIndex;
    int PreviousItemIndex = -1;

    private Player _photonPlayer;
    private int _id;
    public int Id { get => _id; set => _id = value; }
    public Player PhotonPlayer { get => _photonPlayer; set => _photonPlayer = value; }
    
    [PunRPC]
    public void Inicializa(Player player)
    {
        PhotonPlayer = player;
        Id = player.ActorNumber;
        GameManager.Instancia.Jogadores.Add(this);
    }

    //////////////////////////////////////////////////////////INICIA EM PRIMEIRO///////////////////////////////////////////////////////////////////

    [PunRPC]


    private void Awake()

    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pv = GetComponent<PhotonView>();
        GameManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<GameManager>();                          
    }



    //////////////////////////////////////////////////////////INICIA EM SEGUNDO///////////////////////////////////////////////////////////////////
    [PunRPC]

    private void Start()
    {
        
        if (photonView.IsMine)
        {
            EquipeItems(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
        }
    }


    //////////////////////////////////////////////////////////ATUALIZA��ES EFETIVADAS///////////////////////////////////////////////////////////////////

    [PunRPC]

    void Update()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipeItems(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (ItemIndex >= itens.Length - 1)
            {
                EquipeItems(0);
            }
            else
            {
                EquipeItems(ItemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (ItemIndex <= 0)
            {
                EquipeItems(itens.Length - 1);
            }
            else
            {
                EquipeItems(ItemIndex - 1);
            }
        }

        if (Time.time > nextFireTime)
        { 
            if (Input.GetMouseButtonDown(0))
            {
                itens[ItemIndex].Use();
                nextFireTime = Time.time + cooldownTime;
            }

        }
       


        if (Input.GetKeyDown(KeyCode.E))
        {
            Inventario();
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen();
        }
    }
 


    [PunRPC]


    void Inventario()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            InventarioEE.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            InventarioEE.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    [PunRPC]

    void PauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            PausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            PausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
    [PunRPC]
    void EquipeItems(int _index)
    {
        if (_index == PreviousItemIndex)
            return;

        ItemIndex = _index;

        itens[ItemIndex].itemGameObject.SetActive(true);

        if (PreviousItemIndex != -1)
        {
            itens[PreviousItemIndex].itemGameObject.SetActive(false);
        }

        PreviousItemIndex = ItemIndex;

        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", ItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            EquipeItems((int)changedProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage)
    {
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);

    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!pv.IsMine)
            return;

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            currentHealth -= bossDamage;

            healthbarImage.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
        {
            Die();
        }

        }
    }
    [PunRPC]   
    void Die()
    {
        GameManager.Die();
    }

}

