using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PhotonInit : MonoBehaviour
{
    [SerializeField] string Version = "V_car.1.0";
    [SerializeField] InputField userID;
    [SerializeField] InputField roomName;
    private void Awake()
    {               //���� Ŭ���忡 ���� ��������
        PhotonNetwork.ConnectUsingSettings(Version);
        roomName.text = "Room" + Random.Range(0, 999).ToString("000");
    }
    void OnJoinedLobby()
    {
        print("�κ�����!");
        //PhotonNetwork.JoinRandomRoom(); //�ƹ����̳� ����
        userID.text = GetUserID();
    }
    string GetUserID()
    {
        string userID = PlayerPrefs.GetString("USER_");
        if(string.IsNullOrEmpty(userID))
        {
            userID = "USER_" + Random.Range(0, 999).ToString("000");
        }
        return userID;
    }
    void OnPhotonRandomJoinFailed() //������ ���н� ȣ��Ǵ� �ݹ� �Լ�
    {
        print("No Room!!");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.CreateRoom("RacingCarGame", roomOptions, TypedLobby.Default);
    }
    void OnJoinedRoom()
    {
        Debug.Log($"������");
        //CreateCar();
        StartCoroutine(LoadPlayScene());
    }
    void OnPhotonCreateRoomFailed(object[] codeAndMsg) //������ ���н� ȣ��Ǵ� �ݹ� �Լ�
    {
        Debug.Log("Create RoomFailed=" + codeAndMsg[1]);
    }
    IEnumerator LoadPlayScene()
    {   //���̵��� �����Ʈ��ũ �����κ��� �޽��� ���� �ߴ�
        PhotonNetwork.isMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("RacePlay");
        yield return ao;
    }
    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.player.NickName = userID.text;
        PlayerPrefs.SetString("USER_", userID.text);
        PhotonNetwork.JoinRandomRoom();
    }
    public void OnClickCreateRoom()
    {
        //1. �游��� ��ư �Է½� �Է¶��� �Է����� ���� ���� ���� ���� �����ؼ� �ڵ�
        string _roomName = roomName.text;
        if(string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "Room " + Random.Range(0, 999).ToString("000");

        }
        PhotonNetwork.player.NickName = userID.text;
        PlayerPrefs.SetString("USER_", userID.text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }
    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        //���� ��ܿ� �����Ʈ��ũ ���� ������ ǥ��
    }
    void CreateCar()
    {
        float randomPos = Random.Range(257, 357f);
        Quaternion rot = Quaternion.Euler(-1.3f, 256f, 4.3f);
        PhotonNetwork.Instantiate("PlayerCar_02", new Vector3(randomPos, 7.4f, 181f), rot, 0);
    }
}
