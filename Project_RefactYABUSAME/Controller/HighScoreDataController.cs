using UnityEngine;

public class HighScoreDataController : MonoBehaviour
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

	//Controller
	private ScoreDataController ScoreDataController;

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartHighScoreDataController()
	{
		HighScoreDriver = GameObject.Find("Driver").GetComponent<HighScoreDriver>();
		ScoreDataController = GameObject.Find("ScoreDataController").GetComponent<ScoreDataController>();
	}

	//--------------------------------------//
	//	ハイスコア更新判定処理					//
	//--------------------------------------//
	public void CompareHighScore()
	{
		int scoreNow = ScoreDataController.GetScoreNow();//今回のスコアを取得
		int highScore = HighScoreDriver.ReadHighScore();//今までのハイスコアを取得
		
		if(scoreNow > highScore)
		{   //上回っていれば更新
			HighScoreDriver.SaveHighScore(scoreNow);
		}
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
