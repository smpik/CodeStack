using UnityEngine;

public class SoundDriver : MonoBehaviour
{
	//==========================================================================//
	//	定義																		//
	//==========================================================================//

	//--------------------------------------//
	//	外部定数定義							//
	//--------------------------------------//

	///サウンド定義(要アタッチ)
	public AudioClip SoundDispResult;//リザルトキャンバスを表示するときの音
	public AudioClip SoundHitItem;//矢がItemに当たった時の音
	public AudioClip SoundHitTarget;//矢が的に当たった時の音
	public AudioClip SoundHitWall;//矢が壁に当たった時の音
	public AudioClip SoundShoot;//矢の射出音
	public AudioClip SoundTapButton;//ボタンタップ音

	//--------------------------------------//
	//	内部定数定義							//
	//--------------------------------------//

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	private AudioSource AudioPlayer;//音を再生するオブジェクト

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartSoundDriver()
	{
		//AudioPlayerはオーディオ再生専用のオブジェクトなのでUnityObjectにかかわる処理でも本ドライバで行う。
		DontDestroyOnLoad(GameObject.Find("AudioPlayer"));//シーン遷移してもAudioPlayerを削除されないようにする。
		AudioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();//AudioSource取得
	}

	//--------------------------------------//
	//	サウンド再生メソッド					//
	//--------------------------------------//

	///PlayOneShotは第一引数が音源、第二引数が音量)

	public void PlaySoundDispResult()
	{
		AudioPlayer.PlayOneShot(SoundDispResult, 0.3f);
	}

	public void PlaySoundHitItem()
	{
		AudioPlayer.PlayOneShot(SoundHitItem, 0.5f);
	}

	public void PlaySoundHitTarget()
	{
		AudioPlayer.PlayOneShot(SoundHitTarget, 0.5f);
	}

	public void PlaySoundHitWall()
	{
		AudioPlayer.PlayOneShot(SoundHitWall, 0.1f);
	}

	public void PlaySoundShoot()
	{
		AudioPlayer.PlayOneShot(SoundShoot, 0.5f);
	}

	public void PlaySoundTapButton()
	{
		AudioPlayer.PlayOneShot(SoundTapButton, 0.5f);
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
