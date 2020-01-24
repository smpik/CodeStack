using UnityEngine;

public class DeleterController : MonoBehaviour
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
	private const float DISTANCE_DELETER_AND_PlAYER = 100;

	//--------------------------------------//
	//	内部変数定義							//
	//--------------------------------------//
	private UnityObjectDriver UnityObjectDriver;

	//==========================================================================//
	//	関数																		//
	//==========================================================================//

	//--------------------------------------//
	//	初期化処理							//
	//--------------------------------------//
	public void StartDeleterController()
	{
		UnityObjectDriver = GameObject.Find("Driver").GetComponent<UnityObjectDriver>();
	}

	public void UpdateDeleterController()
	{
		moveDeleter();
	}

	//--------------------------------------//
	//	DELETER移動処理						//
	//--------------------------------------//
	private void moveDeleter()
	{
		float posXPlayer = UnityObjectDriver.GetPos(UnityObjectDriver.NAME_UNITY_OBJECT.PLAYER).x;
		float posYDeleter = UnityObjectDriver.GetPos(UnityObjectDriver.NAME_UNITY_OBJECT.DELETER).y;
		float posZDeleter = UnityObjectDriver.GetPos(UnityObjectDriver.NAME_UNITY_OBJECT.DELETER).z;

		Vector3 setPos = new Vector3(posXPlayer - DISTANCE_DELETER_AND_PlAYER, posYDeleter, posZDeleter);

		UnityObjectDriver.SetPos(UnityObjectDriver.NAME_UNITY_OBJECT.DELETER, setPos);
	}

	//--------------------------------------//
	//	渡し処理								//
	//--------------------------------------//

}
