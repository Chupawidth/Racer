using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{


	public GameObject[] trafficCars, roadBlocks, vlcCans, sideTres, coinsParent, powerPickups, playerCars;
	public static GamePlayController Static;
	public static int collectedCoinsCounts, distanceTravelled;
	public static bool isGameEnded = false;
	public GameObject playerObj;
	public GameObject gameEndMenu;
	public Text coinsText, distanceText, ScoreText;
	float startPlayerCarPositionZ;
	public carCamera camScript;
	public float trafficStartingPoint;
	public float trafficCarDistance;
	public int ingameScoreCount;
	   
	
	// Use this for initialization
	void OnEnable ()
	{
		isGameEnded = false;
		playerCarControl.gameEnded += onGameEnd;
		coinControl.isONMagetPower = false;



	}

	void OnDisable ()
	{
		playerCarControl.gameEnded -= onGameEnd;
	}

	void onGameEnd (System.Object obj, System.EventArgs args)
	{
		CancelInvoke ();
		gameEndMenu.SetActive (true);
		this.enabled = false;
	}

	void Start ()
	{
		Static = this;
		//RenderSettings.fog = true;
		//for traffic cars

		collectedCoinsCounts = 0;

		if (camScript == null)
			camScript = Camera.main.GetComponent<carCamera> ();

		playerObj = GameObject.FindGameObjectWithTag ("Player");

		if (playerObj == null)
			playerObj = GameObject.Instantiate (playerCars [carSelection.carIndex]) as GameObject;
		startPlayerCarPositionZ = playerObj.transform.position.z;
		camScript.targetTrans = playerObj.transform;
	}

	public void OnGameStart ()
	{

	 
		InvokeRepeating ("generateTrafficCars", 2, 1.0f);
		InvokeRepeating ("generateTrafficCars", 60, 2.0f);
		InvokeRepeating ("generatePowerups", 15, 15);
		
		//for sideObject trees
		//and we don't need tree and rock in city
		if (!Application.loadedLevelName.Contains ("city")) {
			InvokeRepeating ("generateSideTress", 0, 2.0f);
		}
		
		InvokeRepeating ("generateCoins", 0, 2);
	}

	void LateUpdate ()
	{
	
		coinsText.text = "" + collectedCoinsCounts;
		ScoreText.text = "" + ingameScoreCount;
		distanceText.text = "" + GamePlayController.distanceTravelled + " m";
		if (isGameEnded) {
			coinsText.text = "";
			distanceText.text = "";
			ScoreText.text = "";
			//gameOverTxt.text = "GAME OVER :(";
		} else {
			distanceTravelled = Mathf.RoundToInt ((playerCarControl.thisPosition.z + (1104f)) / 10);
		}

		#if UNITY_EDITOR
		if (Input.GetKey (KeyCode.R)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);

		}

		#endif
	}

	float trafficCarCount = 5;

	void generateTrafficCars ()
	{
		int randomNumber = Random.Range (0, trafficCars.Length);
		GameObject trafficObj = GameObject.Instantiate (trafficCars [randomNumber]) as GameObject;
		
		trafficObj.transform.position = new Vector3 (Random.Range (-6f, 6f) * 4.5f, 0, playerObj.transform.position.z + (trafficCarDistance));


		
		trafficObj.transform.position = new Vector3 (Random.Range (-6f, 6f) * 4.5f, 0, playerObj.transform.position.z + 500);

		trafficCarCount++;

		AceHelper.randomizeArray (trafficCars);
	}

	void generatePowerups ()
	{
		
		GameObject pickupObj = GameObject.Instantiate (powerPickups [Random.Range (0, powerPickups.Length - 1)]) as GameObject;
		
		pickupObj.transform.position = new Vector3 (Random.Range (-10, 10), 5, playerObj.transform.position.z + 400 + (Random.Range (1, 10) * 10));
	}

	void generateSideTress ()
	{

		//left side
		GameObject treeObj = GameObject.Instantiate (sideTres [Random.Range (0, sideTres.Length - 1)]) as GameObject;
		treeObj.transform.position = new Vector3 (Random.Range (-80, -60), 0, playerObj.transform.position.z + 780);
		treeObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);

		treeObj = GameObject.Instantiate (sideTres [Random.Range (0, sideTres.Length - 1)]) as GameObject;
		treeObj.transform.position = new Vector3 (Random.Range (-90, -60), 0, playerObj.transform.position.z + 1230);
		treeObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
		//rightside
		treeObj = GameObject.Instantiate (sideTres [Random.Range (0, sideTres.Length - 1)]) as GameObject;
		treeObj.transform.position = new Vector3 (Random.Range (60, 80), 0, playerObj.transform.position.z + 820);
		treeObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);

		treeObj = GameObject.Instantiate (sideTres [Random.Range (0, sideTres.Length - 1)]) as GameObject;
		treeObj.transform.position = new Vector3 (Random.Range (60, 90), 0, playerObj.transform.position.z + 1320);
		treeObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
	}

	void generateCoins ()
	{
		//left side
		GameObject coin = GameObject.Instantiate (coinsParent [Random.Range (0, coinsParent.Length - 1)]) as GameObject;
		coin.transform.position = new Vector3 (Random.Range (-14, 6), 2, playerObj.transform.position.z + 400);
		
		Destroy (coin, 10);
		 
	}
}
