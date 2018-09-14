using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

[SerializeField] AudioClip[] audioClips 	 = null;
AudioSource audioSource 				 	 = null;



	// Use this for initialization
	void Start () {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClips[0];
		audioSource.Play();//BGM1
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D)){
			audioSource.clip = audioClips[1];
			audioSource.Play();//BGM2
		}
		if(Input.GetKeyDown(KeyCode.A)){
			audioSource.PlayOneShot(audioClips[2]);//SE1
		}
		if(Input.GetKeyDown(KeyCode.S)){
			audioSource.PlayOneShot(audioClips[3]);//SE2
		}
	}
}
