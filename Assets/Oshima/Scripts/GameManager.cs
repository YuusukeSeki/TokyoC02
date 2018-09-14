using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

[SerializeField] Player _player 			= null;
//[SerializeField] Boss _boss					= null;
[SerializeField] UIManager _uiManager 		= null;
[SerializeField] AudioManager _audioManager = null;
[SerializeField] GameObject pousePanel 		= null;
int gameState								= 0;
EventSystem eventSystem 					= null;
string touchLayerName						= "";
Status status 								= Status.PLAYING;

enum Status{
		PLAYING,
		POUSE
	}


	// Use this for initialization
	void Start () {
		status = Status.PLAYING;
		eventSystem = EventSystem.current;
		pousePanel.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		if(status == Status.PLAYING){
			Play();
		}
		
	}

	private void Play(){
		_uiManager.EditHpGauge(_player.maxHp, _player.hp);

		//画面タップ時プレイヤーがジャンプ、アイコンの上ではジャンプしない
		if(Input.GetMouseButtonDown(0)) {
			touchLayerName = "";
　　　		 PointerEventData pointer = new PointerEventData(EventSystem.current);
        	pointer.position = Input.mousePosition;
        	List<RaycastResult> result = new List<RaycastResult>();
        	EventSystem.current.RaycastAll(pointer, result);

        	foreach (RaycastResult raycastResult in result)
        	{
            	touchLayerName = LayerMask.LayerToName(raycastResult.gameObject.layer);
        	}

			if(touchLayerName != "Icon"){
				_player.Jump();
			}
　		}
	}

	public void SceneChange(int id){
		//SceneManager.LoadScene ("scene" + id.ToString());
	}

	public void OnPauseButton(){
		Time.timeScale = 0f;
		status = Status.POUSE;
		pousePanel.SetActive(true);
	}
	
	public void OnRestartButton(){
		Time.timeScale = 1f;
		status = Status.PLAYING;
		pousePanel.SetActive(false);
	}
}
