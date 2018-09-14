using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    float alfa;
    float speed = 0.01f;
    float red, green, blue;

    Player _player;

    public enum FadeState
    {
        NONE,
        FADE_IN,
        FADE_OUT
    };

    FadeState _fadeState;


    void Start()
    {
        _fadeState = FadeState.NONE;
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;

        _player = GameObject.Find("Player").GetComponent<Player>();

    }

    void Update()
    {
        if (_fadeState == FadeState.NONE)
            return;

        if (_fadeState == FadeState.FADE_OUT)
        {
            alfa += speed;
            if(alfa >= 1)
            {
                alfa = 1;
                _fadeState = FadeState.FADE_IN;
                _player.Init();
            }
        }
        else
        {
            alfa -= speed;
            if (alfa <= 0)
            {
                alfa = 0;
                _fadeState = FadeState.NONE;
            }
        }

        GetComponent<Image>().color = new Color(red, green, blue, alfa);


    }

    public void StartFadeOut()
    {
        _fadeState = FadeState.FADE_OUT;

    }

    public void SetColor(float r, float g, float b)
    {
        red = r;
        green = g;
        blue = b;
    }

    public FadeState GetFadeState()
    {
        return _fadeState;
    }

}
