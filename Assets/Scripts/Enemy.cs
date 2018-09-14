using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float hp;                    // 体力
    public float attackPower;           // 攻撃力
    public float moveSpeed;             // 移動速度

    public bool shotBullet;             // true で射撃行動を取る
    public bool aimingPlayer;           // true で弾をプレイヤー方向に撃つ

    public float attackPower_Bullet;    // 弾による攻撃力
    public float speed_Bullet;          // 弾の移動速度
    public float numShot_Seconds;       // 秒間発射数
    float cntTime;                      // 秒数記憶バッファ
    
    GameObject player;
    [SerializeField] GameObject bullet;

	// Use this for initialization
	void Start () {
        // 初期化処理
        Init();

    }

    // Update is called once per frame
    void Update () {
        // 移動処理
        Move();

        // 弾を撃つ
        ShotBullet();

    }

    // 初期化処理
    public virtual void Init()
    {
        // フレームカウンター初期化
        cntTime = 0;

        player =  GameObject.Find("Player");

        enabled = false;
    }

    // 解放処理
    public virtual void selfDelete()
    {

    }

    // 移動処理
    public virtual void Move()
    {
        if (aimingPlayer)
        {// 追尾
            Vector3 vec = (player.GetComponent<Player>().transform.position - transform.position) * moveSpeed;
            vec.Normalize();

            transform.position += vec * moveSpeed * Time.deltaTime;
        }
        else
        {// まっすぐ左に進行
            transform.position += transform.right * -moveSpeed * Time.deltaTime;
        }

    }

    // プレイヤーとの接触判定
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }

    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }

    }

    // 弾を撃つ処理
    public void ShotBullet()
    {
        // 射撃フラグがfalseで無処理
        if (!shotBullet)
            return;

        // 発射間隔毎に弾を撃つ
        cntTime += Time.deltaTime * numShot_Seconds;

        if(cntTime >= 1)
        {
            GameObject initBullet = Instantiate(bullet, transform.position + new Vector3(-0.6f, 0.0f), transform.rotation);

            if (aimingPlayer)
            {// プレイヤーを狙う場合
                // バレット生成位置からプレイヤーへの単位ベクトルを算出
                Vector3 aimVector = player.GetComponent<Player>().transform.position - initBullet.transform.position;
                aimVector.Normalize();

                // 進行方向をバレットに設定する
                initBullet.GetComponent<EnemyBullet>()._vector = aimVector;
            }

            initBullet.GetComponent<EnemyBullet>().SetBullet(aimingPlayer, attackPower_Bullet, speed_Bullet);

            cntTime = 0;
        }

    }

    // ダメージを受ける処理
    public void ReceiveDamage(float damage)
    {
        // HPを減らす
        hp -= damage;

        // HPが0以下で破棄
        if (hp <= 0)
        {
            Destroy(gameObject);

        }

    }

    // 画面外判定
    void OnBecameInvisible()
    {
        enabled = false;

    }

    // 画面内判定
    void OnBecameVisible()
    {
        enabled = true;

    }

}

