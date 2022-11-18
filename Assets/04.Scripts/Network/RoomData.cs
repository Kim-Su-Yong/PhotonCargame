using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomData : MonoBehaviour
{
    public string roomName = "";
    public int connectPlayer = 0;
    public int maxPlayers = 0;
    public Text textRoomName;
    public Text textConnectInfo;

    public void DisplayRoomData()
    {
        textRoomName.text = roomName;
        textConnectInfo.text = "(" + connectPlayer.ToString() + "/" + maxPlayers.ToString() + ")";
    }
}
