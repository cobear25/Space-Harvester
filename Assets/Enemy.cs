using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform ship;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ship.GetComponent<ShipController>().paused) return;
        transform.Rotate(0, 0, Random.Range(-55, 55) * Time.deltaTime);
        if (ship != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, ship.position, 6 * Time.deltaTime);
        }
    }

    public void Kill()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        ship.GetComponent<ShipController>().CargoUp();
        Destroy(gameObject, 3);
    }
}
