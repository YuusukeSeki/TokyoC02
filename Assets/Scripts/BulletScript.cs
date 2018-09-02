using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public int speed;   // 移動速度


	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().velocity = transform.right.normalized * speed;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        // レイヤー名がEnemy、Boxの時は弾を削除
        if (layerName == "Enemy")
        {
            // EnemyとBulletの破棄
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (layerName == "Box")
        {
            // Bulletのみを破棄
            Destroy(gameObject);
        }

    }

}
