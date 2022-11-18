using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayUserID : MonoBehaviour
{
    public Text userID;
    private PhotonView pv = null;
    void Start()
    {
        pv = PhotonView.Get(this);
        userID.text = pv.owner.NickName.ToString();
    }
}
