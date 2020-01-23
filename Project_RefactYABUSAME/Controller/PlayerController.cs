using UnityEngine;

public class PlayerController : MonoBehaviour
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
	private const float LIMIT_PLAYER_VELOCITY = 80;	//プレイヤの速度の上限(これ以上は加速しないようにする)
	private const float FORCE_PLAYER_MOVE_X = 7f;	//移動時にプレイヤに印加するx軸方向の力
	private const float FORCE_PLAYER_MOVE_Y = 0;	//移動時にプレイヤに印加するy軸方向の力
	private const float FORCE_PLAYER_MOVE_Z = 0;	//移動時にプレイヤに印加するz軸方向の力
	private const float FORCE_PLAYER_JUMP_X = -7f;	//ジャンプ時にプレイヤに印加するx軸方向の力(ジャンプ中加速し続けないようにx軸負方向に力を印加)
	private const float FORCE_PLAYER_JUMP_Y = 500;	//ジャンプ時にプレイヤに印加するy軸方向の力
	private const float FORCE_PLAYER_JUMP_Z = 0;    //ジャンプ時にプレイヤに印加するz軸方向の力

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	//Driver
	private AnimationDriver AnimationDriver;
	private UnityObjectDriver UnityObjectDriver;

	private Rigidbody RbPlayer;
	private bool PermitAddForce;	//プレイヤへの力の印加の許可、ポーズ時に禁止する。
	private bool FlgOnRoad;         //接地判定フラグ

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartPlayerController()
	{
		AnimationDriver = GameObject.Find("Driver").GetComponent<AnimationDriver>();
		UnityObjectDriver = GameObject.Find("Driver").GetComponent<UnityObjectDriver>();
		RbPlayer = UnityObjectDriver.GetRb(NAME_UNITY_OBJECT.PLAYER);
		PermitAddForce = true;//初期化(力の印加を禁止するのはポーズ、ゲームオーバーのとき)
		FlgOnRoad = false;//初期化(trueでもいい気もする)
	}

	//--------------------------------------//
	//	Update処理							//
	//--------------------------------------//
	public void UpdatePlayerController()
	{
		movePlayer();
	}

	//--------------------------------------//
	//	プレイヤ移動関数(毎周期)				//
	//--------------------------------------//
	private void movePlayer()
	{
		if (PermitAddForce && (RbPlayer.velocity.magnitude < LIMIT_PLAYER_VELOCITY))
		{	//力の印加が許可、かつ、プレイヤの速度が上限以下のとき
			RbPlayer.AddForce(FORCE_PLAYER_MOVE_X, FORCE_PLAYER_MOVE_Y, FORCE_PLAYER_MOVE_Z, ForceMode.Force);
		}
	}

	//--------------------------------------//
	//	プレイヤジャンプ関数(ボタンタップ)		//
	//--------------------------------------//
	public void JumpPlayer()
	{
		if (FlgOnRoad)
		{	//地面と接触しているときだけジャンプ可
			RbPlayer.constraints = RigidbodyConstraints.None;//Freezeを解除(全部解除してるから空中でBlockにぶつかると大変なことになるかも)
			RbPlayer.constraints = RigidbodyConstraints.FreezeRotation;
			RbPlayer.AddForce(FORCE_PLAYER_JUMP_X, FORCE_PLAYER_JUMP_Y, FORCE_PLAYER_JUMP_Z);

			Todo;//ジャンプアニメーションへ遷移
		}
	}

	//--------------------------------------//
	//	プレイヤへのAddForce解除				//
	//--------------------------------------//
	public void ClaerAddForcePlayer()
	{
		RbPlayer.velocity = Vector3.zero;
	}

	//--------------------------------------//
	//	プレイヤへのAddForce許可				//
	//--------------------------------------//
	public void SetAddForcePermittion()
	{
		PermitAddForce = true;
	}

	//--------------------------------------//
	//	プレイヤへのAddForce禁止				//
	//--------------------------------------//
	public void ClaerAddForcePermittion()
	{
		PermitAddForce = false;
	}

	//--------------------------------------//
	//	コールバック							//
	//--------------------------------------//
	//着地判定
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "tag_ROAD")//着地したとき
		{
			RbPlayer.constraints = RigidbodyConstraints.FreezePositionY;
			RbPlayer.constraints = RigidbodyConstraints.FreezePositionZ;
			RbPlayer.constraints = RigidbodyConstraints.FreezeRotation;
			FlgOnRoad = true;//接地判定フラグをセット
		}
	}

	//接地判定
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "tag_ROAD")//接地し続けているとき
		{
			FlgOnRoad = true;//接地判定フラグをセット
		}
	}

	//離地判定
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "tag_ROAD")//離れたとき
		{
			FlgOnRoad = false;//接地判定フラグをクリア
		}
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
