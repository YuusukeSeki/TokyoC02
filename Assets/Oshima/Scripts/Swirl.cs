using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swirl : ObjectHitCheck {

[SerializeField] GameManager _gameManager = null;
[SerializeField] int swirlId			  = 0;

public override void DoSomeEvent(){
	Debug.Log("swirl" + swirlId);
}
}
