using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour {
    public Sprite SpriteMae;
    public Sprite SpriteAto;
    bool flag = false;
    int cnt;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        cnt++;

        if(cnt % 50 == 0)
        {
            flag = !flag;
        }

        if (flag)
        {
            GetComponent<SpriteRenderer>().sprite = SpriteMae;

            //Resources.Load<Sprite>(fileName);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = SpriteAto;

        }

    }
}
