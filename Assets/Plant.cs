using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Sprite plant1;
    public Sprite plant1_1;
    public Sprite plant2;
    public Sprite plant2_1;
    public Sprite plant3;
    public Sprite plant3_1;
    public Sprite plant4;

    public AudioSource audioSource;
    public AudioClip softSound;
    public AudioClip pluckSound;

    SpriteRenderer spriteRenderer;
    public int phase = 1;
    ShipController ship;
    public List<Vector2> lastPositions = new List<Vector2>();
    public List<Vector3> lastRotations = new List<Vector3>();

    float startAngle = 0;
    public float noise;

    public Transform homeBase;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = plant1;
        Invoke("IncrementPhase", 2);
        noise = Random.Range(-15, 15);
    }

    // Update is called once per frame
    void Update()
    {
        lastPositions.Add(transform.position);
        lastRotations.Add(transform.eulerAngles);
        if (lastPositions.Count > 2)
        {
            lastPositions.RemoveAt(0);
            lastRotations.RemoveAt(0);
        }

        if (homeBase != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, homeBase.position, 10 * Time.deltaTime);
            if (Vector2.Distance(transform.position, homeBase.position) < 0.1f)
            {
                audioSource.PlayOneShot(softSound);
                Destroy(gameObject, 0.2f);
            }
        }
    }

    void IncrementPhase()
    {
        phase++;
        switch (phase)
        {
            case 1:
                spriteRenderer.sprite = plant1;
                break;
            case 2:
                spriteRenderer.sprite = plant1_1;
                break;
            case 3:
                spriteRenderer.sprite = plant2;
                break;
            case 4:
                spriteRenderer.sprite = plant2_1;
                break;
            case 5:
                spriteRenderer.sprite = plant3;
                break;
            case 6:
                spriteRenderer.sprite = plant3_1;
                break;
            case 7:
                spriteRenderer.sprite = plant3_1;
                break;
            case 8:
                spriteRenderer.sprite = plant3_1;
                break;
            default:
                spriteRenderer.sprite = plant4;
                break;
        }
        if (phase <= 8)
        {
            Invoke("IncrementPhase", 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (ship != null)
        {
            return;
        }
        if (other.gameObject.layer == 7)
        {
            ShipController shipController = other.GetComponent<ShipController>();
            if (phase >= 9 && shipController.plants.Count < shipController.maxCargo)
            {
                GetComponent<HingeJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
                audioSource.PlayOneShot(pluckSound);
                ship = other.GetComponent<ShipController>();
                ship.plants.Add(this);
                ship.cargoText.text = $"Cargo: {ship.plants.Count}/{ship.maxCargo}";
            }
            // shake
            audioSource.PlayOneShot(softSound);
            startAngle = transform.eulerAngles.z;
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-5, 5));
            Invoke("Shake", 0.01f);
            Invoke("Shake", 0.02f);
            Invoke("Shake", 0.03f);
            Invoke("Shake", 0.04f);
            Invoke("Shake", 0.05f);
            Invoke("Shake", 0.06f);
            Invoke("Shake", 0.07f);
            Invoke("Shake", 0.08f);
            Invoke("Straighten", 0.09f);
        }
    }

    void Shake()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-7, 7));
    }

    void Straighten()
    {
        transform.eulerAngles = new Vector3(0, 0, startAngle);
    }
}
