using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//TODO tint
public class PlayerPanel : MonoBehaviour{
    public int number;
    internal PlayerInput.Type inputType;
    [SerializeField] TMP_Text text;

    int TypeCount => Enum.GetNames(typeof(PlayerInput.Type)).Length;

    void Awake() {
        inputType = PlayerInput.LoadPlayerInput(number);
    }

    void Update() {
        Refresh();
    }

    void Refresh() {
        text.text = $"P{number}: {GetInputName(inputType)}";
        if(!InputIsConnected(inputType))
            text.text += " (unconnected)"; 
    }

    string GetInputName(PlayerInput.Type inputType) {
        switch (inputType) {
            case PlayerInput.Type.Keyboard:     return "Keyboard";
            case PlayerInput.Type.Joystick1:    return "Joystick1";
            case PlayerInput.Type.Joystick2:    return "Joystick2";
            case PlayerInput.Type.Joystick3:    return "Joystick3";
            case PlayerInput.Type.Joystick4:    return "Joystick4";
        }
        return "Unselected";
    }

    bool InputIsConnected(PlayerInput.Type inputType) => ((int)inputType) < 2 || ((int)inputType - 1 ) <= Input.GetJoystickNames().Length;

    public void Previous() => AddNumber(-1);
    public void Next() => AddNumber(1);

    void AddNumber(int gain) {
        inputType = (PlayerInput.Type)((TypeCount + (int)inputType + gain) % TypeCount);
        if (inputType != PlayerInput.Type.None && InputTypeIsAlreadyUsed()) {
            AddNumber(gain);
            return;
        }
        Refresh();
    }

    bool InputTypeIsAlreadyUsed() => FindObjectsOfType<PlayerPanel>().Count((pp) => pp.inputType == inputType) >= 2;
}