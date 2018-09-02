using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    int score;  // 現在の得点

    public float level;                 // レベル
    public float experience;            // 今までの取得経験値
    public float getExperience;         // 今ステージでの取得経験値
    public float maxHP;                 // 最大HP
    public float maxMP;                 // 最大MP
    public float hp;                    // 現在のHP
    public float mp;                    // 現在のMP
    public float runSpeed;              // 移動速度
    public float jumpPower;             // ジャンプ力
    public float attackPower;           // 攻撃力
    public float attackCost;            // 攻撃時に消費するMP
    public float healMp_PerFrame;       // フレーム毎のMP自動回復量

    float downSpeed;                    // 落下速度
    public float downSpeed_PerFrame;    // フレーム毎の落下加速度

    Rigidbody2D rb;             // 物理演算コンポーネント
    Animator animCtrl;          // アニメーションコントロール
    public GameObject bullet;   // PlayerBullet プレハブ

    Vector3 trgPos;             // タッチ座標バッファ(マウス：デバック用)


    // Use this for initialization
    void Start()
    {
        // プレイヤーパラメータの初期化
        hp = maxHP;
        mp = maxMP;

        // 物理演算コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();

        // アニメーションの取得
        animCtrl = GetComponent<Animator>();

        // その他変数初期化
        downSpeed = 0;  // 落下速度
        score = 0;      // 現在の得点

    }

    // Update is called once per frame
    void Update () {

        // 弾発射処理
        ShotBullet();

        // 接地判定処理
        CheckIsGround();

        // 前方向の障害物判定処理
        CheckForword();

        // 座標更新処理
        Vector2 nowPos = rb.position;
        nowPos += new Vector2(runSpeed, downSpeed) * Time.deltaTime;
        rb.MovePosition(nowPos);

        // アニメーション変更処理
        // downSpeed < 0 : 下降アニメーション
        // downSpeed > 0 : 上昇アニメーション
        animCtrl.SetFloat("DownSpeed", downSpeed);

	}

    // 弾発射処理
    void ShotBullet()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            // 弾をプレイヤーと同じ位置/角度で生成
            Instantiate(bullet, transform.position + new Vector3(0.3f, 0.0f), transform.rotation);
        }
    }

    //  接地判定処理
    void CheckIsGround()
    {
        RaycastHit2D hit;

        // 下方向チェック
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.32f, -0.32f), Vector2.right, 0.64f);
        if (hit.transform != null)
        {
            // 接地中の処理
            downSpeed = 0;  // 重力の初期化
            animCtrl.SetBool("IsGround", true); // アニメーションフラグの切り替え

            // ジャンプ&スライディング処理
            JumpAndSlidingProcessing();

        }
        else
        {
            // 空中の処理
            // 重力処理（等加速度運動）
            downSpeed += downSpeed_PerFrame;  // 落下速度をどんどん早くする

            animCtrl.SetBool("IsGround", false);    // アニメーションフラグの切り替え
        }

    }

    // 前方向の障害物判定処理
    void CheckForword()
    {
        RaycastHit2D hit;

        // 前方向チェック
        hit = Physics2D.Raycast(transform.position + new Vector3(0.34f, 0.26f), Vector2.down, 0.52f);
        if (hit.transform != null)
        {
            // 障害物に当たった
            animCtrl.SetBool("IsIdle", true);

        }
        else
        {
            // 障害物に当たっていない
            animCtrl.SetBool("IsIdle", false);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // コイン取得処理
        //collision.gameObject.SetActive(false);
        //score += 1; // スコア加算処理

        //Debug.Log(score);

    }


}
