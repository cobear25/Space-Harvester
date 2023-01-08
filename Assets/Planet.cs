using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject plantPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AddPlant", Random.Range(1, 9));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddPlant()
    {
        GameObject plant = Instantiate(plantPrefab);
        float angle = Random.Range(0, Mathf.PI * 2);
        float radius = transform.localScale.x / 2f;
        float xPos = radius * Mathf.Cos(angle);
        float yPos = radius * Mathf.Sin(angle);
        plant.transform.position = new Vector2(transform.position.x + xPos, transform.position.y + yPos);
        plant.transform.localScale = new Vector3(2, 2, 2);
        plant.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle);
        Invoke("AddPlant", Random.Range(1, 9));
    }
}
