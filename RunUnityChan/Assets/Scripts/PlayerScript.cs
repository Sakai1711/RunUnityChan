using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	public float speed = 4f;
	public float jumpPower = 700f; //ジャンプ力
	public LayerMask groundLayer; //Linecastで判定するLayer
	public GameObject mainCamera;
	public GameObject explosion;
	public Text gameOverText;

	private Rigidbody2D rigidbody2D;
	private Animator anim;
	private bool isGrounded; //着地判定
	private bool gameClear = false; //ゲームクリアーしたら操作を無効にする
	public Text clearText; //ゲームクリアー時に表示するテキスト
	Vector3 gameOverPos = new Vector3(-11.5f, -3.2f, 0f);

	protected Joystick joystick;
	protected JoyButtonScript joybutton;

	void Start () {
		anim = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();

		joystick = FindObjectOfType<Joystick>();
		joybutton = FindObjectOfType<JoyButtonScript>();
	}

	void Update ()
	{
		//Linecastでユニティちゃんの足元に地面があるか判定
		isGrounded = Physics2D.Linecast (
		transform.position + transform.up * 1,
		transform.position - transform.up * 0.05f,
		groundLayer);
		//ジャンプさせない
		if (!gameClear) {
			//スペースキーを押し、
			//if (Input.GetKeyDown ("space")) {
			if(joybutton.Pressed){
				//着地していた時、
				if (isGrounded) {
					//Dashアニメーションを止めて、Jumpアニメーションを実行
					anim.SetBool("Dash", false);
					anim.SetTrigger("Jump");
					//着地判定をfalse
					isGrounded = false;
					//AddForceにて上方向へ力を加える
					rigidbody2D.AddForce (Vector2.up * jumpPower);
				}
			}
		}
		//上下への移動速度を取得
		float velY = rigidbody2D.velocity.y;
		//移動速度が0.1より大きければ上昇
		bool isJumping = velY > 0.1f ? true:false;
		//移動速度が-0.1より小さければ下降
		bool isFalling = velY < -0.1f ? true:false;
		//結果をアニメータービューの変数へ反映する
		anim.SetBool("isJumping",isJumping);
		anim.SetBool("isFalling",isFalling);
		
		//落下でゲームオーバー
		if (transform.position.y < mainCamera.transform.position.y - 8)
			if (!gameClear)
				GameOver();
	}
	
	//ユニティちゃんの動きと同時にカメラを動かす
	void FixedUpdate ()
	{
		if (!gameClear) {
			if(!gameOverText.enabled){
				//float x = Input.GetAxisRaw ("Horizontal");
				float x = joystick.Horizontal;
				if (x != 0) {
					rigidbody2D.velocity = new Vector2 (x * speed, rigidbody2D.velocity.y);
					Vector2 temp = transform.localScale;
					temp.x = x;
					transform.localScale = temp;
					anim.SetBool ("Dash", true);
					if (transform.position.x > mainCamera.transform.position.x - 4) {
						Vector3 cameraPos = mainCamera.transform.position;
						cameraPos.x = transform.position.x + 4;
						mainCamera.transform.position = cameraPos;
					}
					Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
					Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
					Vector2 pos = transform.position;
					pos.x = Mathf.Clamp(pos.x, min.x + 0.5f, max.x);
					transform.position = pos;
				} else {
					rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
					anim.SetBool ("Dash", false);
				}
			}else{
				if (Input.GetMouseButtonDown (0)) {
					Application.LoadLevel("title");
				}
			}
		}else{
			//クリアーテキストを表示
			clearText.enabled = true;
			//アニメーションは走り
			anim.SetBool ("Dash", true);
			//右に進み続ける
			rigidbody2D.velocity = new Vector2 (speed, rigidbody2D.velocity.y);
			//5秒後にタイトル画面へ戻るCallTitleメソッドを呼び出す
			Invoke("CallTitle", 5);
		}
	}

	public void GameOver(){
		Instantiate (explosion, gameObject.transform.position + new Vector3(0f, 1.2f ,0f), gameObject.transform.rotation);
		transform.position = gameOverPos;
		//Destroy (gameObject);
		gameOverText.enabled = true;

		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel("title");
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		//タグがClearZoneであるTriggerにぶつかったら
		if (col.tag == "clearzone") {
			//ゲームクリアー
			gameClear = true;
		}
	}

	void CallTitle ()
	{
		//タイトル画面へ
		Application.LoadLevel("title");
	}
}