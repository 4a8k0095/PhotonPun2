using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace MultiplayerFPS
{
    public class PlayerSetup : MonoBehaviourPun
    {
        [SerializeField] private List<Behaviour> componentsToDisable;

        [SerializeField] private GameObject playerUIPrefab;
        private GameObject playerUIInstance;
        public GameObject PlayerUIInstance
        {
            get { return playerUIInstance; }
        }

        [SerializeField] private string remoteLayerName = "RemotePlayer";

        private void Start()
        {
            RegisterPlayer();

            if (!photonView.IsMine)
            {
                DisableComponents();
                AssignRemoteLayer();
            }
            else
            {
                playerUIInstance = Instantiate(playerUIPrefab);
                PlayerUI playerUI = playerUIInstance.GetComponent<PlayerUI>();
                playerUI.SetPlayer(GetComponent<Player>());

                GetComponent<Player>().Setup();
            }
        }

        private void OnDisable()
        {
            GameMaster.Instace.UnRegisterPlayer(transform.name);
        }

        private void RegisterPlayer()
        {
            string _netID = photonView.ViewID.ToString();
            Player _player = GetComponent<Player>();
            GameMaster.Instace.RegisterPlayer(_netID, _player);
        }

        private void DisableComponents()
        {
            for (int i = 0; i < componentsToDisable.Count; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }

        private void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        }
    }
}