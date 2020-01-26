using UnityEngine;

public class ArrowDataController : MonoBehaviour
{
	//==========================================================================//
	//	定義																		//
	//==========================================================================//

	//--------------------------------------//
	//	外部定数定義							//
	//--------------------------------------//

	//--------------------------------------//
	//	内部定数定義							//
	//--------------------------------------//
	private const int VALUE_DEFAULT_ARROWS = 10;	//ゲームスタート時の矢の本数
	private const int VALUE_GAME_OVER_ARROWS = 0;	//ゲームオーバ判定用
	private const int VALUE_GIFT_ARROWS = 10;		//リワードで与える矢の本数

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	//Driver
	private UIDriver UIDriver;

	//Controller
	private ArrowController ArrowController;
	private TimerController TimerController;

	private int ValArrows;//矢の本数

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartArrowDataController()
	{
		UIDriver = GameObject.Find("Driver").GetComponent<UIDriver>();

		ArrowController = GameObject.Find("ArrowController").GetComponent<ArrowController>();
		TimerController = GameObject.Find("TimerController").GetComponent<TimerController>();

		ValArrows = VALUE_DEFAULT_ARROWS;
	}

	//--------------------------------------//
	//	矢本数減算処理						//
	//--------------------------------------//
	public void SubstractArrows(int numSubstract)
	{
		ValArrows -= numSubstract;	//デクリメント

		if(ValArrows <= VALUE_GAME_OVER_ARROWS)	//矢の本数が0になったら
		{
			ValArrows = 0;//矢の本数を0で固定(ブロック衝突時にマイナスの値になってしまうから)
			ArrowController.ClearFlagPermitShoot();//矢の射出禁止
			TimerController.SetFlgTimerUntilGameOverOn();//ゲームオーバーまでディレイタイマオン
		}

		UIDriver.SetTextArrow(ValArrows);//矢の残り本数テキストの更新
	}

	//--------------------------------------//
	//	矢本数加算処理						//
	//--------------------------------------//
	public void AddArrows(int numAdd)
	{
		ValArrows += numAdd;				//インクリメント
		UIDriver.SetTextArrow(ValArrows);	//矢の残り本数テキストの更新
	}

	//--------------------------------------//
	//	矢本数回復処理(リワード)				//
	//--------------------------------------//
	public void GiftArrows()
	{
		AddArrows(VALUE_GIFT_ARROWS);
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
