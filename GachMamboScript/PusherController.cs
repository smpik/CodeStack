﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherController : MonoBehaviour
{
	private Rigidbody RbPusher; //PusherのRigidbody

	private Vector3 PosStartPusher;//Pusherの初期位置(オフセットに用いる)
	private Vector3 PosPusher;  //Pusherの現在地

	private const float PUSHER_AMPLITUDE = 0.33f;//Pusherのピストン運動の振幅

    // Start is called before the first frame update
    void Start()
    {
		RbPusher = GameObject.Find("Pusher").GetComponent<Rigidbody>(); //PusherのRigidbodyを取得
		PosStartPusher = GameObject.Find("Pusher").transform.position;//Pusherの初期値を取得
		PosPusher = PosStartPusher;//Pusherの現在地を初期化
    }

    // Update is called once per frame
    void Update()
    {
		PosPusher = GameObject.Find("Pusher").transform.position;//Puserの現在地を取得

		//Pusherのz座標 = (振幅　* sin関数) + Pusherの初期位置のz座標
		PosPusher = new Vector3(PosPusher.x, PosPusher.y, (PUSHER_AMPLITUDE * Mathf.Sin(Time.time))+PosStartPusher.z );

		RbPusher.MovePosition(PosPusher);
	}
}
