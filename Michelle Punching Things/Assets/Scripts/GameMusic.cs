using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour {

    private GameObject[] music;

    private void Awake() {
        if (gameObject.CompareTag("Game Music")) {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    private void Start() {
        music = GameObject.FindGameObjectsWithTag("Game Music");

        if(gameObject.CompareTag("Game Music") && music.Length > 1) {
            Destroy(music[1]);
        } else if(!gameObject.CompareTag("Game Music") && music.Length == 1){
            Destroy(music[0]);
        }
    }

}
