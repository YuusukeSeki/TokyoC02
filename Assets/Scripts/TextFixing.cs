using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFixing : MonoBehaviour {

    public Text _text;
    int _letter;

	// Use this for initialization
	void Start () {
        _letter = 0;
        _text.text = "Letter × " + _letter;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LetterTodokeru()
    {
        _letter++;

        _text.text = "Letter × " + _letter;

    }

}
