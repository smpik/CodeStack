using UnityEngine;

public class GameController : MonoBehaviour
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

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	//Driver
	private HighScoreDriver HighScoreDriver;
	private SoundDriver SoundDriver;
	private TimeScaleDriver TimeScaleDriver;
	private UIDriver UIDriver;

	//Controller
	private ArrowController ArrowController;
	private ArrowDataController ArrowDataController;
	private HighScoreDataController HighScoreDataController;
	private PlayerController PlayerController;
	private ScoreDataController ScoreDataController;
	private TimerController TimerController;

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartGameController()
	{
		HighScoreDriver = GameObject.Find("Driver").GetComponent<HighScoreDriver>();
		SoundDriver = GameObject.Find("Driver").GetComponent<SoundDriver>();
		TimeScaleDriver = GameObject.Find("Driver").GetComponent<TimeScaleDriver>();
		UIDriver = GameObject.Find("Driver").GetComponent<UIDriver>();

		ArrowController = GameObject.Find("ArrowController").GetComponent<ArrowController>();
		ArrowDataController = GameObject.Find("ArrowDataController").GetComponent<ArrowDataController>();
		HighScoreDataController = GameObject.Find("HighScoreDataController").GetComponent<HighScoreDataController>();
		PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
		ScoreDataController = GameObject.Find("ScoreDataController").GetComponent<ScoreDataController>();
		TimerController = GameObject.Find("TimerController").GetComponent<TimerController>();
	}

	//--------------------------------------//
	//	ボタンタップで呼ばれる処理				//
	//--------------------------------------//
	//ポーズボタンから呼ばれる
	public void PoseGame()
	{
		TimeScaleDriver.StopUnityWorldTime();
		PlayerController.ClaerAddForcePermittion();	//プレイヤへの力の印加を禁止
		UIDriver.UndispMainCanvas();				//メインキャンバスを非表示
		UIDriver.DispPoseCanvas();					//ポーズキャンバスを表示
	}

	//リプレイボタンから呼ばれる
	public void ReplayGame()
	{
		TimeScaleDriver.PlayUnityWorldTime();
		PlayerController.SetAddForcePermittion();	//プレイヤへの力の印加を許可
		UIDriver.UndispPoseCanvas();				//ポーズキャンバスを非表示
		UIDriver.DispMainCanvas();					//メインキャンバスを表示
	}
	//--------------------------------------//
	//	ゲームオーバー処理					//
	//--------------------------------------//
	//矢がゼロ本になったら呼ばれる
	public void GameOver()
	{
		TimeScaleDriver.StopUnityWorldTime();
		UIDriver.UndispMainCanvas();				//メインキャンバスを非表示
		UIDriver.UndispPoseCanvas();				//ポーズキャンバスを非表示
		UIDriver.DispResultCanvas();				//リザルトキャンバスを表示
		SoundDriver.PlaySoundDispResult();			//リザルトキャンバス表示音再生
		HighScoreDataController.CompareHighScore(); //ハイスコアの更新判定
		setResult();								//リザルトテキスト設定

		/*	コンティニュー時に初期化が必要な変数の初期化	*/	//←コンティニュー確定してからでよくない?
		TimerController.ResetTimerUntilGameOver();	//ゲームオーバまでの待ち時間カウント用タイマのリセット
		ArrowController.SetFlagPermitShoot();		//矢の射出許可(ManageArrowにて矢が0本になったら射出禁止にするため)
	}

	//リザルトテキスト設定
	private void setResult()
	{
		int resultScore = ScoreDataController.GetScoreNow();//スコアの取得
		//到達距離の取得
		int highScore = HighScoreDriver.ReadHighScore();//ハイスコアの読み出し
		UIDriver.SetTextResultScore(resultScore);//リザルトスコアテキストの設定
		//リザルト到達距離の設定
		UIDriver.SetTextResultHighScore(highScore);//ハイスコアテキストの設定
	}

	//--------------------------------------//
	//	動画視聴後処理						//
	//--------------------------------------//
	public void WatchedReward()
	{
		UIDriver.UndispButtonContinue();		//「動画を見てコンティニュー」ボタンを非表示にする。
		PlayerController.ClaerAddForcePlayer();	//プレイヤに引火している力をリセットする(1回だけ力の印加を解除する。
		ArrowDataController.GiftArrows();		//矢の本数を回復
		UIDriver.UndispResultCanvas();			//リザルトキャンバスを非表示にする
		UIDriver.DispMainCanvas();				//メインキャンバスを表示する
		TimeScaleDriver.PlayUnityWorldTime();	//ポーズを解除
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
