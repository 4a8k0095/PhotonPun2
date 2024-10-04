using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerFPS
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster Instace;

        private const string PLAYER_ID_PREFIX = "Player ";

        private Dictionary<string, Player> players = new Dictionary<string, Player>();

        [SerializeField] private GameObject sceneCamera;

        private void Awake()
        {
            if (Instace == null)
                Instace = FindObjectOfType<GameMaster>();
            else
                Destroy(this.gameObject);
        }

        public void RegisterPlayer(string _netID, Player _player)
        {
            string _playerID = PLAYER_ID_PREFIX + _netID;
            players.Add(_playerID, _player);
            _player.transform.name = _playerID;
        }

        public void UnRegisterPlayer(string _playerID)
        {
            players.Remove(_playerID);
        }

        public Player GetPlayer(string _playerID)
        {
            return players[_playerID];
        }

        //private void OnGUI()
        //{
        //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        //    GUILayout.BeginVertical();

        //    foreach(string _playerID in players.Keys)
        //    {
        //        GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        //    }

        //    GUILayout.EndVertical();
        //    GUILayout.EndArea();
        //}

        public void SetSceneCameraActive(bool _isActive)
        {
            if (sceneCamera == null)
                return;

            sceneCamera.SetActive(_isActive);
        }
    }
}