using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    public float speed = 0;
    public float rotateSpeed = 100;
    public float acceleration = 0.5f;
    public float maxSpeed = 10;

    public TextMeshProUGUI cargoText;
    public TextMeshProUGUI totalHarvestText;
    public RectTransform speedMeter;
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public GameObject gameOverPanel;
    public AudioClip pewSound;
    public AudioClip explosionSound;

    public int maxCargo = 20;
    public int totalHarvest = 0;
    Rigidbody2D rb;
    public List<Vector2> lastPositions = new List<Vector2>();
    public List<Vector3> lastRotations = new List<Vector3>();
    public Vector2 lastPos = Vector2.zero;
    public List<Plant> plants = new List<Plant>();

    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        lastPos = transform.position;
        Invoke("AddEnemy", 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            speed += Time.deltaTime * acceleration;
            speedMeter.localScale = new Vector2(1, speed / maxSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            speed -= Time.deltaTime * acceleration;
            speedMeter.localScale = new Vector2(1, speed / maxSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position + (transform.right * 0.5f);
            bullet.transform.eulerAngles = transform.eulerAngles;
            GetComponent<AudioSource>().PlayOneShot(pewSound);
        }
        if (speed > 0) 
        {
            speed = Mathf.Min(maxSpeed, speed);
        }
        else if (speed < 0)
        {
            speed = Mathf.Max(-maxSpeed, speed);
        }
        transform.position += transform.right * Time.deltaTime * speed;
        if (transform.position.x > 100)
        {
            transform.position = new Vector2(-100, transform.position.y);
        }
        if (transform.position.x < -100)
        {
            transform.position = new Vector2(100, transform.position.y);
        }
        if (transform.position.y > 100)
        {
            transform.position = new Vector2(transform.position.x, -100);
        }
        if (transform.position.y < -100)
        {
            transform.position = new Vector2(transform.position.x, 100);
        }
        // rb.velocity = transform.right * Time.deltaTime * speed * 100;
        lastPos = transform.position;
        lastPositions.Add(transform.position);
        lastRotations.Add(transform.eulerAngles);
        if (lastPositions.Count > 2)
        {
            lastPositions.RemoveAt(0);
            lastRotations.RemoveAt(0);
        }
        for (int i = 0; i < plants.Count; i++)
        {
            if (i == 0)
            {
                plants[i].transform.position = lastPositions[0];
                plants[i].transform.eulerAngles = new Vector3(0, 0, lastRotations[0].z - 180 + plants[i].noise);
            }
            else 
            {
                plants[i].transform.position = plants[i - 1].lastPositions[0];
                plants[i].transform.eulerAngles = new Vector3(0, 0, plants[i - 1].lastRotations[0].z + plants[i].noise);
            }
        }
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            horizontal = 0;
        }
        if (horizontal > 0)
        {
            transform.Rotate(0, 0, -1 * rotateSpeed * Time.deltaTime, Space.Self);
        }
        else if (horizontal < 0)
        {
            transform.Rotate(0, 0, 1 * rotateSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void UnloadCargo(Transform homeBase)
    {
        foreach (Plant plant in plants)
        {
            plant.homeBase = homeBase; 
        }
        totalHarvest += plants.Count;
        totalHarvestText.text = $"Total Harvest: {totalHarvest}";
        plants.Clear();
    }

    public void AddEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.ship = transform;
        enemy.transform.position = new Vector2(Random.Range(-150, 150), Random.Range(-150, 150));
        Invoke("AddEnemy", 10);
    }

    public void CargoUp()
    {
        maxCargo += 5;
        cargoText.text = $"Cargo: {plants.Count}/{maxCargo}";
        GetComponent<AudioSource>().PlayOneShot(explosionSound);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 9)
        {
            dead = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ParticleSystem>().Play();
            GetComponent<PolygonCollider2D>().enabled = false;
            other.gameObject.GetComponent<Enemy>().Kill();
            GetComponent<AudioSource>().PlayOneShot(explosionSound);
            Invoke("ShowGameOver", 1);
        }
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true); 
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
