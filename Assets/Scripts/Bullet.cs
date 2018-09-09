using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float _attackPower;   // 攻撃力
    public float _speed;         // 移動速度
    public Vector3 _vector;         // 進行方向

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 移動処理
    public virtual void Move()
    {

    }

    // 当たり判定
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Box" || layerName == "Ground")
        {
            // このBulletを破棄
            Destroy(gameObject);
        }

    }

    // イベント処理
    public virtual void DoSomeEvent()
    {

    }

    // 画面外判定
    void OnBecameInvisible()
    {
        // オブジェクトの破棄
        Destroy(gameObject);

        //enabled = false;

    }

}

