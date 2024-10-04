using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private MultiplayerFPS.PlayerWeapon primaryWeapon;

    private void Start()
    {
        // 判定 PhotonNetwork.InRoom 是否等於 true 的用意：
        // 因為房主是使用 PhotonNetwork.CreateRoom 的人
        // 所以只有房主在一開始 InRoom 就等於 true
        // 但第二位玩家加入時，因為第二位玩家此時 InRoom != true
        // 必須執行到 OnJoinedRoom 時 InRoom 才會等於 true
        // 所以第一位玩家的本地端並無法產生第二位玩家的角色
        // 而第二位玩家能夠產生第一位玩家的角色
        // 因為第一位玩家的 InRoom = true
        // 因此後續加入的玩家必須在 OnJoinedRoom 這個 Callback function
        // 使用 PhotonNetwork.Instantiate
        // 如此一來，前面的玩家才能在自己的本地端正確產生新玩家的角色
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Start !!");
            // 在自己的本地端創建物件
            // 並同步其他 InRoom = true (在房間內)的玩家的本地端
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity);
        }
    }

    // 只會在自己本地端執行
    public override void OnJoinedRoom()
    {
        Debug.Log("成功加入遊戲 !!");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity, 0);
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
