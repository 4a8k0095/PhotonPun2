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

    // �u�|�b�ۤv���a�ݰ���
    public override void OnJoinedRoom()
    {
        Debug.Log("���\�[�J�C�� !!");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity);
    }

    // �����a�[�J�� ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"�s���a�[�J !!");
    }

    // �����a���}�� ����
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("�����a���} !!");
    }

    // �ѷs�� MasterClient ����
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("�ХD�w���}�A�����s�ХD !!");
    }

    // ���a���}�C���Ǯ�, ��L�a�^��C�����J�f
    public override void OnLeftRoom()
    {
        Debug.Log("�z�w�^��j�U !!");
        SceneManager.LoadScene(0);
    }

    // ���}�C������ �� Leave Button ����
    public void LeaveRoom()
    {
        Debug.Log("�z�w���}�C���ж� !!");
        PhotonNetwork.LeaveRoom();
    }
}
