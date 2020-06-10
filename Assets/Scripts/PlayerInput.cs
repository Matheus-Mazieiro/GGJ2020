using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO use inheritance
public class PlayerInput {
    public enum Type {
        None, Keyboard, Joystick1
    };

    Type type;

    public float VerticalInputAxis {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetAxis("Vertical");
                case Type.Joystick1:    return Input.GetAxis("VerticalJoystick1");
            }
            throw new System.NotImplementedException();
        }
    }

    public float HorizontalInputAxis {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetAxis("Horizontal");
                case Type.Joystick1:    return Input.GetAxis("HorizontalJoystick1");
            }
            throw new System.NotImplementedException();
        }
    }

    public float HorizontalInputAxisRaw {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetAxisRaw("Horizontal");
                case Type.Joystick1:    return Input.GetAxisRaw("HorizontalJoystick1");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool CloseHoleButtonTriggered {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetButton("Fire2");
                case Type.Joystick1:    return Input.GetButton("Fire2Joystick1");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool FlushButtonTriggered {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetButton("Fire2");
                case Type.Joystick1:    return Input.GetButton("Fire2Joystick1");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool DoorButtonTriggered {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetButtonDown("Fire2");
                case Type.Joystick1:    return Input.GetButtonDown("Fire2Joystick1");
            }
            throw new System.NotImplementedException();
        }
    }

    public PlayerInput(Type pType) {
        type = pType;
    }
}
