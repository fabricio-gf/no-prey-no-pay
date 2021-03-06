﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReferee : MonoBehaviour {

    // SINGLETON
	public static RoundReferee instance;

    // PUBLIC ATTRIBUTES
	public float StockLimit;

    // PRIVATE ATTRIBUTES
	[SerializeField] private ScoreDisplay Display;
	[SerializeField] private GameObject VictoryWindow;

    private HashSet<int> alivePlayers;

    public void Awake(){
        // init singleton
        Debug.Assert(instance == null, this.gameObject.name + " - RoundReferee : must be unique!");
        instance = this;
    }

	public static void StartRound(){
		print("ROUND STARTED");

        GameMgr.PlayGame();

        instance.alivePlayers = new HashSet<int>();
        foreach (GameObject player in PlayerSpawner.SpawnnedPlayers)
        {
            player.SendMessage("MSG_StartRound", instance); 
            instance.alivePlayers.Add((int) player.GetComponent<PlayerInputCtlr>().m_nbPlayer);
        }
	}
	
    public static void RegisterDeath(int _player)
    {
        instance.alivePlayers.Remove(_player);

        if (instance.alivePlayers.Count == 1){
            HashSet<int>.Enumerator it = instance.alivePlayers.GetEnumerator();
            it.MoveNext();
            EndRound(it.Current);
        }
    }

    public static void EndRound(int _player)
    {
        MatchReferee.instance.EndRound(_player);
    }
}
