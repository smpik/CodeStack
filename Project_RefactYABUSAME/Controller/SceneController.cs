using UnityEngine;

public class SceneController : MonoBehaviour
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
	private SceneDriver SceneDriver;
	private SoundDriver SoundDriver;
	private TimeScaleDriver TimeScaleDriver;

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartSceneController()
	{
		SceneDriver = GameObject.Find("Driver").GetComponent<SceneDriver>();
		SoundDriver = GameObject.Find("Driver").GetComponent<SoundDriver>();
		TimeScaleDriver = GameObject.Find("Driver").GetComponent<TimeScaleDriver>();
	}

	//--------------------------------------//
	//	シーン遷移処理(ボタンタップ)			//
	//--------------------------------------//
	//メインシーンへ遷移
	public void TransMainScene()
	{
		SoundDriver.PlaySoundTapButton();		//ボタンタップ音再生
		SceneDriver.TranScene(NAME_SCENE.MAIN);	//メインシーンへ遷移
	}

	//タイトルシーンへ遷移
	public void TransTitleScene()
	{
		SoundDriver.PlaySoundTapButton();			//ボタンタップ音再生
		TimeScaleDriver.PlayUnityWorldTime();		//ポーズの解除
		SceneDriver.TranScene(NAME_SCENE.TITLE);	//タイトルシーンへ遷移
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
