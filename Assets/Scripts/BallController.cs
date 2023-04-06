using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    [SerializeField] TextMeshProUGUI level1ScoreLabel;
    [SerializeField] TextMeshProUGUI level2ScoreLabel;
    [SerializeField] TextMeshProUGUI level3ScoreLabel;
    [SerializeField] BasePopUp resultPopUp;
    [SerializeField] OptionsPopUp optionsPopUp;
    [SerializeField] BasePopUp hitPopup;
    [SerializeField] BasePopUp parPopUp;
    [SerializeField] private BasePopUp basePopUp;
    [SerializeField] private Transform level2pos;
    [SerializeField] private Transform level3pos;
    private int level = 1;
    private int[] scoreArray;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
        scoreArray = new int[0];
        
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionsPopUp.Open();
                hitPopup.Close();
                parPopUp.Close();
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
    private void putScore()
    {
        for (int i = 0; i < scoreArray.Length; i++)
        {
            if(scoreArray.Length == 1)
            {
                level1ScoreLabel.text = scoreArray[0].ToString();
                level2ScoreLabel.text = "N/A";
                level3ScoreLabel.text = "N/A";
            }
            else if (scoreArray.Length == 2)
            {
                level1ScoreLabel.text = scoreArray[0].ToString();
                level2ScoreLabel.text = scoreArray[1].ToString();
                level3ScoreLabel.text = "N/A";
            }
            else if(scoreArray.Length == 3)
            {
                level1ScoreLabel.text = scoreArray[0].ToString();
                level2ScoreLabel.text = scoreArray[1].ToString();
                level3ScoreLabel.text = scoreArray[2].ToString();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "OutOfBounds")
        {
            //basePopUp.Open();
            //transform.position = spawnPoint;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            StartCoroutine(Respawn());
        }
        if (collision.collider.tag == "Hole")
        {
            string player = PlayerPrefs.GetString("PlayerName", "Player1");
            //if (line) { Destroy(line); }
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Array.Resize(ref scoreArray, scoreArray.Length + 1);
            int score = Result(hits);
            scoreArray[scoreArray.Length - 1] = score;
            putScore();
            StartCoroutine(DestroyAfterSeconds(2f));
            StartCoroutine(DisableCollisionForSeconds(collision.collider, 2f));
           // Debug.Log("Hits: " + hits);
            resultLabel.text = "Congratulations " + player + ", You got " + score.ToString();
            resultPopUp.Open();
            hits = 0;
           // Debug.Log(level);
            level++;
            
        }
    }
    IEnumerator Respawn()
    {
        basePopUp.Open();
        yield return new WaitForSeconds(2f);
        transform.position = spawnPoint;
        basePopUp.Close();

    }
    
    IEnumerator DisableCollisionForSeconds(Collider collider, float duration)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(duration);
        collider.enabled = true;
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        line.enabled = false;
        yield return new WaitForSeconds(seconds);
        if(level == 2)
        { 
            transform.position = level2pos.position;
            hits = 0;
        }
        else { 
            transform.position = level3pos.position;
        }
        line.enabled = true;
        resultPopUp.Close();
        hitsCount.text = hits.ToString();
    }

    private int Result(int hit)
    {
        int result;
        if(hit == 1)
        {
            result = -1;
        }
        else if(hit == 2)
        {
            result = 0;
        }
        else
        {
            result = hits - 2;
        }
        return result;
    }
}
