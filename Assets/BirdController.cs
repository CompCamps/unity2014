using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {
	public float upwardsVelocityFalloff = 14f;
	public float upwardsVelocityCompensate = 0;
	public float takeoffSpeed = 8;
	public float rightSpeed = 15;
	public int difficulty = 15;

	public float upperPipeLimit = 6;
	public float lowerPipeLimit = -4;
	public float rightPipeLimit = 24;
	public float leftPipeLimit = -16;

	int score = 0;
	int highscore = 0;

	GameObject pipeObject1;
	GameObject pipeObject2;
	bool gameStart;
	// Use this for initialization
	void Start () {
		this.rigidbody.useGravity = false;
		gameStart = false;

		pipeObject1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pipeObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pipeObject1.AddComponent<Rigidbody>();
		pipeObject2.AddComponent<Rigidbody>();

		pipeObject1.transform.localScale = new Vector3(4, 10, 3);
		pipeObject2.transform.localScale = new Vector3(4, 10, 3);

		pipeObject1.collider.isTrigger = true;
		pipeObject2.collider.isTrigger = true;

		pipeObject1.rigidbody.useGravity = false;
		pipeObject2.rigidbody.useGravity = false;

		resetGame();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			if(!gameStart)
			{
				gameStart = true;
				this.rigidbody.useGravity = true;
			}
			this.rigidbody.velocity = new Vector3(0,takeoffSpeed,0);
		}
		if(gameStart){
			if(this.rigidbody.velocity.y < upwardsVelocityFalloff && this.rigidbody.velocity.y > 0)
				this.rigidbody.AddRelativeForce(0,upwardsVelocityCompensate,0);

			if(pipeObject1.transform.position.x - this.rigidbody.position.x < leftPipeLimit)
			{
				pipeObject1.transform.position = new Vector3(this.rigidbody.position.x + rightPipeLimit, lowerPipeLimit + Random.Range(0, upperPipeLimit), 0);
				pipeObject2.transform.position = new Vector3(this.rigidbody.position.x + rightPipeLimit, pipeObject1.transform.position.y + difficulty, 0);
				score++;
				if(score > highscore)
					highscore = score;
			}
		}
	}

	void FixedUpdate() {
		if(gameStart){
			pipeObject1.rigidbody.velocity = new Vector3(-rightSpeed, 0, 0);
			pipeObject2.rigidbody.velocity = new Vector3(-rightSpeed, 0, 0);
		}
		Vector3 pos = this.rigidbody.transform.position;
		pos.z = 0;
		this.rigidbody.transform.position = pos;
		this.rigidbody.transform.rotation = new Quaternion(0,0,0,0);
	}

	void OnCollisionEnter(Collision collision) {
		resetGame ();
	}

	void OnTriggerEnter(Collider other){
		resetGame();
	}

	void resetGame()
	{
		score = 0;
		this.rigidbody.useGravity = false;
		gameStart = false;
		this.gameObject.transform.position = new Vector3(-3.4f, 5, 0);
		pipeObject1.transform.position = new Vector3(this.rigidbody.position.x + rightPipeLimit, lowerPipeLimit + Random.Range(0, upperPipeLimit), 0);
		pipeObject2.transform.position = new Vector3(this.rigidbody.position.x + rightPipeLimit, pipeObject1.transform.position.y + difficulty, 0);
		pipeObject1.rigidbody.velocity = new Vector3(0, 0, 0);
		pipeObject2.rigidbody.velocity = new Vector3(0, 0, 0);
		this.rigidbody.velocity = new Vector3(0,0,0);
	}

	void OnGUI() {
		GUIStyle style = GUI.skin.GetStyle ("label");
		//Set the style font size to increase and decrease over time
		style.fontSize = 24;

		GUI.Label(new Rect(10, 10, 200, 80), "Score: " + score + "\nHigh Score: " + highscore);
	}
}
