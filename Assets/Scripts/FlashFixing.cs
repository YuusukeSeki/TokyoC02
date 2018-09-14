using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFixing : MonoBehaviour {

    SpriteRenderer _sr;

    bool _flash;        // 点滅フラグ

    float _flashTime;   // 点滅時間
    float _cntFlash;    // 計測カウンター
    float _alpha;       // α値
    bool _reverse;      // 反転フラグ


    // Use this for initialization
    void Start () {
        _sr = GetComponent<SpriteRenderer>();

        _flash = false;     // 点滅フラグ
        _flashTime = 1;     // 点滅時間
        _cntFlash = 0;      // 計測カウンター
        _alpha = 1;         // α値
        _reverse = true;    // 反転フラグ

    }

    // Update is called once per frame
    void Update () {
        if (!_flash)
            return;

        _cntFlash += Time.deltaTime;

        if (_cntFlash > _flashTime)
        {
            _alpha = 1;
            _cntFlash = 0;
        }
        else
        {
            if (_reverse)
            {
                //_alpha -= 0.1f;   // 規則的
                _alpha -= _flashTime - _cntFlash;  // 昔のロックマンみたい

                if (_alpha <= 0)
                {
                    _alpha = 0;
                    _reverse = !_reverse;
                }
            }
            else
            {
                //_alpha += 0.1f;   // 規則的
                _alpha += _flashTime - _cntFlash;  // 昔のロックマンみたい

                if (_alpha >= 1)
                {
                    _alpha = 1;
                    _reverse = !_reverse;
                }
            }

        }

        _sr.color = new Color(1, 1, 1, _alpha);

    }

    // 点滅開始
    public void StartFlash()
    {
        _flash = true;
        _cntFlash = 0;

    }

    // 点滅終了
    public void EndFlash()
    {
        _flash = false;
        _cntFlash = 0;
        _alpha = 1;
        _reverse = true;
    }

    // 点滅時間の設定
    public void SetFrashTime(float time)
    {
        _flashTime = time;

    }

}
