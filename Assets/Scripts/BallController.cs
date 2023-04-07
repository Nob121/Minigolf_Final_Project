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
    private AudioSource audioSource;
    private int level = 1;
    private LineRenderer line;
    private float powerUpTime;
    private float power;
    private string[] scoreArray;
    private int hits;
    private float holeTime;
    private Vector3 spawnPoint;

    [SerializeField] private float maxPower;
    [SerializeField] private float changeAngleSpeed;
    [SerializeField] private float lineLength;
    [SerializeField] Slider powerSlider;
    [SerializeField] TextMeshProUGUI hitsCount;
    [SerializeField] TextMeshProUGUI resultLabel;
    [SerializeField] TextMeshProUGUI level1ScoreLabel;
    [SerializeField] TextMeshProUGUI level2ScoreLabel;
    [SerializeField] TextMeshProUGUI level3ScoreLabel;
    [SerializeField] TextMeshProUGUI finalScoreLabel1;
    [SerializeField] TextMeshProUGUI finalScoreLabel2;
    [SerializeField] TextMeshProUGUI finalScoreLabel3;
    [SerializeField] BasePopUp resultPopUp;
    [SerializeField] OptionsPopUp optionsPopUp;
    [SerializeField] OptionsPopUp gameOverPopUp;
    [SerializeField] BasePopUp hitPopup;
    [SerializeField] BasePopUp parPopUp;
    [SerializeField] private BasePopUp basePopUp;
    [SerializeField] private Transform level2pos;
    [SerializeField] private Transform level3pos;
    [SerializeField] private Transform level1pos;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip hitClip;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
        scoreArray = new string[0];
        putScore();
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
                if (power >= 0.5)
                {
                    audioSource.PlayOneShot(hitClip);
                }
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
            if(scoreArray.Length == 1)
            {
                level1ScoreLabel.text = scoreArray[0];
                level2ScoreLabel.text = "NA";
                level3ScoreLabel.text = "NA";
            }
            else if (scoreArray.Length == 2)
            {
                level1ScoreLabel.text = scoreArray[0];
                level2ScoreLabel.text = scoreArray[1];
                level3ScoreLabel.text = "NA";
            }
            else if(scoreArray.Length == 3)
            {
                level1ScoreLabel.text = scoreArray[0];
                level2ScoreLabel.text = scoreArray[1];
                level3ScoreLabel.text = scoreArray[2];
            }
            else if(scoreArray.Length == 0)
            {
                level1ScoreLabel.text = "NA";
                level2ScoreLabel.text = "NA";
                level3ScoreLabel.text = "NA";
            }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "OutOfBounds")
        {
            audioSource.PlayOneShot(errorClip);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            StartCoroutine(Respawn());
        }
        if (collision.collider.tag == "Hole")
        {
            string player = PlayerPrefs.GetString("PlayerName", "Player1");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            audioSource.PlayOneShot(successClip);
            Array.Resize(ref scoreArray, scoreArray.Length + 1);
            string score = Result(hits);
            scoreArray[scoreArray.Length - 1] = score;
            putScore();
            StartCoroutine(DestroyAfterSeconds(2f));
            StartCoroutine(DisableCollisionForSeconds(collision.collider, 2f));
            resultLabel.text = "Congratulations " + player + ", You got " + score.ToString();
            resultPopUp.Open();
            hits = 0;
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
        }
        else if(level == 3){ 
            transform.position = level3pos.position;
        }
        else
        {
            transform.position = level1pos.position;
        }
        if(level == 4)
        {
            putNumberWhenGameOver();
            gameOverPopUp.Open();
            hitPopup.Close();
            parPopUp.Close();
            Array.Resize(ref scoreArray, 0);
            putScore();
            level = 1;
            

        }
        line.enabled = true;
        resultPopUp.Close();
        hitsCount.text = hits.ToString();
    }
    private void putNumberWhenGameOver()
    {
        finalScoreLabel1.text = scoreArray[0];
        finalScoreLabel2.text = scoreArray[1];
        finalScoreLabel3.text = scoreArray[2];
    }
    private string Result(int hit)
    {
        string result;
        if(hit == 1)
        {
            result = "-1";
        }
        else if(hit == 2)
        {
            result = "0";
        }
        else
        {
            result = "+" + (hits - 2).ToString();
        }
        return result;
    }
}
