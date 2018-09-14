using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

[SerializeField] Text[] texts 		= null;
[SerializeField] Image[] Icons 	    = null;
[SerializeField] Image[] Bars 	    = null;
[SerializeField] Sprite[] sprites 	= null;

public bool onCanvasClick = false;


void Start(){

}

//HPBarの操作
public void EditHpGauge(float maxHp, float nowHp){
    float hp = nowHp/maxHp;
    Bars[0].fillAmount = hp;
}

public void EditMpGauge(){

}

public void EditItemNumber(){

}

public void EditScore(){
	
}

//Spriteの交換
private void EditSprite(int num){
    Sprite touchSprite = Icons[num].sprite;
    Sprite toSprite = Icons[0].sprite;

    Icons[0].sprite = touchSprite;
    Icons[num].sprite = toSprite;
}

//Iconをクリックした時
public void OnIconClick(int num){
    onCanvasClick = true;
    if(num == 0){
        Debug.Log("skil");
    }
    else if(num == 3){
        Debug.Log("friend");
    }
    else{
        EditSprite(num);
    }
    
}

}
