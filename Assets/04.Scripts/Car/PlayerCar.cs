using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCar : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] PhotonView PV = null;
    [Header("Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] BoxCollider boxCollider;
    [Header("WheelCollider")]
    [SerializeField] WheelCollider FRC;
    [SerializeField] WheelCollider FLC;
    [SerializeField] WheelCollider RRC;
    [SerializeField] WheelCollider RLC;
    [Header("WheelTransforms")]
    [SerializeField] Transform FRM;
    [SerializeField] Transform FLM;
    [SerializeField] Transform RRM;
    [SerializeField] Transform RLM;
    [Header("FrictionCurve")]
    WheelFrictionCurve wheelFLF;
    WheelFrictionCurve wheelFRF;
    [Header("MpoveOrRotate Key")]
    [SerializeField] float steer = 0f; //Input.GetAxos("Horizontal")
    [SerializeField] float brake = 0f; //브레이크 전진할때 브레이크는 후진키임 후진시 전진
    [SerializeField] float accel = 0f;
    [Header("WheelSpeed")]
    [SerializeField] float CurrentSpeed = 0f; //현재 스피드
    [SerializeField] float maxSteer = 40f; //앞바퀴 A,D 누를때 바퀴의 회전각
    [SerializeField] float maxTorgue = 400f;
    [SerializeField] float maxBrake = 3500f;
    [SerializeField] float motor = 0f; //양수면 전진 음수이면 후진
    [SerializeField] float braking = 0f; //브레이크가 전진할때 후진하는 키값이다. 후진할때 전진키가 브레이크 

    [Header("Bool")] [SerializeField] bool reverse = false;
    [SerializeField] float slipRate = 1f;
    [SerializeField] float Drift = 0.4f;

    [SerializeField] Text SpeedTXT;
    private Vector3 CurPos = Vector3.zero;
    private Quaternion CurRot = Quaternion.identity;
    void Awake()
    {
        PV = PhotonView.Get(this);
        PV.synchronization = ViewSynchronization.UnreliableOnChange;
        PV.ObservedComponents[0] = this;
        rb = GetComponent<Rigidbody>();
        if (PV.isMine)
        {
            Camera.main.GetComponent<FollowCam>().target = this.transform;
            rb.centerOfMass = new Vector3(0f, -0.5f, 0.0f);
            //무게중심 잡기
        }
        boxCollider = GetComponentInChildren<BoxCollider>();
        FRC = transform.GetChild(2).GetChild(3).GetComponent<WheelCollider>();
        FLC = transform.GetChild(2).GetChild(2).GetComponent<WheelCollider>();
        RRC = transform.GetChild(2).GetChild(1).GetComponent<WheelCollider>();
        RLC = transform.GetChild(2).GetChild(0).GetComponent<WheelCollider>();
        FRM = transform.GetChild(1).GetChild(3).GetComponent<Transform>();
        FLM = transform.GetChild(1).GetChild(2).GetComponent<Transform>();
        RRM = transform.GetChild(1).GetChild(1).GetComponent<Transform>();
        RLM = transform.GetChild(1).GetChild(0).GetComponent<Transform>();
        CurPos = transform.position;
        CurRot = transform.rotation;
    }
    //송수신 자신의 움직임은 송신 다른 네트워크 유저의 움직임은 수신한다.
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            CurPos = (Vector3)stream.ReceiveNext();
            CurRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Update()
    {
        if (PV.isMine)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                wheelFRF.stiffness = Drift;
                FRC.forwardFriction = wheelFRF;
                wheelFRF.stiffness = Drift;
                FRC.sidewaysFriction = wheelFRF;
                wheelFRF.stiffness = Drift;

                wheelFLF.stiffness = Drift;
                FLC.forwardFriction = wheelFLF;
                wheelFLF.stiffness = Drift;
                FLC.sidewaysFriction = wheelFLF;
                wheelFLF.stiffness = Drift;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                wheelFRF.stiffness = slipRate;
                FRC.forwardFriction = wheelFRF;
                wheelFRF.stiffness = slipRate;
                FRC.sidewaysFriction = wheelFRF;

                wheelFLF.stiffness = slipRate;
                FLC.forwardFriction = wheelFLF;
                wheelFLF.stiffness = slipRate;
                FLC.sidewaysFriction = wheelFLF;
            }
            CurrentSpeed = rb.velocity.sqrMagnitude;
            //두바뮈의 크기의 개체가 밀려나는 힘으로 속도와 방향을 결정
            steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);
            accel = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f);
            brake = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 0f);
            if (accel > 0)
            {
                StartCoroutine(ForwardCar());
            }
            if (brake > 0)
            {
                StartCoroutine(BackwardCar());
            }
            if (reverse == true)
            {
                motor = -1 * brake;
            }
            else
            {
                motor = accel;
                braking = brake;
            }
            //휠콜라이더의 마찰의 힘으로 움직이는 로직
            FLC.steerAngle = maxSteer * steer;
            FRC.steerAngle = maxSteer * steer;
            //뒷바퀴는 마찰의 힘으로 움직인다.
            RLC.motorTorque = motor * maxTorgue;
            RRC.motorTorque = motor * maxTorgue;
            //왼쪽 앞바퀴 오른쪽 앞바퀴는 모델링이 회전되게
            FLM.localEulerAngles = new Vector3(FLM.localEulerAngles.x, steer * maxSteer, FLM.localEulerAngles.z);
            FRM.localEulerAngles = new Vector3(FRM.localEulerAngles.x, steer * maxSteer, FRM.localEulerAngles.z);

            FRM.Rotate(FRC.rpm * Time.deltaTime, 0f, 0f);
            FLM.Rotate(FLC.rpm * Time.deltaTime, 0f, 0f);
            FLM.Rotate(RLC.rpm * Time.deltaTime, 0f, 0f);
            FRM.Rotate(RRC.rpm * Time.deltaTime, 0f, 0f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, CurPos, Time.deltaTime * 3.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, CurRot, Time.deltaTime * 3.0f);
        }
        SpeedMeter();
        PV.RPC("SpeedMeter", PhotonTargets.Others, null);
    }
    IEnumerator ForwardCar()
    {
        yield return new WaitForSeconds(0.01f);
        CurrentSpeed = 0f;
        if (accel > 0)
            reverse = false;
        else
            reverse = true;
    }
    IEnumerator BackwardCar()
    {
        yield return new WaitForSeconds(0.01f);
        CurrentSpeed = 0f;
        if (accel > 0)
            reverse = false;
        else
            reverse = true;
    }
    [PunRPC] //원격지 함수 호출
    void SpeedMeter()
    {
        int speedmeter = (int)CurrentSpeed / 36;
        SpeedTXT.text = "Speed : " + speedmeter;
    }
}