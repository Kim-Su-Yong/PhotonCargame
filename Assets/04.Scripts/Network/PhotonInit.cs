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
    {               //포톤 클라우드에 접속 버전별로
        PhotonNetwork.ConnectUsingSettings(Version);
        roomName.text = "Room" + Random.Range(0, 999).ToString("000");
    }
    void OnJoinedLobby()
    {
        print("로비접속!");
        //PhotonNetwork.JoinRandomRoom(); //아무방이나 접속
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
    void OnPhotonRandomJoinFailed() //방접속 실패시 호출되는 콜백 함수
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
        Debug.Log($"룸접속");
        //CreateCar();
        StartCoroutine(LoadPlayScene());
    }
    void OnPhotonCreateRoomFailed(object[] codeAndMsg) //방접속 실패시 호출되는 콜백 함수
    {
        Debug.Log("Create RoomFailed=" + codeAndMsg[1]);
    }
    IEnumerator LoadPlayScene()
    {   //씬이동시 포톤네트워크 서버로부터 메시지 수신 중단
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
        //1. 방만들기 버튼 입력시 입력란에 입력했을 경우와 하지 않을 때를 구분해서 코딩
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
        //좌측 상단에 포톤네트워크 접속 정보를 표시
    }
    void CreateCar()
    {
        float randomPos = Random.Range(257, 357f);
        Quaternion rot = Quaternion.Euler(-1.3f, 256f, 4.3f);
        PhotonNetwork.Instantiate("PlayerCar_02", new Vector3(randomPos, 7.4f, 181f), rot, 0);
    }
}
