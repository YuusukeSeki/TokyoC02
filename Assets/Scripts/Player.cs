using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //public int sceneId;    // ランニング、シューティングの切り替えスイッチ
    //public bool isArrive;  // true でクリア

    //public float level;             // レベル
    //public float experience;        // 今までの取得経験値
    //public float getExperience;     // 今ステージでの取得経験値
    //public float attackPower;       // 攻撃力
    //public float attackCost;        // 攻撃時に消費するMP

    [SerializeField] float maxHP;             // 最大HP
    [SerializeField] float maxMP;             // 最大MP
    [SerializeField] float hp;                // 現在のHP
    [SerializeField] float mp;                // 現在のMP
    [SerializeField] float runSpeed;          // 移動速度
    [SerializeField] float jumpPower;         // ジャンプ力
    [SerializeField] float healMp_PerSeconds; // フレーム毎のMP自動回復量
    [SerializeField] float invincibleSeconds; // 攻撃を受けた際の無敵時間（フレーム単位）

    // 画像切り替え（※切り替わらず。原因調査中）
    SpriteRenderer MainSpriteRenderer;
    [SerializeField] Sprite StandSprite;
    [SerializeField] Sprite SlidingSprite;

    float downSpeed;                            // 落下速度
    [SerializeField] float downSpeed_PerFrame;  // フレーム毎の落下加速度

    bool isGround;
    bool isSliding;
    float cntSliding;

    Rigidbody2D rb;             // 物理演算コンポーネント
    Animator animCtrl;          // アニメーションコントロール

    float prePos_Y; // 前フレームのY座標

    //[SerializeField] GameObject bullet;           // PlayerBullet　プレハブ
    //[SerializeField] AudioManager _audioManager;  // オーディオデバイス


    // Use this for initialization
    void Start()
    {
        MainSpriteRenderer = GetComponent<SpriteRenderer>();

        // プレイヤーパラメータの初期化
        hp = maxHP;
        mp = maxMP * 0.5f;

        // 物理演算コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();

        // アニメーションの取得
        animCtrl = GetComponent<Animator>();

        // その他変数初期化
        downSpeed = 0;      // 落下速度
        isGround = false;
        isSliding = false;
        cntSliding = 0;
        prePos_Y = transform.position.y;

    }

    // Update is called once per frame
    void Update () {
        //コライダーのサイズをオブジェクトに合わせる
        Vector2 objectSize = GetComponent<SpriteRenderer>().bounds.size;
        objectSize.y -= 0.04f;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = objectSize;


        // スライディング中
        if (isSliding)
        {
            cntSliding += Time.deltaTime;

            // スライディング終了処理
            if (cntSliding >= 1)
            {
                isSliding = false;
                cntSliding = 0;
                animCtrl.SetBool("IsSliding", false);

                // 画像切り替え（※切り替わらず。原因調査中）
                MainSpriteRenderer.sprite = StandSprite;

                // 座標補正（ガクつく）
                {
                    Vector3 pos = transform.position;
                    pos.y += 0.96f + 0.04f;
                    transform.position = pos;
                }

            }

        }

        // 接地判定
        CheckIsGround();

        // デバッグ用
        if (Input.GetButtonDown("Jump"))
        {
            // ジャンプ
            Jump();

        }
        if (Input.GetButtonDown("Fire2"))
        {
            // スライディング
            Sliding();
        }

        // 前方向の障害物判定処理
        //CheckForword();

        // 移動処理
        Move();

        // アニメーション変更処理
        // downSpeed < 0 : 下降アニメーション
        // downSpeed > 0 : 上昇アニメーション
        //float downSpeed = transform.position.y - prePos_Y;
        animCtrl.SetFloat("DownSpeed", downSpeed);

        // 前フレームのY座標更新
        prePos_Y = transform.position.y;

        // MP回復
        mp = mp + healMp_PerSeconds * Time.deltaTime > maxMP ? maxMP : mp + healMp_PerSeconds * Time.deltaTime;

    }

    // 初期化処理
    void Init()
    {
    }

    // 移動処理
    void Move()
    {
        Vector3 nowPos = transform.position + new Vector3(runSpeed, downSpeed, 0) * Time.deltaTime;
        transform.position = new Vector3(nowPos.x, nowPos.y, 0);

    }

    // 弾発射処理
    //void ShotBullet()
    //{
    //    if (Input.GetButtonDown("Fire2"))
    //    {
    //        // 弾をプレイヤーと同じ位置/角度で生成
    //        GameObject initBullet = Instantiate(bullet, transform.position + new Vector3(0.3f, 0.0f), transform.rotation);
    //        initBullet.GetComponent<PlayerBullet>().SetBullet(attackPower, 8);

    //    }
    //}

    //  接地判定処理
    bool CheckIsGround()
    {
        RaycastHit2D hit;

        // 下方向チェック
        if (!isSliding)
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.32f, -0.96f), Vector2.right, 0.64f);
        else
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.96f, -0.32f), Vector2.right, 1.92f);

        if (hit.transform != null)
        {// レイヤー名を取得
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

            if (layerName == "Ground")
            {// 接地中の処理
                Vector2 pos = transform.position;
                pos.y = hit.collider.transform.position.y + hit.collider.bounds.size.y * 0.5f + gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f - 0.04f;
                transform.position = pos;
                downSpeed = 0;                      // 重力の初期化

                animCtrl.SetBool("IsGround", true); // アニメーションフラグの切り替え
                isGround = true;

                return true;
            }
        }

        // 重力処理（等加速度運動）
        downSpeed -= downSpeed_PerFrame;        // 落下速度をどんどん早くする
        animCtrl.SetBool("IsGround", false);    // アニメーションフラグの切り替え
        isGround = false;

        return false;

    }

    // 前方向の障害物判定処理
    //bool CheckForword()
    //{
    //    RaycastHit2D hit;

    //    // 前方向チェック
    //    hit = Physics2D.Raycast(transform.position + new Vector3(0.34f, 0.26f), Vector2.down, 0.52f);
    //    if (hit.transform != null)
    //    {
    //        // 障害物に当たった
    //        //animCtrl.SetBool("IsIdle", true);

    //        return true;
    //    }
    //    else
    //    {
    //        // 障害物に当たっていない
    //        //animCtrl.SetBool("IsIdle", false);

    //        return false;
    //    }
    //}

    // ジャンプ&スライディング処理
    //void JumpAndSlidingProcessing()
    //{

    //    if (Input.GetButtonUp("Jump") && isGround)
    //    {
    //        Jump();
    //    }
    //    if (Input.GetButtonUp("Fire2") && isGround)
    //    {
    //        Sliding();
    //    }

    //}

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
        // HPを減らす
        hp -= damage;
        
        // HPが0以下でGameOver
        if(hp <= 0)
        {
            // GameOver処理

        }

    }

    // ジャンプ処理
    public bool Jump()
    {
        if (isGround)
        {
            downSpeed = jumpPower;

            //rb.AddForce(Vector2.up * jumpPower);

            return true;
        }
        else
            return false;
    }

    // スライディング処理
    public bool Sliding()
    {
        if (isGround)
        {
            isSliding = true;

            // 座標補正（ガクつく）
            {
                //Vector3 pos = transform.position;
                //pos.y -= 0.32f + 0.04f;
                //transform.position = pos;
            }

            // 画像切り替え（※切り替わらず。原因調査中）
            MainSpriteRenderer.sprite = SlidingSprite;

            animCtrl.SetBool("IsSliding", true);

            return true;
        }
        else
            return false;
    }

    // HP取得
    public float GetHp()
    {
        return hp;
    }


}
