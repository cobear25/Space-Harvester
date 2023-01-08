using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * 15;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);    
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 9)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.Kill();
        }
    }
}
