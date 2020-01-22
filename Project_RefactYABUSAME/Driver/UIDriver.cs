using UnityEngine;
using UnityEngine.UI;

public class UIDriver : MonoBehaviour
{
	//==========================================================================//
	//	定義																	//
	//==========================================================================//

	//--------------------------------------//
	//	外部定数定義						//
	//--------------------------------------//

	//--------------------------------------//
	//	内部定数定義						//
	//--------------------------------------//

	//--------------------------------------//
	//	内部変数定義						//
	//--------------------------------------//
	private GameObject CanvasMain;//メインキャンバス(ゲームプレイ中のUIを表示するキャンバス)
	private GameObject CanvasResult;//リザルトキャンバス(ゲームオーバー時のUIを表示するキャンバス)
	private GameObject CanvasPose;//ポーズキャンバス(ゲーム中プレイ中のポーズ時のUIを表示するキャンバス)

	private GameObject BlockComing;//ブロック検知警告用UI(表示位置を操作するためGameObject)

	private GameObject TextDamage;//ダメージ表示テキスト(アニメーションを使用するためGameObject)
	private GameObject TextInc;//得点表示テキスト(アニメーションを使用するためGameObject)

	private Text TextScore;//メインキャンバスに表示する現在のスコア
	private Text TextArrow;//メインキャンバスに表示する矢の残り本数
	private Text TextDistance;//メインキャンバスに表示する現在の到達距離
	private Text TextResultScore;//リザルトキャンバスに表示するスコア
	private Text TextResultDistance;//リザルトキャンバスに表示する到達距離
	private Text TextResultHighScore;//リザルトキャンバスに表示するハイスコア

	//==========================================================================//
	//	関数																	//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartUIDriver()
	{
		atachObject();				//オブジェクトのアタッチ
		undispStartAtachAfter();	//アタッチした後一部のUIを非表示(リザルトキャンバスなど(アタッチは表示されてないとできないから))
	}

	///オブジェクトのアタッチ(初期時にアタッチするものはここへ書く)
	private void atachObject()
	{
		CanvasMain = GameObject.Find("MainCanvas");
		CanvasResult = GameObject.Find("ResultCanvas");
		CanvasPose = GameObject.Find("PoseCanvas");
		BlockComing = GameObject.Find("BlockComing");
		TextDamage = GameObject.Find("TextDamage");
		TextInc = GameObject.Find("TextInc");
		TextScore = GameObject.Find("ScoreText").GetComponent<Text>();
		TextArrow = GameObject.Find("ArrowText").GetComponent<Text>();
		TextDistance = GameObject.Find("DistanceText").GetComponent<Text>();
		TextResultScore = GameObject.Find("TextResultScore").GetComponent<Text>();
		TextResultDistance = GameObject.Find("TextResultDistance").GetComponent<Text>();
		TextResultHighScore = GameObject.Find("TextResultHiScore").GetComponent<Text>();
	}

	///アタッチ後、非表示にしたいUIはここに書く
	private void undispStartAtachAfter()
	{
		UndispResultCanvas();	//リザルトキャンバスを非表示
		UndispPoseCanvas();		//ポーズキャンバスを非表示
		UndispBlockComing();	//ブロック検知警告UIを非表示
		UndispTextDamage();		//ダメージ表示テキストを非表示
		UndispTextInc();		//得点表示テキストを非表示
	}

	//--------------------------------------//
	//	表示非表示メソッド					//
	//--------------------------------------//

	///CanvasMain("CanvasMain"としたいがメソッド名が"Main"で終わると紛らわしいため名前の構成を入れ替えてる)
	public void DispMainCanvas()
	{
		CanvasMain.SetActive(true);
	}
	public void UndispMainCanvas()
	{
		CanvasMain.SetActive(false);
	}

	///CanvasPose
	public void DispPoseCanvas()
	{
		CanvasPose.SetActive(true);
	}
	public void UndispPoseCanvas()
	{
		CanvasPose.SetActive(false);
	}

	///ResultCanvas
	public void DispResultCanvas()
	{
		CanvasResult.SetActive(true);
	}
	public void UndispResultCanvas()
	{
		CanvasResult.SetActive(false);
	}

	///BlockComing
	public void DispBlockComing()
	{
		BlockComing.SetActive(true);
	}
	public void UndispBlockComing()
	{
		BlockComing.SetActive(false);
	}

	///TextDamage
	public void DispTextDamage()
	{
		TextDamage.SetActive(true);
	}
	public void UndispTextDamage()
	{
		TextDamage.SetActive(false);
	}

	///TextInc
	public void DispTextInc()
	{
		TextInc.SetActive(true);
	}
	public void UndispTextInc()
	{
		TextInc.SetActive(false);
	}

	//--------------------------------------//
	//	テキスト設定メソッド				//
	//--------------------------------------//
	///テキスト設定するものはここに書く

	///TextDamage(GameObjectなのでTextとやり方が少し違う)
	public void SetTextDamage(int damage)
	{
		TextDamage.GetComponent<Text>().text = "-" + damage;
	}

	///TextInc
	public void SetTextInc(int inc)
	{
		TextInc.GetComponent<Text>().text = "+" + inc;
	}

	///TextScore
	public void SetTextScore(uint score)
	{
		TextScore.text = score.ToString();
	}

	///TextArrow
	public void SetTextArrow(int arrow)
	{
		TextArrow.text = arrow.ToString();
	}

	///TextDistance
	public void SetTextDistance(int distace)
	{
		TextDistance.text = "距離：" + distace;
	}

	///TextResultScore
	public void SetTextResultScore(uint score)
	{
		TextResultScore.text = score.ToString();
	}

	///TextResultDistance;
	public void SetTextResultDistance(int distance)
	{
		TextResultDistance.text = distance.ToString();
	}

	///TextResultHighScore;
	public void SetTextResultHighScore(uint highScore)
	{
		TextResultHighScore.text = highScore.ToString();
	}

	//--------------------------------------//
	//	渡し処理							//
	//--------------------------------------//

}
