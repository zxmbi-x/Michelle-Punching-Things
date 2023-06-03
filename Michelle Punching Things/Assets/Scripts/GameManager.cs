using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

	public int treesPunched = 0;
	public int stonePunched = 0;
	public int enemiesPunched = 0;
	public int playerHealth = 20;

	[SerializeField] private TextMeshProUGUI treesText;
	[SerializeField] private TextMeshProUGUI stoneText;
	[SerializeField] private TextMeshProUGUI enemiesText;
	[SerializeField] private TextMeshProUGUI playerText;
	[SerializeField] private TextMeshProUGUI gameOverText;
	[SerializeField] private GameObject gameUI;
	[SerializeField] private GameObject gameOverUI;
	[SerializeField] private GameObject[] enemies;
	private bool gameOver = false;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	// private void Start() {
	// 	Time.timeScale = 1;
	// }

	private void Update() {
		treesText.text = treesPunched + "";
		stoneText.text = stonePunched + "";
		enemiesText.text = enemiesPunched + "";
		playerText.text = playerHealth + "";

		if(treesPunched >= 5 && stonePunched >= 5 && enemiesPunched >= 5 && !gameOver) {
			WinGame();
		}
	}

	private void WinGame() {
		// Time.timeScale = 0;
		GameOver();
		gameOverText.text = "You won!";
	}

	public void SpawnEnemy() {
		if(enemiesPunched < 5) {
			enemies[enemiesPunched].SetActive(true);
		}
	}

	public void LoseGame() {
		// Time.timeScale = 0;
		GameOver();
		gameOverText.text = "You lost!";
	}

	private void GameOver() {
		gameUI.SetActive(false);
		gameOverUI.SetActive(true);
		gameOver = true;
	}

	public void RestartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void MainMenu() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

}
