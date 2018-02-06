using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// store a public reference to the Player game object, so we can refer to it's Transform
	public GameObject player;

	// Store a Vector3 offset from the player (a distance to place the camera from the player at all times)
	private Vector3 offset;

    /// <summary>
    /// カメラのrotation初期値
    /// </summary>
    private Quaternion defRotation;

    /// <summary>
    /// 角度変化量
    /// </summary>
    private float deltaAngle;

    /// <summary>
    /// 角度変化速度 調整用の値
    /// </summary>
    [SerializeField]
    private float angleTrim = 100f;

    // At the start of the game..
    void Start ()
	{
		// Create an offset by subtracting the Camera's position from the player's position
		offset = transform.position - player.transform.position;

        defRotation = transform.rotation;

        deltaAngle = 0f;
	}

	// After the standard 'Update()' loop runs, and just before each frame is rendered..
	void LateUpdate ()
	{
        // Set the position of the Camera (the game object this script is attached to)
        // to the player's position, plus the offset amount
        //! transform.position = player.transform.position + offset;

        // 水平方向の入力から
        // 角度変化量を計算
        float moveHorizontal = Input.GetAxis("Horizontal");
        deltaAngle += moveHorizontal * Time.deltaTime * angleTrim; // 角度変化分

        // ワールドY軸を中心とした回転分の角度*初期角度
        transform.rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up) * defRotation;
        // プレイヤー位置からカメラ後方にoffset（初期位置関係）の距離だけ離れた場所
        transform.position = player.transform.position - transform.forward * offset.magnitude;

    }
}