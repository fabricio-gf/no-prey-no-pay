﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectInputController : MenuInputController {

	private bool[] m_submitTriggerList = new bool[4];
	private bool[] m_previousTriggerList = new bool[4];
	private bool[] m_pauseTriggerList = new bool[4];
	private bool[] m_changeColorTriggerList = new bool[4];

	void Start(){
		for(int i = 0; i < 4; i++){
			m_submitTriggerList[i] = false;
			m_previousTriggerList[i] = false;
			m_pauseTriggerList[i] = false;
			m_changeColorTriggerList[i] = false;

		}
	}

	public override void Update()
    {
		for(int i = 0; i < 4; i++){
            // reset triggers when button released
            m_submitTriggerList[i]     = !InputMgr.GetMenuButton(i + 1, InputMgr.eMenuButton.SUBMIT)      || m_submitTriggerList[i];
            m_previousTriggerList[i]   = !InputMgr.GetMenuButton(i + 1, InputMgr.eMenuButton.PREVIOUS)    || m_previousTriggerList[i];
            m_pauseTriggerList[i]      = !InputMgr.GetMenuButton(i + 1, InputMgr.eMenuButton.PAUSE)       || m_pauseTriggerList[i];
            m_changeColorTriggerList[i] = !InputMgr.GetMenuButton(i + 1, InputMgr.eMenuButton.CHANGE_COLOR) || m_changeColorTriggerList[i];
		}
	}
	    
    public bool GetSubmit(int player)
    {
        if (m_submitTriggerList[player] && InputMgr.GetMenuButton(player + 1, InputMgr.eMenuButton.SUBMIT))
        {
            m_submitTriggerList[player] = false;
            return true;
        }

        return false;
    }

	
    public bool GetPrevious(int player)
    {
        if (m_previousTriggerList[player] && InputMgr.GetMenuButton(player + 1, InputMgr.eMenuButton.PREVIOUS))
        {
            m_previousTriggerList[player] = false;
            return true;
        }

        return false;
    }
	
    public bool GetPause(int player)
    {
        if (m_pauseTriggerList[player] && InputMgr.GetMenuButton(player + 1, InputMgr.eMenuButton.PAUSE))
        {
            m_pauseTriggerList[player] = false;
            return true;
        }

        return false;
    }


    public bool GetChangeColor(int player)
    {
        if (m_changeColorTriggerList[player] && InputMgr.GetMenuButton(player + 1, InputMgr.eMenuButton.CHANGE_COLOR))
        {
            m_changeColorTriggerList[player] = false;
            return true;
        }

        return false;
    }
}
