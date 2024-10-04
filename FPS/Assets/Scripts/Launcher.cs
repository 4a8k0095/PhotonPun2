using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace MultiplayerFPS
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [Tooltip("Play 按鈕")]
        [SerializeField] private GameObject controlPanel;

        [Tooltip("Loading 字串")]
        [SerializeField] private GameObject progressLabel;

        [Header("PlayerLimit")]
        [Tooltip("當遊戲室玩家人數已滿額, 新玩家只能新開遊戲室來進行遊戲.")]
        [SerializeField] private byte maxPlayersPerRoom = 4;

        // 遊戲版本的編碼, 可讓 Photon Server 做同款遊戲不同版本的區隔.
        private string gameVersion = "1";

        private bool isConnecting;

        private void Awake()
        {
            // 確保所有連線的玩家均載入相同的遊戲場景
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        // 與 Photon Cloud 連線 由 Connect Button 控制
        public void Connect()
        {
            isConnecting = true;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // 檢查是否與 Photon Cloud 連線
            if (PhotonNetwork.IsConnected)
            {
                // 已連線, 嘗試隨機加入一個遊戲室
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // 未連線, 嘗試與 Photon Cloud 連線
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        // 連線成功時執行
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN 呼叫 OnConnectedToMaster(), 已連上 Photon Cloud.");

            // 確認已連上 Photon Cloud
            // 隨機加入一個遊戲室
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        // 連線失敗時執行
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN 呼叫 OnDisconnected() {0}.", cause);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        // 隨機加入 遊戲房間 失敗時執行
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN 呼叫 OnJoinRandomFailed(), 隨機加入遊戲室失敗.");

            // 隨機加入遊戲室失敗. 可能原因是 1. 沒有遊戲室 或 2. 有的都滿了.    
            // 自己開一個遊戲室.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        // 成功加入 遊戲房間 時執行
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("我是創建遊戲室的玩家");
                PhotonNetwork.LoadLevel("Main");
            }
        }
    }
}