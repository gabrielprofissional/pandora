using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instancia { get; private set; }
    public List<NewMoviment> Jogadores { get => _jogadores; private set => _jogadores = value; }
    [SerializeField] private string _localizacaoPrefab;
    public GameObject player;
    [SerializeField] private Transform[] _spawns;
    private int _jogadoresEmJogo = 0;
    private List<NewMoviment> _jogadores;
    
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        photonView.RPC("AdicionaJogador", RpcTarget.AllBuffered);
        _jogadores = new List<NewMoviment>();
    }

    [PunRPC]
    private void AdicionaJogador()
    {
        _jogadoresEmJogo++;
        if (_jogadoresEmJogo == PhotonNetwork.PlayerList.Length)
        {
            CriaJogador();
        }
    }
    [PunRPC]
    private void CriaJogador()
    {
        player = PhotonNetwork.Instantiate(_localizacaoPrefab, _spawns[Random.Range(0, _spawns.Length)].position, Quaternion.identity, 0, new object[] { photonView.ViewID });
        var jogador = player.GetComponent<NewMoviment>();

        jogador.photonView.RPC("inicializaPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(player);
        CriaJogador();
    }
}