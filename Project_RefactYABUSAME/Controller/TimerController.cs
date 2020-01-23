using UnityEngine;

public class TimerController : MonoBehaviour
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
	private const float TIME_UNTIL_GAME_OVER = 1;
	private const float TIME_UNTIL_DAMAGE = 1;

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	//Controller
	private GameController GameController;

	//Timer
	private float TimerUntilGameOver;		//最後の矢を放ってからゲームオーバーするまで間をあけるため
	private float TimerUntilDamage;			//ブロックと当たっている間のタイマ

	//Flag
	private bool FlgTimerHittingBlockOn;
	private bool FlgTimerUntilGameOverOn;   //矢の本数がゼロになったらセットするフラグ

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartTimerContoroller()
	{
		GameController = GameObject.Find("GameController").GetComponent<GameController>();
		FlgTimerUntilGameOverOn = false;
		FlgTimerHittingBlockOn = false;
		TimerUntilGameOver = TIME_UNTIL_GAME_OVER;
		TimerHittingBlock = TIME_UNTIL_DAMAGE;
	}

	//--------------------------------------//
	//	Upadate処理							//
	//--------------------------------------//
	public void UpdateTimerController()
	{
		if (FlgTimerUntilGameOverOn)
		{
			if (TimerUntilGameOver > 0)//少し待つ
			{
				TimerUntilGameOver -= Time.deltaTime;
			}
			else
			{
				GameController.GameOver();//ゲームオーバー処理をコール
			}
		}

		if (FlgTimerHittingBlockOn)
		{
			if (TimerHittingBlock > 0)
			{
				TimerHittingBlock -= Time.deltaTime;
			}
			else
			{
				//ダメージを与える
				TimerHittingBlock = TIME_HITTING_BLOCK;//リセット
			}
		}
	}

	//--------------------------------------//
	//	タイマフラグセット					//
	//--------------------------------------//
	//ゲームオーバーディレイタイマフラグセット
	public void SetFlgTimerUntilGameOverOn()
	{
		FlgTimerUntilGameOverOn = true;
	}

	//ブロック接触タイマフラグセット
	public void SetFlgTimerHittingBlockOn()
	{
		FlgTimerHittingBlockOn = true;
	}

	//--------------------------------------//
	//	タイマリセット						//
	//--------------------------------------//
	//ゲームオーバーディレイタイマリセット
	public void ResetTimerUntilGameOver()
	{
		TimerUntilGameOver = TIME_UNTIL_GAME_OVER;
		FlgTimerUntilGameOverOn = false;
	}

	//ブロック接触タイマリセット
	public void ResetTimerHittingBlock()
	{
		TimerHittingBlock = TIME_UNTIL_DAMAGE;
		FlgTimerHittingBlockOn = false;
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
