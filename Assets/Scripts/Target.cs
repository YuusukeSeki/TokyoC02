using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    TextFixing _tf;

    // Use this for initialization
    void Start () {
        _tf = GameObject.Find("Main Camera").GetComponent<TextFixing>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _tf.LetterTodokeru();

            Destroy(gameObject);
        }

    }


}
