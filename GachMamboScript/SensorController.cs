using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
	private Vector3 PosStartRay;
	private Vector3 PosEndRay;
	private const float LengthRay = 0.025f;
	private const string SENSORED_COIN = "Coin(Sensored)";

    // Start is called before the first frame update
    void Start()
    {
		PosStartRay = GameObject.Find("Sensor").transform.position;
		PosEndRay = new Vector3(PosStartRay.x, PosStartRay.y, PosStartRay.z + 1);//z軸方向を向けばなんでもいい
    }

    // Update is called once per frame
    void Update()
    {
		/* Sensorの中心からz軸方向にLengthRayだけRayを飛ばし、衝突したオブジェクトの情報をhittedObjInfoに格納する	*/
		RaycastHit hittedObjInfo;
		Debug.DrawRay(PosStartRay, PosEndRay, Color.red, LengthRay);
		if(Physics.Raycast(PosStartRay,PosEndRay, out hittedObjInfo, LengthRay))//Rayはオブジェクトの中心から出る
		{
			/* 未検出のコインなら処理する(1枚のコインが通過するまでに最大2回検知してしまうため検出したかを区別する)	*/
			if(hittedObjInfo.collider.gameObject.name != SENSORED_COIN)
			{
				hittedObjInfo.collider.gameObject.name = SENSORED_COIN;//検出済みコインに名前を変更
				Debug.Log("ok");
			}
		}
    }

	/*
	void OnTriggerEnter(Collider enteredObjectCollider)
	{
		Debug.Log(enteredObjectCollider.gameObject.name + "通過");
	}
	*/
	/*
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("ok");
	}
	*/
}
