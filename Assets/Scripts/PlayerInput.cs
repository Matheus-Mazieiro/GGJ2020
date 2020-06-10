using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO use inheritance
public class PlayerInput {
    public enum Type {
        None, Keyboard, Joystick1, Joystick2, Joystick3, Joystick4
    };

    Type type;

    public float VerticalInputAxis {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetAxis("Vertical");
                case Type.Joystick1:    return Input.GetAxis("VerticalJoystick1");
                case Type.Joystick2:    return Input.GetAxis("VerticalJoystick2");
                case Type.Joystick3:    return Input.GetAxis("VerticalJoystick3");
                case Type.Joystick4:    return Input.GetAxis("VerticalJoystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public float HorizontalInputAxis {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetAxis("Horizontal");
                case Type.Joystick1:    return Input.GetAxis("HorizontalJoystick1");
                case Type.Joystick2:    return Input.GetAxis("HorizontalJoystick2");
                case Type.Joystick3:    return Input.GetAxis("HorizontalJoystick3");
                case Type.Joystick4:    return Input.GetAxis("HorizontalJoystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public float HorizontalInputAxisRaw {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetAxisRaw("Horizontal");
                case Type.Joystick1:    return Input.GetAxisRaw("HorizontalJoystick1");
                case Type.Joystick2:    return Input.GetAxisRaw("HorizontalJoystick2");
                case Type.Joystick3:    return Input.GetAxisRaw("HorizontalJoystick3");
                case Type.Joystick4:    return Input.GetAxisRaw("HorizontalJoystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool CloseHoleButtonTriggered {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetButton("Fire2");
                case Type.Joystick1:    return Input.GetButton("Fire2Joystick1");
                case Type.Joystick2:    return Input.GetButton("Fire2Joystick2");
                case Type.Joystick3:    return Input.GetButton("Fire2Joystick3");
                case Type.Joystick4:    return Input.GetButton("Fire2Joystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool FlushButtonTriggered {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetButton("Fire2");
                case Type.Joystick1:    return Input.GetButton("Fire2Joystick1");
                case Type.Joystick2:    return Input.GetButton("Fire2Joystick2");
                case Type.Joystick3:    return Input.GetButton("Fire2Joystick3");
                case Type.Joystick4:    return Input.GetButton("Fire2Joystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool DoorButtonTriggered {
        get {
            switch (type) {
                case Type.Keyboard:     return Input.GetButtonDown("Fire2");
                case Type.Joystick1:    return Input.GetButtonDown("Fire2Joystick1");
                case Type.Joystick2:    return Input.GetButtonDown("Fire2Joystick2");
                case Type.Joystick3:    return Input.GetButtonDown("Fire2Joystick3");
                case Type.Joystick4:    return Input.GetButtonDown("Fire2Joystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public PlayerInput(Type pType) {
        type = pType;
    }
}
