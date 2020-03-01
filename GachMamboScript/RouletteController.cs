using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteController : MonoBehaviour
{
	/* enum、マクロ、構造体											*/
	private enum ACTION_PATTERN
	{
		DEFAULT = 0,	//通常
		ROULETTE,		//ルーレット
		EXCLUSION		//除外
	}
	private const bool OFF = false;	//消灯
	private const bool ON = true;	//点灯
	private enum MASU_TURN_OFF_EVENT
	{
		NONE = 0,				//イベントなし
		TURN_OFF,				//消灯イベント
	}
	private enum MASU_ROULETTE_EVENT
	{
		NONE = 0,				//イベントなし
		ROULETTE_TURN_COMING,	//ルーレット自分の番来たイベント
		ROULETTE_TURN_END		//ルーレット自分の番終わりイベント
	}
	private struct EventStruct//イベントを取りまとめた構造体。イベント増えたらここに追加
	{
		public MASU_TURN_OFF_EVENT TurnOffEvent;
		public MASU_ROULETTE_EVENT RouletteEvent;
	}
	private struct MasuInfoStruct
	{
		public int Id;						//マスごとのID。何番目のマスかを示す(OnMasu1とかの1)
		public string NameON;				//ONのほうのオブジェクト名
		public string NameOFF;				//OFFのほうのオブジェクト名
		public ACTION_PATTERN ActionState;	//状態1。大きい状態(ルーレットマスのふるまいに何が要求されているか)
		public bool DisplayState;			//状態2。小さい状態(点灯/消灯)
		public bool Excluded;				//除外されているかを示すフラグ(trueなら除外されているマス)
		public EventStruct Event;			//イベント
	}
	private struct RequestInfoStruct	//ルーレットマスの動作に対する要求
	{
		public bool TurnOff;	//消灯要求
		public bool Roulette;	//ルーレット要求
	}
	private const int NUM_MASU_FIRST = 0;//最初のマスの番号
	private const int NUM_MASU_MAX = 7;//マスの数
	private const int NUM_EXCLUDE_MAX = 5;//除外できるマスの最大

	/* 変数実体定義													*/
	private GameObject OnMasu0;
	private GameObject OffMasu0;
	private GameObject OnMasu1;
	private GameObject OffMasu1;
	private GameObject OnMasu2;
	private GameObject OffMasu2;
	private GameObject OnMasu3;
	private GameObject OffMasu3;
	private GameObject OnMasu4;
	private GameObject OffMasu4;
	private GameObject OnMasu5;
	private GameObject OffMasu5;
	private GameObject OnMasu6;
	private GameObject OffMasu6;

	private MasuInfoStruct[] MasuInfo;

	private RequestInfoStruct Request;

	private int RouletteOnMasuIdThisCycle;//今周期に光らせるマスのID
	private int RouletteOnMasuIdBeforeCycle;//前周期で光らせたマスのID
	private uint RouletteTimer;//ルーレット残り時間を示すタイマー
	private int ExcludedMasuCounter;//除外されたマスの数を数えるカウンタ
	private bool[] ExcludedMasuList;//除外されたマスリスト(trueが除外されている)

	// Start is called before the first frame update
	void Start()
    {
		generateStructInstance();		//各構造体のインスタンス生成

		initMasuInfo();					//各マスの情報初期化
		initRequest();                  //RequestInfoの初期化

		/* 各内部変数の初期化 */
		RouletteOnMasuIdThisCycle = NUM_MASU_FIRST;
		RouletteOnMasuIdBeforeCycle = NUM_MASU_FIRST;
		//RouletteTimer = ROULETTE_TIME; ←ランダム値を設定しなきゃ！！
		ExcludedMasuList = new bool[NUM_MASU_MAX];
		for(int i=NUM_MASU_FIRST;i<NUM_MASU_MAX;i++)
		{
			ExcludedMasuList[i] = false;
		}

		startAttachObject();			//オブジェクトのアタッチ
	}

    // Update is called once per frame
    void Update()
    {
		if (isRequestComing())//もし要求あるなら
		{
			decideInput();//Input確定処理
			for(int masu = NUM_MASU_FIRST; masu < NUM_MASU_MAX; masu++)
			{
				fireEvent(masu);//イベント発行
				//状態遷移
			}
			//出力処理
			//次周期で使うInput更新処理(タイマとか前回光らせたマスIDとか)
		}
    }

	//==============================================================================//
	//	初期化処理																	//
	//==============================================================================//
	/* MasuInfoのインスタンス生成	*/
	private void generateStructInstance()
	{
		MasuInfo = new MasuInfoStruct[NUM_MASU_MAX];
		Request = new RequestInfoStruct();
	}
	//==================================================//
	/* MasuInfoの情報を初期化する							*/
	//==================================================//
	private void initMasuInfo()
	{
		for (int masu = NUM_MASU_FIRST; masu < NUM_MASU_MAX; masu++)
		{
			initMasuInfoId(masu);           //Idの初期化
			initMasuInfoNameON(masu);       //NameONの初期化
			initMasuInfoNameOFF(masu);      //NameOFFの初期化
			initMasuInfoActionState(masu);  //ActionStateの初期化
			initMasuInfoDisplayState(masu); //DisplayStateの初期化
			initMasuInfoExcluded(masu);		//Excludedの初期化
			initMasuInfoEvent(masu);        //Eventの初期化
		}
	}
	/* MasuInfoIdの初期化	*/
	private void initMasuInfoId(int masu)
	{
		MasuInfo[masu].Id = masu;
	}
	/* MasuInfoNameONの初期化	*/
	private void initMasuInfoNameON(int masu)
	{
		string name;

		switch (masu)
		{
			case 0:
				name = "FaultON";
				break;
			case 1:
				name = "1StepON";
				break;
			case 2:
				name = "2StepON";
				break;
			case 3:
				name = "3StepON";
				break;
			case 4:
				name = "Coin10EventStock+1ON";
				break;
			case 5:
				name = "Coin20EventStock+1ON";
				break;
			case 6:
				name = "Coin50EventStock+1ON";
				break;
			default:
				name = "";
				Debug.Log("MasuInfo.nameの初期化に失敗しました。");
				break;
		}

		MasuInfo[masu].NameON = name;
	}
	/* MasuInfoNameOFFの初期化	*/
	private void initMasuInfoNameOFF(int masu)
	{
		string name;

		switch (masu)
		{
			case 0:
				name = "FaultOFF";
				break;
			case 1:
				name = "1StepOFF";
				break;
			case 2:
				name = "2StepOFF";
				break;
			case 3:
				name = "3StepOFF";
				break;
			case 4:
				name = "Coin10EventStock+1OFF";
				break;
			case 5:
				name = "Coin20EventStock+1OFF";
				break;
			case 6:
				name = "Coin50EventStock+1OFF";
				break;
			default:
				name = "";
				Debug.Log("MasuInfo.nameの初期化に失敗しました。");
				break;
		}

		MasuInfo[masu].NameOFF = name;
	}
	/* MasuInfoActionStateの初期化	*/
	private void initMasuInfoActionState(int masu)
	{
		MasuInfo[masu].ActionState = ACTION_PATTERN.DEFAULT;
	}
	/* MasuInfoDisplayStateの初期化	*/
	private void initMasuInfoDisplayState(int masu)
	{
		MasuInfo[masu].DisplayState = OFF;
	}
	/* MasuInfoExcludedの初期化	*/
	private void initMasuInfoExcluded(int masu)
	{
		MasuInfo[masu].Excluded = false;
	}
	/* MasuInfoEventの初期化	*/
	private void initMasuInfoEvent(int masu)
	{
		MasuInfo[masu].Event.TurnOffEvent = MASU_TURN_OFF_EVENT.NONE;
		MasuInfo[masu].Event.RouletteEvent = MASU_ROULETTE_EVENT.NONE;
	}
	//==================================================//
	/* Requestの各メンバを初期化する						*/
	//==================================================//
	private void initRequest()
	{
		Request.TurnOff = false;
		Request.Roulette = false;
	}
	//==================================================//
	/* オブジェクトのアタッチを行う							*/
	//==================================================//
	private void startAttachObject()
	{
		OnMasu0 = GameObject.Find("FaultON");
		OffMasu0 = GameObject.Find("FaultOFF");
		OnMasu1 = GameObject.Find("1StepON");
		OffMasu1 = GameObject.Find("1StepOFF");
		OnMasu2 = GameObject.Find("2StepON");
		OffMasu2 = GameObject.Find("2StepOFF");
		OnMasu3 = GameObject.Find("3StepON");
		OffMasu3 = GameObject.Find("3StepOFF");
		OnMasu4 = GameObject.Find("Coin10EventStock+1ON");
		OffMasu4 = GameObject.Find("Coin10EventStock+1OFF");
		OnMasu5 = GameObject.Find("Coin20EventStock+1ON");
		OffMasu5 = GameObject.Find("Coin20EventStock+1OFF");
		OnMasu6 = GameObject.Find("Coin50EventStock+1ON");
		OffMasu6 = GameObject.Find("Coin50EventStock+1OFF");
	}
	//==============================================================================//
	//	Update処理																	//
	//==============================================================================//
	//==================================================//
	/* 要求が発生しているか確認を行う						*/
	//==================================================//
	private bool isRequestComing()
	{
		bool ret = false;

		if( (Request.TurnOff == true) || (Request.Roulette == true) )
		{	//どれかひとつでも要求が発生していればtrue
			ret = true;
		}

		return ret;
	}
	//==================================================//
	/* 要求ごとのInput確定処理							*/
	//==================================================//
	private void decideInput()
	{
		//消灯要求時はInput設定必要なし(全マス対象だから特に下準備なし)

		if(Request.Roulette)
		{
			decideInputByRoulette();    //ルーレット要求によるInput確定処理(ルーレット継続するか、光らせるマスを更新する)
		}
	}
	/* ルーレット要求によるInput確定処理(ルーレット継続するか、光らせるマスを更新する)	*/
	private void decideInputByRoulette()
	{
		excludeMasu();//除外処理
		updateRouletteOnMasuAvoidExcludedMasu();//除外マスを避けて光らせるマスを更新する
		
		/* ルーレット継続判定	*/
		if (RouletteTimer<=0)//ルーレット残り時間なしなら
		{
			ClearRouletteRequest();//ルーレット要求をクリアする(今回のルーレット処理は動く)
		}
	}
	/* 除外処理	*/
	private void excludeMasu()
	{
		/* 各マスが除外の対象になっていないかチェックする	*/
		for (int masu = NUM_MASU_FIRST; masu < NUM_MASU_MAX; masu++)
		{
			switch (ExcludedMasuCounter)
			{
				case 1:
					/* 除外マスカウンタが1なら1すすむマスを除外	*/
					if (MasuInfo[masu].NameON == "1StepON")//ON、OFFどちらでもいい(1すすむマスだと判別できれば)
					{
						MasuInfo[masu].Excluded = true;
					}
					break;
				case 2:
					/* 除外マスカウンタが2なら2すすむマスも除外	*/
					if( (MasuInfo[masu].NameON == "1StepON") || (MasuInfo[masu].NameON == "2StepON"))
					{
						MasuInfo[masu].Excluded = true;
					}
					break;
				case 3:
					/* 除外マスカウンタが3ならコイン50イベントストック+1マスも除外	*/
					if ((MasuInfo[masu].NameON == "1StepON") || (MasuInfo[masu].NameON == "2StepON")
						|| (MasuInfo[masu].NameON == "Coin50EventStock+1ON"))
					{
						MasuInfo[masu].Excluded = true;
					}
					break;
				case 4:
					/* 除外マスカウンタが4ならコイン20イベントストック+1マスも除外	*/
					if ((MasuInfo[masu].NameON == "1StepON") || (MasuInfo[masu].NameON == "2StepON")
						|| (MasuInfo[masu].NameON == "Coin50EventStock+1ON") || (MasuInfo[masu].NameON == "Coin20EventStock+1ON"))
					{
						MasuInfo[masu].Excluded = true;
					}
					break;
				case 5:
					/* 除外マスカウンタが5ならコイン10イベントストック+1マスも除外	*/
					if ((MasuInfo[masu].NameON == "1StepON") || (MasuInfo[masu].NameON == "2StepON")
						|| (MasuInfo[masu].NameON == "Coin50EventStock+1ON") || (MasuInfo[masu].NameON == "Coin20EventStock+1ON")
						|| (MasuInfo[masu].NameON == "Coin10EventStock+1ON"))
					{
						MasuInfo[masu].Excluded = true;
					}
					break;
				default:	//それ以外の値のときは何もしない
					break;
			}
		}
	}
	/* 除外マスを避けて光らせるマスを更新する	*/
	private void updateRouletteOnMasuAvoidExcludedMasu()
	{
		/* 光らせるマスが除外されている場合は、更新し続ける	*/
		do//do-while文はdo文内の処理を最低1回は行う(whileの条件式はdo文内の処理実行後に評価される)ため、光らせるマスの更新は必ず行うことができる
		{
			updateRouletteOnMasu();//光らせるマスを更新する
		} while (MasuInfo[RouletteOnMasuIdThisCycle].Excluded == true);
	}
	/* 光らせるマス更新処理	*/
	private void updateRouletteOnMasu()
	{
		RouletteOnMasuIdThisCycle++;

		if (RouletteOnMasuIdThisCycle >= NUM_MASU_MAX)//マス最大数までいってたら最初から
		{
			RouletteOnMasuIdThisCycle = NUM_MASU_FIRST;
		}
	}
	//==================================================//
	/* イベント発行										*/
	//==================================================//
	private void fireEvent(int masu)
	{
		if(Request.TurnOff)
		{
			fireEventByTurnOff(masu);
		}
		if(Request.Roulette)
		{
			fireEventByRoulette(masu);
		}
	}
	/* 消灯イベント発行	*/
	private void fireEventByTurnOff(int masu)
	{
		MasuInfo[masu].Event.TurnOffEvent = MASU_TURN_OFF_EVENT.TURN_OFF;
	}
	/* ルーレットイベント発行	*/
	private void fireEventByRoulette(int masu)
	{
		if ((masu != RouletteOnMasuIdBeforeCycle) && (masu == RouletteOnMasuIdThisCycle))
		{	//自分が光る番が来たら(= 前回は自分じゃない && 今回は自分)
			MasuInfo[masu].Event.RouletteEvent = MASU_ROULETTE_EVENT.ROULETTE_TURN_COMING;//自分の番来たイベント発行
		}
		else if( (masu == RouletteOnMasuIdBeforeCycle) && (masu != RouletteOnMasuIdThisCycle) )
		{	//自分が光る番がおわってたら(= 前回は自分 && 今回は自分じゃない)
			MasuInfo[masu].Event.RouletteEvent = MASU_ROULETTE_EVENT.ROULETTE_TURN_END;//自分の番終わりイベント発行
		}
		else//それ以外ではイベントなし(FS)
		{
			MasuInfo[masu].Event.RouletteEvent = MASU_ROULETTE_EVENT.NONE;
		}
	}
	//==============================================================================//
	//	Setter、Getter																//
	//==============================================================================//
	//==================================================//
	/* 要求処理											*/
	//==================================================//
	public void SetTurnOffRequest()
	{
		Request.TurnOff = true;
	}
	public void ClearTurnOffRequest()
	{
		Request.TurnOff = false;
	}
	public void SetRouletteRequest()
	{
		Request.Roulette = true;
	}
	public void ClearRouletteRequest()
	{
		Request.Roulette = false;
	}
	public void incrementExcludedMasuCounter()
	{
		if (ExcludedMasuCounter < NUM_EXCLUDE_MAX)
		{
			ExcludedMasuCounter++;
		}
	}
}
