using UnityEngine;

public class AnimationDriver : MonoBehaviour
{
	//==========================================================================//
	//	定義																	//
	//==========================================================================//

	//--------------------------------------//
	//	外部定数定義						//
	//--------------------------------------//

	//Animator内のフラグを定義(switch文に使用するため)
	public enum FLAG_ANIMATOR_UMA
	{
		JUMP,
	}

	public enum FLAG_ANIMATOR_PLAYER//アルファベット順にすること
	{
		JUMP,
		SHOOT,
	}

	//--------------------------------------//
	//	内部定数定義						//
	//--------------------------------------//

	//Animator内のフラグを定義
	private const string NameFlagUmaJump = "anim_FlgUmaJump";

	private const string NameFlagPlayerJump = "anim_FlgHumanJump";
	private const string NameFlagPlayerShoot = "anim_flg_shoot";

	//--------------------------------------//
	//	内部変数定義						//
	//--------------------------------------//

	private Animator AnimatorUma;
	private Animator AnimatorPlayer;

	//==========================================================================//
	//	関数																	//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartAnimationDriver()
	{
		AnimatorUma = GameObject.Find("uma").GetComponent<Animator>();
		AnimatorPlayer = GameObject.Find("hito&bow").GetComponent<Animator>();
	}

	//--------------------------------------//
	//	Animation遷移関数(Uma)				//
	//--------------------------------------//

	//AnimatorUma(FLAG_ANIMATOR_PLAYER フラグ名、bool 真/偽)
	public void TransitionAnimationUma(FLAG_ANIMATOR_UMA flagSetTarget, bool value)
	{
		switch (flagSetTarget)

		{
			case FLAG_ANIMATOR_UMA.JUMP:
				AnimatorUma.SetBool(NameFlagUmaJump, value);
				break;
			default:
				break;
		}
    }

	//--------------------------------------//
	//	Animation遷移関数(Player)			//
	//--------------------------------------//

	//AnimatorPlayer(FLAG_ANIMATOR_PLAYER フラグ名、bool 真/偽)
	public void TransitionAnimationPlayer(FLAG_ANIMATOR_PLAYER flagSetTarget, bool value)
	{
		switch (flagSetTarget)
		{
			case FLAG_ANIMATOR_PLAYER.JUMP:
				AnimatorPlayer.SetBool(NameFlagPlayerJump, value);
				break;
			case FLAG_ANIMATOR_PLAYER.SHOOT:
				AnimatorPlayer.SetBool(NameFlagPlayerShoot, value);
				break;
			default:
				break;
		}
	}

	//--------------------------------------//
	//	渡し処理							//
	//--------------------------------------//

	//AnimatorUma
	public float GetAnimationUmaNormalizedTime()
	{
		float ret;

		ret = AnimatorUma.GetCurrentAnimatorStateInfo(0).normalizedTime;

		return ret;
	}

	//AnimatorPlayer
	public float GetAnimationPlayerNormalizedTime()
	{
		float ret;

		ret = AnimatorPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime;

		return ret;
	}

}
