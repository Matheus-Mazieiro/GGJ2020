using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//TODO tint
public class PlayerPanel : MonoBehaviour{
    public int number;
    internal PlayerInput.Type inputType;
    [SerializeField] TMP_Text text;
    [SerializeField] Image controlImage;
    Color imageOriginalColor;

    int TypeCount => Enum.GetNames(typeof(PlayerInput.Type)).Length;

    void Awake() {
        inputType = PlayerInput.LoadPlayerInput(number);
        imageOriginalColor = controlImage.color;
    }

    void Update() {
        Refresh();
        RefreshPressedInput();
    }

    void Refresh() {
        text.text = $"P{number}: {GetInputName(inputType)}";
        if(!InputIsConnected(inputType))
            text.text += " (unconnected)"; 
    }

    void RefreshPressedInput() {
        if (!HasAnyButtonPressed(inputType))
            return;
        DOTween.Kill(controlImage);
        DOTween.Sequence().Append(
            controlImage.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.1f)
        ).Append(
            controlImage.DOColor(imageOriginalColor, 0.2f)
        );
    }

    string GetInputName(PlayerInput.Type inputType) {
        switch (inputType) {
            case PlayerInput.Type.Keyboard:     return "Keyboard";
            case PlayerInput.Type.Joystick1:    return "Joystick1";
            case PlayerInput.Type.Joystick2:    return "Joystick2";
            case PlayerInput.Type.Joystick3:    return "Joystick3";
            case PlayerInput.Type.Joystick4:    return "Joystick4";
        }
        return "None";
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

    bool HasAnyButtonPressed(PlayerInput.Type inputType) {
        if (inputType == PlayerInput.Type.None || inputType == PlayerInput.Type.Keyboard)
            return false;
        for (int i = 0; i < 20; i++)
            if (Input.GetKey($"joystick {(int)inputType - 1} button {i}"))
                return true;
        return false;
    }
}