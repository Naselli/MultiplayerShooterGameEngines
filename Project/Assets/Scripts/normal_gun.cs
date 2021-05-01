using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class normal_gun : NetworkBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0,0,1000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent< Player_Movement >( ).TakeDamage( );
        NetworkServer.Destroy(gameObject);
    }
}
