using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public int sceneId;    // ランニング、シューティングの切り替えスイッチ
    public bool isArrive;  // true でクリア

    public float level;             // レベル
    public float experience;        // 今までの取得経験値
    public float getExperience;     // 今ステージでの取得経験値
    public float maxHP;             // 最大HP
    public float maxMP;             // 最大MP
    public float hp;                // 現在のHP
    public float mp;                // 現在のMP
    public float runSpeed;          // 移動速度
    public float jumpPower;         // ジャンプ力
    public float attackPower;       // 攻撃力
    public float attackCost;        // 攻撃時に消費するMP
    public float healMp_PerSeconds;   // フレーム毎のMP自動回復量

    public float invincibleSeconds;     // 攻撃を受けた際の無敵時間（フレーム単位）

    float downSpeed;                    // 落下速度
    public float downSpeed_PerFrame;    // フレーム毎の落下加速度

    Rigidbody2D rb;             // 物理演算コンポーネント
    Animator animCtrl;          // アニメーションコントロール

    [SerializeField] GameObject bullet;             // PlayerBullet　プレハブ
    //[SerializeField] AudioManager _audioManager;  // オーディオデバイス

    Vector3 trgPos;             // タッチ座標バッファ(マウス：デバック用)


    // Use this for initialization
    void Start()
    {
        // 初期化処理
        Init();
    }

    // Update is called once per frame
    void Update () {

        if (isArrive)
        {

        }

        // 弾発射処理
        ShotBullet();

        // 接地判定処理
        if(CheckIsGround())
        {
            // ジャンプ&スライディング処理
            JumpAndSlidingProcessing();

        }

        // 前方向の障害物判定処理
        CheckForword();

        // 移動処理
        Move();

        // アニメーション変更処理
        // downSpeed < 0 : 下降アニメーション
        // downSpeed > 0 : 上昇アニメーション
        animCtrl.SetFloat("DownSpeed", downSpeed);

	}

    // 初期化処理
    void Init()
    {
        // プレイヤーパラメータの初期化
        hp = maxHP;
        mp = maxMP;

        // シーンIDの取得
        //sceneId = GetSceneId();

        // 物理演算コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();

        // アニメーションの取得
        animCtrl = GetComponent<Animator>();
        
        // ゴールフラグの初期化
        isArrive = false;

        // その他変数初期化
        downSpeed = 0;  // 落下速度

    }

    // 移動処理
    void Move()
    {
        Vector2 nowPos = rb.position;
        nowPos += new Vector2(runSpeed, downSpeed) * Time.deltaTime;
        rb.MovePosition(nowPos);
    }

    // 弾発射処理
    void ShotBullet()
    {
        if (Input.GetButtonDown("ShotBullet"))
        {
            // 弾をプレイヤーと同じ位置/角度で生成
            GameObject initBullet = Instantiate(bullet, transform.position + new Vector3(0.3f, 0.0f), transform.rotation);
            initBullet.GetComponent<PlayerBullet>().SetBullet(attackPower, 8);

        }
    }

    //  接地判定処理
    bool CheckIsGround()
    {
        RaycastHit2D hit;

        // 下方向チェック
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.32f, -0.32f), Vector2.right, 0.64f);

        if (hit.transform != null)
        {// 接地中の処理
            downSpeed = 0;  // 重力の初期化
            animCtrl.SetBool("IsGround", true); // アニメーションフラグの切り替え

            return true;

        }
        else
        {// 空中の処理
            // 重力処理（等加速度運動）
            downSpeed += downSpeed_PerFrame;  // 落下速度をどんどん早くする

            animCtrl.SetBool("IsGround", false);    // アニメーションフラグの切り替え

            return false;
        }


    }

    // 前方向の障害物判定処理
    bool CheckForword()
    {
        RaycastHit2D hit;

        // 前方向チェック
        hit = Physics2D.Raycast(transform.position + new Vector3(0.34f, 0.26f), Vector2.down, 0.52f);
        if (hit.transform != null)
        {
            // 障害物に当たった
            animCtrl.SetBool("IsIdle", true);

            return true;
        }
        else
        {
            // 障害物に当たっていない
            animCtrl.SetBool("IsIdle", false);

            return false;
        }
    }

    // ジャンプ&スライディング処理
    void JumpAndSlidingProcessing()
    {

        if (Input.GetButtonDown("Jump"))    // ジャンプのボタン判定
        {
            // タッチ座標の取得
            trgPos = Input.mousePosition;

        }
        else if (Input.GetButtonUp("Jump"))
        {
            // ジャンプ処理
            // リリース座標のY座標 > トリガー座標のY座標　でジャンプ。
            // リリース座標のY座標 < トリガー座標のY座標　でスライディング。
            // リリース座標のY座標 ==トリガー座標のY座標　で無処理（閾値の設定は要相談）
            if (Input.mousePosition.y > trgPos.y)   // ※タッチスクリーンのスワップ処理をマウスで代用中です
            {
                // ジャンプ処理
                downSpeed = jumpPower;
                transform.Translate(Vector3.up * 0.1f);

            }
            else if(Input.mousePosition.y < trgPos.y)
            {
                // スライディング処理

            }

        }

    }

    // 当たり判定
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        // Bullet(Enemy)との被弾処理
        if (layerName == "Bullet(Enemy)")
        {
            // ダメージ処理
            ReceiveDamage(collision.GetComponent<EnemyBullet>()._attackPower);

            // Enemyの破棄
            Destroy(collision.gameObject);

        }


    }

    // ダメージを受ける
    // damage : 受けるダメージ量
    public void ReceiveDamage(float damage)
    {
//        if()

        // HPを減らす
        hp -= damage;
        
        // HPが0以下でGameOver
        if(hp <= 0)
        {
            // GameOver処理

        }

    }

}
