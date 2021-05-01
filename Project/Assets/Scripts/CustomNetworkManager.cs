using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private Transform[] spawnpoints;
    private                  int         playerIndex;

    
    private void Update( ) {
        
    }

    public override void OnServerAddPlayer( NetworkConnection conn ) {
        GameObject player = Instantiate( playerPrefab );
        player.transform.position = spawnpoints[ playerIndex ].position;
        NetworkServer.AddPlayerForConnection( conn , player );
        playerIndex++;
    }

    public override void OnServerDisconnect( NetworkConnection conn ) {
        playerIndex--;
    }
    
}
