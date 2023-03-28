using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    private float angle;
    [SerializeField] private float maxPower;
    [SerializeField] private float changeAngleSpeed;
    [SerializeField] private float lineLength;
    [SerializeField] Slider powerSlider;
    private LineRenderer line;
    private float powerUpTime;
    private float power;
    [SerializeField] TextMeshProUGUI hitsCount;
    private int hits;
    private float holeTime;
    //[SerializeField] private float minHoleTime;
    private Vector3 spawnPoint;
    //private MenuManager menuManager;
    [SerializeField] TextMeshProUGUI resultLabel;
    [SerializeField] ResultPopUp resultPopUp;
    //[SerializeField] private Camera camera;
    [SerializeField] private Transform level2pos;
    [SerializeField] private Transform level3pos;
    private float level = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
        
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 0.01f)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                angle -= Time.deltaTime * changeAngleSpeed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                angle += Time.deltaTime * changeAngleSpeed;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                PowerUp();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Shoot();
            }
            UpdateLine();
        }
        else
        {
            if (line)
            {
                line.enabled = false;
            }
        }
    }

    private void UpdateLine()
    {
        if (line)
        {
            if (holeTime == 0)
            {
                line.enabled = true;
            }
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
        }
    }
    private void Shoot()
    {
        spawnPoint = transform.position;
        rb.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * maxPower * power, ForceMode.Impulse);
        power = 0;
        powerSlider.value = 0;
        powerUpTime = 0;
        hits++;
        hitsCount.text = hits.ToString();
    }
    private void PowerUp()
    {
        powerUpTime += Time.deltaTime;
        power = Mathf.PingPong(powerUpTime, 1);
        powerSlider.value = power;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {
            holeTime = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "OutOfBounds")
        {
            transform.position = spawnPoint;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (collision.collider.tag == "Hole")
        {
            string player = PlayerPrefs.GetString("PlayerName", "Player1");
            //if (line) { Destroy(line); }
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            StartCoroutine(DestroyAfterSeconds(2f));
            resultLabel.text = "Congratulations " + player + ", You got " + Result();
            resultPopUp.Open();
            hits = 0;
            if(level == 1)
            {
                level = 2;
            }
            Debug.Log(level);
            
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        line.enabled = false;
        yield return new WaitForSeconds(seconds);
        if (level != 2)
        {
            transform.position = level3pos.position;
        }
        else
        {
            transform.position = level2pos.position;
            level++;
        }
        line.enabled = true;
    }

    private string Result()
    {
        if(hits == 1)
        {
            return "Hole in One";
        }
        else if(hits == 2)
        {
            return "Par";
        }
        else
        {
            return "Par +" + (hits - 2).ToString();
        }
    }
}
