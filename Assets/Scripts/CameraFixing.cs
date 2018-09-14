using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixing : MonoBehaviour {

    GameObject _player;
    float _offsetX;

	// Use this for initialization
	void Start () {
        _player = GameObject.Find("Player");
        Vector3 leftTop = GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero);
        Vector3 rightBottom = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Vector3 size = rightBottom - leftTop;
        _offsetX = size.x * 0.25f;

    }
	
	// Update is called once per frame
	void Update () {
        // Y座標は0に固定
        Vector3 pos = transform.position;
        pos.y = 0;
        pos.x = _player.transform.position.x + _offsetX;
        transform.position = pos;

	}
}
