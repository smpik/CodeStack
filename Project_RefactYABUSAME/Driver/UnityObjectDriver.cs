﻿using UnityEngine;

public class UnityObjectDriver : MonoBehaviour
{
	//==========================================================================//
	//	定義																	//
	//==========================================================================//

	//--------------------------------------//
	//	外部定数定義						//
	//--------------------------------------//
	//引数に設定してswitch文などで切り分けるときに用いるため
	public enum NAME_UNITY_OBJECT//アルファベット順にすること
	{
		BLOCK,
		PLAYER,
	}

	//--------------------------------------//
	//	内部定数定義						//
	//--------------------------------------//
	//Hierarchy上のオブジェクト名を設定(Hierarchy上のオブジェクト名を設定変更したらこっちも要修正)
	private const string BlockObjectName = "Block";
	private const string PlayerObjectName = "Player";

	//--------------------------------------//
	//	内部変数定義						//
	//--------------------------------------//
	//Block
	private GameObject ObjBlock;
	private Vector3 PosBlock;

	//Player
	private GameObject ObjPlayer;
	private Rigidbody RbPlayer;
	private Vector3 PosPlayer;

	//==========================================================================//
	//	関数																	//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartUnityObjectDriver()
	{
		startAtach();//オブジェクトのアタッチ
	}

	private void startAtach()
	{
		atachGameObject();	//GameObjectのアタッチ
		atachRigidbody();	//Rigidbodyのアタッチ
		atachPos();			//transform.positionのアタッチ
	}

	//--------------------------------------//
	//	アタッチ処理						//
	//--------------------------------------//
	private void atachGameObject()
	{
		ObjBlock = GameObject.Find(BlockObjectName);
		ObjPlayer = GameObject.Find(PlayerObjectName);
	}

	private void atachRigidbody()
	{
		RbPlayer = ObjPlayer.GetComponent<Rigidbody>();
	}

	private void atachPos()
	{
		PosBlock = ObjBlock.transform.position;
		PosPlayer = ObjPlayer.transform.position;
	}

	//--------------------------------------//
	//	Update処理							//
	//--------------------------------------//
	public void UpdateUnityObjectDriver()
	{
		updatePos();//transform.positionの更新
	}

	private void updatePos()
	{
		atachPos();
	}

	//--------------------------------------//
	//	オブジェクト存在確認処理			//
	//--------------------------------------//
	public bool IsExistHierarchy(NAME_UNITY_OBJECT objectConfirmTarget)
	{
		bool ret = false;

		switch (objectConfirmTarget)//アルファベット順にすること
		{
			case NAME_UNITY_OBJECT.BLOCK:
				ret = GameObject.Find(BlockObjectName);
				break;
			case NAME_UNITY_OBJECT.PLAYER:
				ret = GameObject.Find(PlayerObjectName);
				break;
			default:
				ret = false;
		}

		return ret;
	}

	//--------------------------------------//
	//	GameObject名設定処理				//
	//--------------------------------------//
	public void ChangeGameObjectName(GameObject objectChangeTarget, string newName)
	{
		if (GameObject.Find(objectChangeTarget.name))
		{
			objectChangeTarget.name = newName;
		}
		else
		{
			Debug.Log("Object you try to change name don't exist in hierarchy.(from UnityObjectDriver.cs/ChangeGameObjectName())");
		}
	}

	//--------------------------------------//
	//	渡し処理							//
	//--------------------------------------//

	//Pos渡し
	public Vector3 GetPos(NAME_UNITY_OBJECT objectGetPosTarget)
	{
		Vector3 ret = new Vector3(0, 0, 0);

		switch (objectGetPosTarget)
		{
			case NAME_UNITY_OBJECT.BLOCK:
				ret = PosBlock;
				break;
			case NAME_UNITY_OBJECT.PLAYER:
				ret = PosPlayer;
				break;
			default:
				ret = new Vector3(0, 0, 0);
		}

		return ret;
	}

	//Rb渡し
	public Rigidbody GetRb(NAME_UNITY_OBJECT objectGetRbTarget)
	{
		Rigidbody ret = null;

		switch (objectGetRbTarget)
		{
			case NAME_UNITY_OBJECT.PLAYER:
				ret = RbPlayer;
				break;
			default:
				ret = null;
		}

		return ret;
	}

}
