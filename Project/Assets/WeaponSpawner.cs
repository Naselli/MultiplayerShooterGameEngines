using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public enum WeaponTypes {
        NormalGun,
        Launcher,
        Walkingdog,
        Grenade,
        
    }

    [SerializeField] private WeaponTypes typeOfWeapon;

    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject launcherPrefab;
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private GameObject grenadePrefab;
    
    private                  GameObject objectToSpawn;

    [SerializeField] private int  spawnTimer;
    private                  bool isSpawning = false;
    
    
    void Start()
    {
        switch ( typeOfWeapon ) {
            case WeaponTypes.NormalGun:
                GameObject gun = Instantiate( gunPrefab , transform.position , Quaternion.identity );
                NetworkServer.Spawn(gun);
                gun.transform.parent = gameObject.transform;
                objectToSpawn = gunPrefab;
                break;
            case WeaponTypes.Launcher:
                GameObject launcher = Instantiate( launcherPrefab , transform.position , Quaternion.identity );
                NetworkServer.Spawn(launcher);
                launcher.transform.parent = gameObject.transform;
                objectToSpawn = launcher;
                break;
            case WeaponTypes.Walkingdog:
                GameObject dog = Instantiate( dogPrefab , transform.position , Quaternion.identity );
                NetworkServer.Spawn(dog);
                dog.transform.parent = gameObject.transform;
                objectToSpawn = dogPrefab;
                break;
            case WeaponTypes.Grenade:
                GameObject grenade = Instantiate( grenadePrefab , transform.position , Quaternion.identity );
                NetworkServer.Spawn(grenade);
                grenade.transform.parent = gameObject.transform;
                objectToSpawn = grenade;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( transform.childCount >= 0 && !isSpawning  ) {
            StartCoroutine( SpawnOverTime( ) );
        }
    }

    private IEnumerator SpawnOverTime() {
        isSpawning = true;
        yield return new WaitForSeconds( spawnTimer );
        GameObject g = Instantiate( objectToSpawn , transform.position , Quaternion.identity );
        NetworkServer.Spawn( g );
        isSpawning = false;
    }
}
