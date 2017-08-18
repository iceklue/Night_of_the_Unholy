﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;


public class GameManager : NetworkBehaviour
{
    public static GameManager currentInstance;

    public class Players : SyncListStruct<Player.PlayerInfo>
    { }

    void PlayersChanged(SyncListStruct<Player.PlayerInfo>.Operation op, int itemIndex)
    {
        Debug.Log("Players changed:" + op);
    }

    public Players players; //The player list


    private void Awake()
    {
        if (currentInstance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            currentInstance = this;
        }
        if (players == null) players = new Players();
        players.Callback = PlayersChanged;
    }

    [Command]
    public void CmdZombieTargetRandomPlayer(NetworkInstanceId id) //return random player
    {
        if(players.Count > 0)
        {

            NetworkServer.FindLocalObject(id).GetComponent<Zombie>().target = NetworkServer.FindLocalObject(players[UnityEngine.Random.Range(0, players.Count)].netId);
            //return NetworkServer.FindLocalObject(players[UnityEngine.Random.Range(0, players.Count)].netId);
        }
        else
        {
            NetworkServer.FindLocalObject(id).GetComponent<Zombie>().target = null;
            //return null;
        }
    }

    public void AddPlayerToList(Player.PlayerInfo obj)
    {
        players.Add(obj);
    }

    [Command]
    void CmdAddPlayerToList(Player.PlayerInfo obj)
    {
        players.Add(obj);
        // this code is only executed on the server
       // RpcAddPlayerToList(obj); // invoke Rpc on all clients
    }

    [ClientRpc]
    void RpcAddPlayerToList(Player.PlayerInfo obj)
    {
        // this code is executed on all clients
        
    }

}


