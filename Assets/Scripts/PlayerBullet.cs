using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {

    }

    // 弾の設定処理
    public void SetBullet(float attackPower, float speed)
    {
        GetComponent<Rigidbody2D>().velocity = transform.right.normalized * _speed;

        _attackPower = attackPower;
        _speed = speed;

    }

}
