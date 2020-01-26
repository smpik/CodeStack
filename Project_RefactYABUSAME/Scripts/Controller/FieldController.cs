using UnityEngine;

public class FieldController : MonoBehaviour
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
	private const uint LIMIT_COUNTER_BLOCK	= 3;	//Roadを(LIMIT_COUNTER_BLOCK)回生成したらBlock生成
	private const uint LIMIT_COUNTER_ITEM	= 5;

	private const string PATH_PREFAB_ROAD	= "Prefabs/Road";
	private const string PATH_PREFAB_WALL	= "Prefabs/Wall";
	private const string PATH_PREFAB_TARGET	= "Prefabs/Target";
	private const string PATH_PREFAB_BLOCK	= "Prefabs/Block";
	private const string PATH_PREFAB_ITEM	= "Prefabs/Item";

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	private UnityObjectDriver UnityObjectDriver;

	private uint CounterBlock;		//Block生成タイミングを数えるためのカウンタ
	private uint CounterItem;		//Item生成タイミングのカウンタ

	private bool PermitCreateBlock;
	private bool PermitCreateItem;

	private uint IdLatestRoad;
	private uint IdOldestRoad;
	private int IdBlock;			//BlockのID、矢の減算に使う。

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartFieldController()
	{
		UnityObjectDriver = GameObject.Find("Driver").GetComponent<UnityObjectDriver>();

		CounterBlock	= LIMIT_COUNTER_BLOCK;
		CounterItem		= LIMIT_COUNTER_ITEM;

		PermitCreateBlock	= true;//最初は許可しておかないと1個目のブロックが作れない。(Blockを削除したときにしか許可しないため,続かなくなる)
		PermitCreateItem	= true;

		IdLatestRoad	= 1;//最初から配置してあるRoadのidは1だから
		IdOldestRoad	= 1;
		IdBlock			= 0;
	}

	//--------------------------------------//
	//	Field生成メイン処理					//
	//--------------------------------------//
	private void createFieldMain()
	{
		/* 定期的に生成するオブジェクトの生成	*/
		GameObject temp = createRoad();// Road生成をコール
		createWall(temp);//Wall生成をコール
		createTarget(temp);// Target生成をコール

		/* ランダムに生成するオブジェクトの生成	*/
		if (CounterBlock != 0)	//0でなければデクリメント
		{						//そうしないとuintは0-1=4???????になってしまうから次のif文がうまく回らない
			CounterBlock--;
		}
		if (PermitCreateBlock && (CounterBlock <= 0))
		{
			CounterBlock = LIMIT_COUNTER_BLOCK;//カウンタをリセット
			createBlock(temp);//Block生成をコール
			IdBlock++;//ID更新
		}

		if (CounterItem != 0)	//0でなければデクリメント
		{						//そうしないとuintは0-1=4???????になってしまうから次のif文がうまく回らない
			CounterItem--;
		}
		if (PermitCreateItem && (CounterItem <= 0))
		{
			CounterItem = LIMIT_COUNTER_ITEM;//カウンタをリセット
			createItem(temp);//Item生成をコール
		}
	}

	//--------------------------------------//
	//	Road生成準備							//
	//--------------------------------------//
	private GameObject createRoad()
	{
		float posXLatestRoad = 0;//最新Roadの位置
		Vector3 posCreateRoad;//生成位置
		float scaleLatestRoad = 0;//Roadの長さを取得

		/* ヒエラルキーから最新Roadを取得	*/
		uint i = IdOldestRoad;//do-while文用カウンタ(一番古いRoadから探索)
		do
		{
			posXLatestRoad	= UnityObjectDriver.GetPosByID(UnityObjectDriver.NAME_UNITY_OBJECT.ROAD, i).x;	//最新Roadの座標を取得
			scaleLatestRoad	= UnityObjectDriver.GetScaleByID(UnityObjectDriver.NAME_UNITY_OBJECT.ROAD, i).x;//最新Roadの長さを取得
			i++;
		} while (UnityObjectDriver.IsExistHierarchyByID(UnityObjectDriver.NAME_UNITY_OBJECT.ROAD, i));		//戻り値がfalse=存在しない、なら探索終了

		/* 生成位置設定	*/
		posCreateRoad = new Vector3(posXLatestRoad + scaleLatestRoad, -0.5f, 0);	//生成位置 = 最新Road.pos + 長さ

		/* 生成	*/
		IdLatestRoad++;																//最新RoadのIDを更新(Objectの名前に使うため)
		createFieldObject(PATH_PREFAB_ROAD, posCreateRoad, "Road" + IdLatestRoad);	//オブジェクト生成
		
		return objCreate;															//Wall,Target生成の際に座標を使わせてあげる
	}

	//--------------------------------------//
	//	Field生成							//
	//--------------------------------------//
	private void createFieldObject(string pathLoadPrefab, Vector3 posObjCreate, string newNameObj)
	{
		GameObject prefab = (GameObject)Resources.Load(pathLoadPrefab);					//生成対象のprefabを取得
		GameObject objCreate = Instantiate(prefab, posObjCreate, Quaternion.identity);	//生成
		UnityObjectDriver.ChangeGameObjectName(objCreate, newNameObj);					//生成オブジェクト名 = "Road" + id
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
