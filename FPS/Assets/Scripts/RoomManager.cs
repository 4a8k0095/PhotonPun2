using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity);
        }
    }

    // 只會在自己本地端執行
    public override void OnJoinedRoom()
    {
        Debug.Log("成功加入遊戲 !!");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity);
    }

    // 有玩家加入時 執行
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"新玩家加入 !!");
    }

    // 有玩家離開時 執行
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("有玩家離開 !!");
    }

    // 由新的 MasterClient 執行
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("房主已離開，指派新房主 !!");
    }

    // 玩家離開遊戲室時, 把他帶回到遊戲場入口
    public override void OnLeftRoom()
    {
        Debug.Log("您已回到大廳 !!");
        SceneManager.LoadScene(0);
    }

    // 離開遊戲場景 由 Leave Button 控制
    public void LeaveRoom()
    {
        Debug.Log("您已離開遊戲房間 !!");
        PhotonNetwork.LeaveRoom();
    }
}
