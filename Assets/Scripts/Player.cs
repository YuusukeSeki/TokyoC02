using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* memo
     * ジャンプ時に、意図しないジャンプ力のバラつきがある。
     *   Jump()
     * 　rb.AddForce(Vector2.up * _jumpPower * 10);
     * 　
    */

    // デバッグ表示切替
    public bool _isOutputDebugLog;

    // 接地フラグ
    bool _isGround;

    // 横移動
    [SerializeField] float _runForce;    // 加速度
    float _runSpeed;                     // 現在の速度
    [SerializeField] float _runMaxSpeed; // 最大速度

    // Component
    Rigidbody2D _rb;
    BoxCollider2D _col;
    ChangeSprite _cs;
    FlashFixing _ff;
    Fade _fade;

    // 上移動
    [SerializeField] float _jumpPower;  // ジャンプ力
    float _jumpThreshold;               // 空中判定の閾値

    // 体力
    public float hp, maxHp;

    // 無敵処理関係
    bool _invincible;
    public float _invincibleTime;  // 時間
    float _cntInvincibleTime;    // 計測カウンター

    // ゴールフラグ
    bool isArrive;

    // 死亡フラグ
    bool isDead;


    // 初期座標（デバッグ用。ゴール時、死亡時にこの地点に戻す）
    Vector3 respownPosition;

    // 画面の下の座標
    float _screenBottom;


    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _cs = GetComponent<ChangeSprite>();
        _ff = GetComponent<FlashFixing>();

        _runSpeed = 0;
        _jumpThreshold = 0.00001f;
        hp = maxHp;
        _invincible = false;
        _cntInvincibleTime = 0;

        _ff.SetFrashTime(_invincibleTime);

        isArrive = false;

        _fade = GameObject.Find("Panel").GetComponent<Fade>();
        respownPosition = transform.position;

        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 buf = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        _screenBottom = -buf.y;

    }

    // Update is called once per frame
    void Update()
    {
        // ゴール判定
        if (isArrive)
            return;

        if (_fade.GetFadeState() != Fade.FadeState.NONE)
            return;


        // 状態の変更
        ChangeState();

        // 移動処理
        Move();

        // 穴落ちた判定
        if(transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y < _screenBottom)
        {
            _fade.SetColor(0, 0, 0);
            _fade.StartFadeOut();
        }

    }

    public void Init()
    {
        hp = maxHp;
        _invincible = false;
        _cntInvincibleTime = 0;
        isArrive = false;
        transform.position = respownPosition;
        _rb.velocity = new Vector2(0, 0);
    }

    // 接地判定
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (col.gameObject.transform.position.y < transform.position.y)
            {
                if (!_isGround)
                    _isGround = true;
            }
        }

    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (col.gameObject.transform.position.y < transform.position.y)
            {
                if (!_isGround)
                    _isGround = true;
            }
        }

    }

    // 被弾判定,ゴール判定
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet(Enemy)")
        {
            ReceiveDamage(col.GetComponent<EnemyBullet>()._attackPower);

            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "GoalFlag")
        {
            // フェードイン
            isArrive = true;

            _fade.SetColor(1, 1, 1);
            _fade.StartFadeOut();

        }

    }

    // 状態の変更
    void ChangeState()
    {
        // 空中にいるかどうかの判定。上下の速度(rigidbody.velocity.y)が一定の値を超えている場合、空中とみなす
        if (Mathf.Abs(_rb.velocity.y) > _jumpThreshold)
        {
            _isGround = false;
        }

        // 無敵状態時の処理
        UpdateInvincible();

    }

    // 移動
    void Move()
    {
        //// デバッグ用ジャンプコード
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    Jump();
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    Sliding();
        //}
        //else
        //{
        //    SetStandSprite();
        //}

        // 左右の移動。一定の速度に達するまでは加速度を足していき、最大速度以降は最大速度に固定する。
        _runSpeed += _runForce * Time.deltaTime;
        if (_runSpeed > _runMaxSpeed)
        {
            _runSpeed = _runMaxSpeed;
        }

        transform.position += new Vector3(_runSpeed * Time.deltaTime, 0, 0);

    }

    // ジャンプ
    public void Jump()
    {
        if (_isOutputDebugLog)
            Debug.Log("Player::Jump() が呼ばれました");

        if (_isGround)
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }

    // スライディング
    public void Sliding()
    {
        if (_isOutputDebugLog)
            Debug.Log("Player::Sliding() が呼ばれました");

        if (_isGround)
        {
            _cs.Change(true);
        }

    }

    // ダメージを受ける
    // damage : 受けるダメージ量
    public void ReceiveDamage(float damage)
    {
        if (_invincible)
            return;

        hp -= damage;

        if (hp <= 0)
        {
            // キャラクタ交代処理？

        }
        else
        {
            SetInvincible();
            _ff.StartFlash();
        }

    }

    // コリジョンの修正
    public void ResizeCollider2D(bool sliding, Vector2 objectSize)
    {
        _col.size = objectSize;

        float moveY = objectSize.y * 0.5f;

        if (sliding)
        {
            transform.position += new Vector3(0, -moveY, 0);
        }
        else
        {
            transform.position += new Vector3(0, +moveY, 0);
        }

    }

    // spriteを立ち絵に変更する
    public void SetStandSprite()
    {
        _cs.Change(false);
    }

    // 無敵状態にする
    void SetInvincible()
    {
        _invincible = true;
        _cntInvincibleTime = 0;

    }

    // 無敵状態時のみ行われる処理
    void UpdateInvincible()
    {
        if (!_invincible)
            return;

        _cntInvincibleTime += Time.deltaTime;

        if(_cntInvincibleTime > _invincibleTime)
        {
            _invincible = false;
            _cntInvincibleTime = 0;
            _ff.EndFlash();
        }

    }

}
