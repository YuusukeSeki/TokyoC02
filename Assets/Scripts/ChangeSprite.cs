using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {
    SpriteRenderer _sr;
    Player _player;

    public Sprite _SpriteStanding;
    public Sprite _SpriteSliding;
    bool _flag = false;
    bool _preFlag;


    // Use this for initialization
    void Start () {
        _player = GetComponent<Player>();
        _sr = GetComponent<SpriteRenderer>();
        _preFlag = _flag;

	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Change(bool sliding)
    {
        if (sliding == _preFlag)
            return;

        _flag = sliding;

        if (!_flag)
        {
            _sr.sprite = _SpriteStanding;
        }
        else
        {
            _sr.sprite = _SpriteSliding;
        }

        if (_preFlag != _flag)
        {
            Vector2 objectSize = _sr.bounds.size;
            _player.ResizeCollider2D(_flag, objectSize);
        }

        _preFlag = _flag;
    }

}
