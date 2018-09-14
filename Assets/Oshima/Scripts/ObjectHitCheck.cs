using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitCheck : MonoBehaviour {


public void OnCollisionEnter2D(Collision2D collision){
	// レイヤー名を取得
    string layerName = LayerMask.LayerToName(collision.gameObject.layer);
	if (layerName == "Player"){
		DoSomeEvent();
	}
	
}

public virtual void DoSomeEvent(){
	Debug.Log("collision");
}

}
