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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
    }

    private void Update()
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

    private void UpdateLine()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }
    private void Shoot()
    {
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
}
