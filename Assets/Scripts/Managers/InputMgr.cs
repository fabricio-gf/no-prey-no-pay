﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class InputMgr : MonoBehaviour
{
    // --------------------------------------- ENUMS ------------------------------------- //
    public enum eAxis
    {
        HORIZONTAL,
        VERTICAL
    }

    public enum eButton
    {
        DASH,
        JUMP,
        TOSS,
        ATTACK,
        GRAB
    }

    public enum eMenuButton
    {
        SUBMIT,
        PREVIOUS,
        PAUSE,
        CHANGE_COLOR,

        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public enum eXBoxButton
    {
        A,
        B,
        X,
        Y,
        DPAD_LEFT,
        DPAD_RIGHT,
        DPAD_UP,
        DPAD_DOWN,
        START,
        OPTIONS,
        BUMPR_LEFT,
        TRIGG_LEFT,
        STICK_LEFT,
        BUMPR_RIGHT,
        TRIGG_RIGHT,
        STICK_RIGHT,
    }

    // --------------------------------- PUBLIC ATTRIBUTES ------------------------------- //
    public InputConfig m_configData;

    //[Header("Global")]
    private float m_triggMinRatio    = .3f;
    private bool m_pcDebugMode       = true;

    //[Header("Locomotion")]
    private eXBoxButton m_dashButton;

    private eXBoxButton m_jumpButton;

    private eXBoxButton m_tossButton;
    private eXBoxButton m_attackButton;
    private eXBoxButton m_grabButton;

    //[Header("Menu")]
    private eXBoxButton m_submitButton;
    private eXBoxButton m_previousButton;
    private eXBoxButton m_pauseButton;
    private eXBoxButton m_changeColorButton;



    // --------------------------------- PRIVATE ATTRIBUTES ------------------------------ //
    private static InputMgr m_manager;
    private PlayerIndex[] m_playerBind = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };


    // ======================================================================================
    // PUBLIC MEMBERS
    // ======================================================================================
    public void Start()
    {
        if (!Application.isEditor)
            m_pcDebugMode = false;

        Debug.Assert(m_manager == null, this.name + " - InputMgr : Mgr must be unique!");
        m_manager = this;

        Debug.Assert(m_configData != null, this.name + " - InputMgr : Missing config data");
        InitializeParams();
    }
    
    // ======================================================================================
    public static bool GetButton(int _player, eButton _button)
    {
#if UNITY_EDITOR
        if (m_manager.m_pcDebugMode && _player == 1)
            return GetDebugButton(_button);
        
        Debug.Assert(m_manager != null, "InputMgr - Missing InputMgrConfig in Project");
#endif

        if (_player > 4 || _player <= 0)
            return false;

        GamePadState gamePadState = GamePad.GetState((PlayerIndex)(_player - 1));

        switch (_button)
        {
            case eButton.ATTACK:
                return GetButton(gamePadState, m_manager.m_attackButton);
            case eButton.DASH:
                return GetButton(gamePadState, m_manager.m_dashButton);
            case eButton.GRAB:
                return GetButton(gamePadState, m_manager.m_grabButton);
            case eButton.TOSS:
                return GetButton(gamePadState, m_manager.m_tossButton);
            case eButton.JUMP:
                return GetButton(gamePadState, m_manager.m_jumpButton);
        }

        return false;
    }

    // ======================================================================================
    public static bool GetMenuButton(eMenuButton _menuButton)
    {
#if UNITY_EDITOR
        Debug.Assert(m_manager != null, "InputMgr - Missing InputMgrConfig in Project");
        if (m_manager.m_pcDebugMode && _menuButton == eMenuButton.SUBMIT)
            return Input.GetKey(KeyCode.Return);
#endif

        bool isPressed = false;

        for (int player = 0; player < 4; player++)
        {
            GamePadState gamePadState = GamePad.GetState((PlayerIndex)(player));

            switch (_menuButton)
            {
                case eMenuButton.SUBMIT:
                    isPressed |= GetButton(gamePadState, m_manager.m_submitButton);
                    break;
                case eMenuButton.PREVIOUS:
                    isPressed |= GetButton(gamePadState, m_manager.m_previousButton);
                    break;
                case eMenuButton.PAUSE:
                    isPressed |= GetButton(gamePadState, m_manager.m_pauseButton);
                    break;
                case eMenuButton.LEFT:
                    isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_LEFT)   || gamePadState.ThumbSticks.Left.X < -m_manager.m_triggMinRatio;
                    break;
                case eMenuButton.RIGHT:
                    isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_RIGHT)  || gamePadState.ThumbSticks.Left.X > m_manager.m_triggMinRatio;
                    break;
                case eMenuButton.UP:
                    isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_UP)     || gamePadState.ThumbSticks.Left.Y > m_manager.m_triggMinRatio;
                    break;
                case eMenuButton.DOWN:
                    isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_DOWN)   || gamePadState.ThumbSticks.Left.Y < -m_manager.m_triggMinRatio;
                    break;
                case eMenuButton.CHANGE_COLOR:
                    isPressed |= GetButton(gamePadState, m_manager.m_changeColorButton);
                    break;
            }
        }

        return isPressed;
    }

    // ======================================================================================
    public static bool GetMenuButton(int _player, eMenuButton _menuButton)
    {
#if UNITY_EDITOR
        Debug.Assert(m_manager != null, "InputMgr - Missing InputMgrConfig in Project");
#endif

        if (_player > 4 || _player <= 0)
            return false;

        bool isPressed = false;

        GamePadState gamePadState = GamePad.GetState((PlayerIndex)(_player - 1));

        switch (_menuButton)
        {
            case eMenuButton.SUBMIT:
                isPressed |= GetButton(gamePadState, m_manager.m_submitButton);
                break;
            case eMenuButton.PREVIOUS:
                isPressed |= GetButton(gamePadState, m_manager.m_previousButton);
                break;
            case eMenuButton.PAUSE:
                isPressed |= GetButton(gamePadState, m_manager.m_pauseButton);
                break;
            case eMenuButton.LEFT:
                isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_LEFT) || gamePadState.ThumbSticks.Left.X < -m_manager.m_triggMinRatio;
                break;
            case eMenuButton.RIGHT:
                isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_RIGHT) || gamePadState.ThumbSticks.Left.X > m_manager.m_triggMinRatio;
                break;
            case eMenuButton.UP:
                isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_UP) || gamePadState.ThumbSticks.Left.Y < -m_manager.m_triggMinRatio;
                break;
            case eMenuButton.DOWN:
                isPressed |= GetButton(gamePadState, eXBoxButton.DPAD_DOWN) || gamePadState.ThumbSticks.Left.Y > m_manager.m_triggMinRatio;
                break;
            case eMenuButton.CHANGE_COLOR:
                isPressed |= GetButton(gamePadState, m_manager.m_changeColorButton);
                break;
        }

        return isPressed;
    }
    
    // ======================================================================================
    public static float GetAxis(int _player, eAxis _axis)
    {
#if UNITY_EDITOR
        if (m_manager.m_pcDebugMode && _player == 1)
            return GetDebugAxis(_axis);
        
        Debug.Assert(m_manager != null, "InputMgr - Missing InputMgrConfig in Project");
#endif

        if (_player > 4 || _player <= 0)
            return 0f;

        GamePadState gamePadState = GamePad.GetState((PlayerIndex)(_player - 1));

        if (!gamePadState.IsConnected)
            return 0f;

        switch (_axis)
        {

            case eAxis.HORIZONTAL:
                return gamePadState.ThumbSticks.Left.X;
            case eAxis.VERTICAL:
                return gamePadState.ThumbSticks.Left.Y;
        }

        return 0f;
    }

    // ======================================================================================
    public static void VibrateController(int _player, float _leftForce, float _rightForce)
    {
#if UNITY_EDITOR
        Debug.Assert(m_manager != null, "InputMgr - Missing InputMgrConfig in Project");
#endif

        if (_player > 4 || _player <= 0)
            return;

        GamePad.SetVibration((PlayerIndex) _player - 1, _leftForce, _rightForce);
    }

    // ======================================================================================
    // PRIVATE METHODS
    // ======================================================================================
    private static bool GetButton(GamePadState _gamePadState, eXBoxButton _xboxButton)
    {
#if UNITY_EDITOR
        Debug.Assert(m_manager != null, "InputMgr - Missing InputMgrConfig in Project");
#endif
        if (!_gamePadState.IsConnected)
            return false;

        switch (_xboxButton)
        {
            // TRIGGERS AS BUTTONS
            case eXBoxButton.TRIGG_LEFT:
                return _gamePadState.Triggers.Left > m_manager.m_triggMinRatio;
            case eXBoxButton.TRIGG_RIGHT:
                return _gamePadState.Triggers.Right > m_manager.m_triggMinRatio;

            // BUTTONS
            case eXBoxButton.A:
                return _gamePadState.Buttons.A == ButtonState.Pressed;
            case eXBoxButton.B:
                return _gamePadState.Buttons.B == ButtonState.Pressed;
            case eXBoxButton.X:
                return _gamePadState.Buttons.X == ButtonState.Pressed;
            case eXBoxButton.Y:
                return _gamePadState.Buttons.Y == ButtonState.Pressed;
            case eXBoxButton.BUMPR_LEFT:
                return _gamePadState.Buttons.LeftShoulder == ButtonState.Pressed;
            case eXBoxButton.BUMPR_RIGHT:
                return _gamePadState.Buttons.RightShoulder == ButtonState.Pressed;
            case eXBoxButton.STICK_LEFT:
                return _gamePadState.Buttons.LeftStick == ButtonState.Pressed;
            case eXBoxButton.STICK_RIGHT:
                return _gamePadState.Buttons.RightStick == ButtonState.Pressed;
            case eXBoxButton.START:
                return _gamePadState.Buttons.Start == ButtonState.Pressed;
            case eXBoxButton.OPTIONS:
                return _gamePadState.Buttons.Guide == ButtonState.Pressed;
            case eXBoxButton.DPAD_UP:
                return _gamePadState.DPad.Up == ButtonState.Pressed;
            case eXBoxButton.DPAD_DOWN:
                return _gamePadState.DPad.Down == ButtonState.Pressed;
            case eXBoxButton.DPAD_LEFT:
                return _gamePadState.DPad.Left == ButtonState.Pressed;
            case eXBoxButton.DPAD_RIGHT:
                return _gamePadState.DPad.Right == ButtonState.Pressed;
        }

        return false;
    }


    // ======================================================================================
    private static bool GetDebugButton (eButton _button)
    {
        switch (_button)
        {
            case eButton.ATTACK:
                return Input.GetKey(KeyCode.A);
            case eButton.DASH:
                return Input.GetKey(KeyCode.F);
            case eButton.GRAB:
                return Input.GetKey(KeyCode.D);
            case eButton.TOSS:
                return Input.GetKey(KeyCode.S);
            case eButton.JUMP:
                return Input.GetKey(KeyCode.Space);
        }

        return false;
    }

    // ======================================================================================
    private static float GetDebugAxis (eAxis _axis)
    {
        switch (_axis)
        {
            case eAxis.HORIZONTAL:
                return (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
            case eAxis.VERTICAL:
                return (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        }

        return 0f;
    }

    // ======================================================================================
    private void InitializeParams()
    {
        // global
        m_triggMinRatio     = m_configData.m_triggMinRatio;
        m_pcDebugMode       = m_configData.m_pcDebugMode;

        // locomotion
        m_dashButton        = m_configData.m_dashButton;
        m_jumpButton        = m_configData.m_jumpButton;
        m_tossButton        = m_configData.m_tossButton;
        m_attackButton      = m_configData.m_attackButton;
        m_grabButton        = m_configData.m_grabButton;

        // menu
        m_submitButton      = m_configData.m_submitButton;
        m_previousButton    = m_configData.m_previousButton;
        m_pauseButton       = m_configData.m_pauseButton;
        m_changeColorButton = m_configData.m_changeColorButton;
    }
}
