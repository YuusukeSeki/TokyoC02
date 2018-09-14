using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChara : MonoBehaviour {

    //　現在どのキャラクターを操作しているか
    private int nowChara;

    //　操作可能なゲームキャラクター
    [SerializeField]
    private List<GameObject> charaLists;

	// Use this for initialization
    void Start()
    {
        //　最初の操作キャラクターを0番目のキャラクターにする為、キャラクターの総数をnowCharaに設定し最初のキャラクターが設定されるようにする
        nowChara = charaLists.Count;
        ChangeCharacter(nowChara);
    }

	// Update is called once per frame
    void Update()
    {
        //　Qキーが押されたら操作キャラクターを次のキャラクターに変更する
        if (Input.GetKeyDown("q"))
        {
            ChangeCharacter(nowChara);
        }
    }

    //　操作キャラクター変更メソッド
    void ChangeCharacter(int tempNowChara)
    {

        bool flag;  //　オン・オフのフラグ
        
        //　次の操作キャラクターを指定
        int nextChara = tempNowChara + 1;
        //　次の操作キャラクターがいない時は最初のキャラクターに設定
        if (nextChara >= charaLists.Count)
        {
            nextChara = 0;
        }
        //　次の操作キャラクターだったら動く機能を有効にし、それ以外は無効にする
        for (var i = 0; i < charaLists.Count; i++)
        {

            if (i == nextChara)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            //　操作するキャラクターと操作しないキャラクターで機能のオン・オフをする
            //charaLists[i].GetComponent<ControlOnOffChara>().enabled = flag;
            //　キャラクターのアニメーションを最初の状態にする為アニメーションパラメータのSpeedを0にする
            charaLists[i].GetComponent<Animator>().SetFloat("Speed", 0);
        }
        //　次の操作キャラクターを現在操作しているキャラクターに設定して終了
        nowChara = nextChara;
    }
}
