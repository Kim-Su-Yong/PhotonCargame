using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float distance = 4f;
    public float height = 3f;
    public float moveDamping = 10f;
    public float rotDamping = 20f;
    Transform tr;
    

  void Start()
    {
        tr = this.transform;
    }
    void LateUpdate()
    {
        Vector3 CamPos = target.position - (target.forward * distance)
            + (target.up * height);
        tr.position = Vector3.Slerp(tr.position, CamPos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotDamping);
        tr.LookAt(target.transform);
    }
}
