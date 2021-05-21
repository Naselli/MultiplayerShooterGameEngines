using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using Mirror ;
using UnityEngine.UI ;

public class Player_Movement : NetworkBehaviour {
    [ SerializeField ] private Text healthText ;

    [ SerializeField ] private float      speed = 5f ;
    [ SerializeField ] private GameObject cam ;
    private                    Vector3    moveInput ;
    private                    Vector2    moveDir ;
    private                    Rigidbody  rB ;
    private                    GameObject projectile ;

    [ SyncVar ( hook = nameof ( DisplayHealth ) ) ]
    public int health = 100 ;

    private WeaponSpawner.WeaponTypes weaponHolding ;
    private int                       ammo ;

    private void Start ( ) {
        if ( ! isLocalPlayer ) return ;
        cam.SetActive ( true ) ;
        rB = GetComponent < Rigidbody > ( ) ;
    }

    void Update ( ) {
        if ( ! isLocalPlayer ) return ;

        #region Movement
        moveInput = Vector3.zero ;

        float moveX = Input.GetAxis ( "Horizontal" ) ;
        float moveY = Input.GetAxis ( "Vertical" ) ;
        if ( Input.GetKey ( KeyCode.W ) ) moveInput += cam.transform.forward ;
        if ( Input.GetKey ( KeyCode.S ) ) moveInput += - cam.transform.forward ;
        if ( Input.GetKey ( KeyCode.A ) ) moveInput += - cam.transform.right ;
        if ( Input.GetKey ( KeyCode.D ) ) moveInput += cam.transform.right ;

        transform.Rotate ( 0 , Input.GetAxis ( "Mouse X" ) , 0 ) ;
        cam.transform.Rotate ( Mathf.Clamp(Input.GetAxis ( "Mouse Y" ), -20, 20) , 0 , 0 ) ;
        #endregion

        #region CheckWeapon
        if ( Input.GetKeyDown ( KeyCode.Mouse0 ) ) {
            switch ( weaponHolding ) { }
        }
        #endregion
    }
    private void FixedUpdate ( ) {
        transform.position += new Vector3 ( moveInput.x , 0 , moveInput.z ).normalized * ( speed * Time.deltaTime ) ;
    }

    [ Command ] void CMD_SpawnProjectile ( ) {
        GameObject p = GameObject.Instantiate ( projectile , transform.position + new Vector3 ( 0 , 1 , 1.5f ) , Quaternion.identity ) ;
        NetworkServer.Spawn ( p ) ;
    }

    [ ServerCallback ] private void OnTriggerEnter ( Collider other ) {
        if ( gameObject.transform.childCount == 0 ) {
            switch ( other.gameObject.tag ) {
                case "Gun" :
                    weaponHolding = WeaponSpawner.WeaponTypes.NormalGun ;
                    ammo = 30 ;
                    break ;
                case "Dog" :
                    weaponHolding = WeaponSpawner.WeaponTypes.Walkingdog ;
                    ammo = 1 ;
                    break ;
                case "Grenade" :
                    weaponHolding = WeaponSpawner.WeaponTypes.Grenade ;
                    ammo = 2 ;
                    break ;
                case "Launcher" :
                    weaponHolding = WeaponSpawner.WeaponTypes.Launcher ;
                    ammo = 2 ;
                    break ;
            }
        }
    }

    public void TakeDamage ( )                                                  => health -= 20 ;
    public void DisplayHealth ( System.Int32 oldValue , System.Int32 newValue ) => healthText.GetComponent < Text > ( ).text = "" + newValue ;

    private Vector3 GetLookDirection ( Vector3 v ) => new Vector3 ( v.x , 0 , v.z ) ;
}