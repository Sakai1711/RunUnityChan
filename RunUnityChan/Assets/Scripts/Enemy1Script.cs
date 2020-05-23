using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {
	
	Rigidbody2D rigidbody2D;
	public int speed = -3;

    public PlayerScript playerScript;

	//メインカメラのタグ名　constは定数(絶対に変わらない値)
	private const string MAIN_CAMERA_TAG_NAME = "MainCamera";
	//カメラに映っているかの判定
	private bool _isRendered = false;

	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		if (_isRendered) {
			rigidbody2D.velocity = new Vector2 (speed, rigidbody2D.velocity.y);
		}

		if (gameObject.transform.position.y < Camera.main.transform.position.y - 8 ||gameObject.transform.position.x < Camera.main.transform.position.x - 10) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		//UnityChanとぶつかった時
		if (col.gameObject.tag == "UnityChan") {
			playerScript.GameOver();
		}
	}

	void OnWillRenderObject()
	{
    //メインカメラに映った時だけ_isRenderedをtrue
		if(Camera.current.tag == MAIN_CAMERA_TAG_NAME){
		_isRendered = true;
		}
	}
}