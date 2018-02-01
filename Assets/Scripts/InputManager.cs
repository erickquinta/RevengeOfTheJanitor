// Will handle all the UI Inputs (keycodes for now)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>{
    public KeyCode jump {get; set;}
    public KeyCode right {get;set;}
    public KeyCode left {get;set;}
    public KeyCode punch {get;set;}
    public KeyCode kick {get;set;}
    public KeyCode crouch {get;set;}

    void Awake(){
        jump = KeyCode.UpArrow;//(KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey, UpArrow"));
        right = KeyCode.RightArrow;
        left = KeyCode.LeftArrow;
        punch = KeyCode.X;
        kick = KeyCode.C;
        crouch = KeyCode.DownArrow;
    }

    void Update(){
        if(Input.GetKeyDown(jump)){
            PlayerManager.Instance.Jump();
        }
        if(Input.GetKeyDown(punch)){
            PlayerManager.Instance.Punch();
        }
        /*if(Input.GetKeyDown(right)){
            PlayerManager.Instance.Run();
        }
        if(Input.GetKeyDown(left)){
            PlayerManager.Instance.Run();
        }*/
        if(Input.GetKeyDown(kick)){
            PlayerManager.Instance.Kick();
        }
        if(Input.GetKeyDown(crouch)){
            PlayerManager.Instance.Crouch();
        }
    }
}