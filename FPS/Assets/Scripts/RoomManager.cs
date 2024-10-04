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
        // �P�w PhotonNetwork.InRoom �O�_���� true ���ηN�G
        // �]���ХD�O�ϥ� PhotonNetwork.CreateRoom ���H
        // �ҥH�u���ХD�b�@�}�l InRoom �N���� true
        // ���ĤG�쪱�a�[�J�ɡA�]���ĤG�쪱�a���� InRoom != true
        // ��������� OnJoinedRoom �� InRoom �~�|���� true
        // �ҥH�Ĥ@�쪱�a�����a�ݨõL�k���ͲĤG�쪱�a������
        // �ӲĤG�쪱�a������ͲĤ@�쪱�a������
        // �]���Ĥ@�쪱�a�� InRoom = true
        // �]������[�J�����a�����b OnJoinedRoom �o�� Callback function
        // �ϥ� PhotonNetwork.Instantiate
        // �p���@�ӡA�e�������a�~��b�ۤv�����a�ݥ��T���ͷs���a������
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Start !!");
            // �b�ۤv�����a�ݳЫت���
            // �æP�B��L InRoom = true (�b�ж���)�����a�����a��
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity);
        }
    }

    // �u�|�b�ۤv���a�ݰ���
    public override void OnJoinedRoom()
    {
        Debug.Log("���\�[�J�C�� !!");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity, 0);
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
