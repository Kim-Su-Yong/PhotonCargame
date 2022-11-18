using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int PlayerCount = 0;
    int playerClamp = 0;
    void Awake()
    {   //포톤 클라우드로 서버로부터 메시지 수신
        PhotonNetwork.isMessageQueueRunning = true;
        CreateCar();
    }

    void Update()
    {
        
    }
    void CreateCar()
    {
        //float randomPos = Random.Range(257, 287f);
        //Quaternion rot = Quaternion.Euler(-1.3f, 266f, 4.3f);
        //PhotonNetwork.Instantiate("PlayerCar_02", new Vector3(randomPos, 7.4f, 181f), rot, 0);

        Room myRoom = PhotonNetwork.room;
        playerClamp = myRoom.MaxPlayers;
        PlayerCount = myRoom.PlayerCount;

        Vector3[] SpawnCarPos = new Vector3[5];
        SpawnCarPos[0] = new Vector3(433f, 0.05f, 230f);
        SpawnCarPos[1] = new Vector3(438f, 0.05f, 230f);
        SpawnCarPos[2] = new Vector3(433f, 0.05f, 230f);
        SpawnCarPos[3] = new Vector3(450f, 0.05f, 230f);
        SpawnCarPos[4] = new Vector3(458f, 0.05f, 230f);
        Quaternion SpawnRot = Quaternion.Euler(-4f, -2f, -1.3f);


        if (PlayerCount == 1)
            PhotonNetwork.Instantiate("PlayerCar_02", SpawnCarPos[0], SpawnRot, 0);
        else if(PlayerCount == 2)
            PhotonNetwork.Instantiate("PlayerCar_02", SpawnCarPos[1], SpawnRot, 0);
        else if (PlayerCount == 3)
            PhotonNetwork.Instantiate("PlayerCar_02", SpawnCarPos[2], SpawnRot, 0);
        else if (PlayerCount == 4)
            PhotonNetwork.Instantiate("PlayerCar_02", SpawnCarPos[3], SpawnRot, 0);
        else if (PlayerCount == 5)
            PhotonNetwork.Instantiate("PlayerCar_02", SpawnCarPos[4], SpawnRot, 0);
    }
}