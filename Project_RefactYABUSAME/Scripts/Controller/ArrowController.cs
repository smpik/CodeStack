using UnityEngine;

public class ArrowController : MonoBehaviour
{
	//==========================================================================//
	//	定義																		//
	//==========================================================================//

	//--------------------------------------//
	//	外部定数定義							//
	//--------------------------------------//
	private const int VALUE_SHOOT_ONE = 1;			//矢1本
	private const float VALUE_FORCE_ARROW_X = 0f;	//射出する際に矢に加える力
	private const float VALUE_FORCE_ARROW_Y = 0f;
	private const float VALUE_FORCE_ARROW_Z = 100f;

	//--------------------------------------//
	//	内部定数定義							//
	//--------------------------------------//

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	//Driver
	private AnimationDriver AnimationDriver;
	private SoundDriver SoundDriver;
	private UnityObjectDriver UnityObjectDriver;

	//Controller
	private ArrowDataController ArrowDataController;

	private bool FlagPermitShoot;
	private int NumArrowPattern;

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartArrowController()
	{
		AnimationDriver = GameObject.Find("Driver").GetComponent<AnimationDriver>();
		SoundDriver = GameObject.Find("Driver").GetComponent<SoundDriver>();
		UnityObjectDriver = GameObject.Find("Driver").GetComponent<UnityObjectDriver>();

		ArrowDataController = GameObject.Find("ArrowDataController").GetComponent<ArrowDataController>();

		FlagPermitShoot = false;
		NumArrowPattern = VALUE_SHOOT_ONE;
	}

	//--------------------------------------//
	//	ShootArrow							//
	//--------------------------------------//
	public void ShootArrow()
	{
		if (FlagPermitShoot)//射出許可あり
		{
			//射出パターン決定
			ArrowDataController.SubstractArrows(VALUE_SHOOT_ONE);											//矢の本数を減らす
			SoundDriver.PlaySoundShoot();																	//効果音の再生
			AnimationDriver.TransitionAnimationPlayer(AnimationDriver.FLAG_ANIMATOR_PLAYER.SHOOT, true);	//shootアニメーションへの遷移
		}
	}

	//--------------------------------------//
	//	矢射出許可							//
	//--------------------------------------//
	public void SetFlagPermitShoot()
	{
		FlagPermitShoot = true;
	}

	//--------------------------------------//
	//	矢射出禁止							//
	//--------------------------------------//
	public void ClearFlagPermitShoot()
	{
		FlagPermitShoot = false;
	}

	//--------------------------------------//
	//	矢射出パターン加算					//
	//--------------------------------------//
	public void AddNumArrowPattern()
	{
		NumArrowPattern++;
	}

	//--------------------------------------//
	//	射出パターン決定						//
	//--------------------------------------//
	private void shootArrowByPattern()
	{
		Vector3 posFirstArrowCreate;																//矢の生成位置
		float posXFirstArrow = 0;																	//基準となる一本目の矢の生成位置のx座標
		Vector3 posPlayer = UnityObjectDriver.GetPos(UnityObjectDriver.NAME_UNITY_OBJECT.PLAYER);	//プレイヤの位置を取得

		posXFirstArrow += (NumArrowPattern - 1);//射出する矢の本数により一本目の生成位置をずらす。(-1は1本だけのときにずらさないため)
		posFirstArrowCreate = new Vector3(posPlayer.x + posXFirstArrow, posPlayer.y + 3, posPlayer.z);//+3をしていないとプレイ他のコライダと矢が接触してしまいプレイヤが移動する。
		createShootArrow(posFirstArrowCreate);//一本目の矢を射出

		if (NumArrowPattern > VALUE_SHOOT_ONE)//複数本の矢を射出するとき
		{
			for (int i = 2; i <= NumArrowPattern; i++)
			{
				Vector3 posArrowCreate;
				float disBeforeArrow = (-1) * (i + i - 2);//直前の矢からずらす量を計算
				posArrowCreate = new Vector3(posPlayer.x + posXFirstArrow + disBeforeArrow, posPlayer.y + 3, posPlayer.z);
				createShootArrow(posArrowCreate);//i本目の矢を射出
			}
		}
	}

	//--------------------------------------//
	//	矢生成&射出							//
	//--------------------------------------//
	private void createShootArrow(Vector3 posCreate)
	{
		GameObject prefab = (GameObject)Resources.Load("Prefabs/Arrow");		//prefabを取得
		GameObject arrow = Instantiate(prefab, posCreate, Quaternion.identity);	//プレイヤの座標にarrowオブジェクトを生成
		arrow.GetComponent<Rigidbody>().AddForce(VALUE_FORCE_ARROW_X, VALUE_FORCE_ARROW_Y, VALUE_FORCE_ARROW_Z, ForceMode.Impulse);	//z軸正方向にarrowを飛ばす
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//
	//矢射出パターン取得
	public int GetNumArrowPattern()
	{
		return NumArrowPattern;
	}
}
