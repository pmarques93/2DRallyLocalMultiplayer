using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;

public class Player_original : MonoBehaviour
{
    
    [SerializeField] private float speedVariable;
    [SerializeField] private float turnSpeedVariable;
    private bool handBreak;
    private bool movingBack;
    private bool reduceMovingBackTimer;
    private float movingBackTimer;

    private float acceleration;
    private float turn;
    private Vector2 frontDirection;

    [SerializeField] Transform back;
    [SerializeField] Transform front;
    [SerializeField] Transform leftRear;
    [SerializeField] Transform rightRear;


    private bool pressingTurnLong;
    private float pressingTurnTime;
    [SerializeField] private TrailRenderer[] trailRender;
    [SerializeField] private ParticleSystem dust;
    [HideInInspector] public Rigidbody2D rb;

    [SerializeField] private CinemachineVirtualCamera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CreateCamera();
        

        movingBackTimer = 0.3f;
        reduceMovingBackTimer = false;

        pressingTurnTime = 0f;
    }


    void FixedUpdate()
    {
        Forward();
        Backwards();
        HandBreak();
        TrailEffect();
    }

    private void Update()
    {
        // Keys
        acceleration = Input.GetAxis("p1_Vertical");
        turn = Input.GetAxisRaw("p1_Horizontal");
        handBreak = Input.GetButton("p1_Fire1");
        frontDirection = front.position - back.position;

        MovingBackTimerMethod();
    }


    private void CreateCamera()
    {
        CinemachineVirtualCamera myCamera = Instantiate(cam, cam.transform.position, cam.transform.rotation);

        CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        myCamera.gameObject.layer = 24 + allCameras.Length;
        myCamera.Follow = this.transform;
    }

    private void Forward()
    {
        // Forward
        if (acceleration > 0)
        {
            rb.AddForce(frontDirection * acceleration * Time.fixedDeltaTime * speedVariable);
        }

        if (rb.velocity.magnitude > 3f && movingBack == false)
        {
            if (turn != 0)
            {
                transform.eulerAngles += new Vector3(0f, 0f, -turn) * Time.fixedDeltaTime * turnSpeedVariable;
            }
        }
    }

    private void Backwards()
    {
        // R
        if (acceleration < 0)
        {
            if (rb.velocity.magnitude < 2)
            {
                movingBack = true;

                rb.AddForce(frontDirection * acceleration * Time.fixedDeltaTime * speedVariable / 4);

                if (turn != 0)
                {
                    transform.eulerAngles += new Vector3(0f, 0f, turn) * Time.fixedDeltaTime * (turnSpeedVariable / 2);
                }
            }
        }
    }

    private void HandBreak()
    {
        // Handbreak
        if (handBreak)
        {
            if (rb.velocity.magnitude > 5)
            {
                if (turn == 1)
                {
                    rb.AddForce((leftRear.position - rightRear.position) * 2f * Time.fixedDeltaTime * speedVariable);
                    rb.drag += 1.5f * Time.fixedDeltaTime;
                }
                if (turn == -1)
                {
                    rb.AddForce((rightRear.position - leftRear.position) * 2f * Time.fixedDeltaTime * speedVariable);
                    rb.drag += 0.1f;
                }
                if (turn == 0)
                {
                    rb.drag = 6f;
                }
            }

            if (acceleration != 0)
            {
                dust.Play();
                dust.enableEmission = true;
            }
            else
            {
                dust.enableEmission = false;
            }

        }
        else
        {
            dust.enableEmission = false;

            rb.drag = 2.8f;
        }
    }

    private void TrailEffect()
    {
        // Trail on max turn
        if (turn == 1 || turn == -1)
        {
            pressingTurnLong = true;

        }
        else
        {
            pressingTurnLong = false;
        }

        if (pressingTurnLong)
        {
            pressingTurnTime += Time.fixedDeltaTime;
        }

        if (pressingTurnTime > 0.3f)
        {
            foreach (TrailRenderer trail in trailRender)
            {
                trail.emitting = true;
            }
        }
        if (turn == 0)
        {
            pressingTurnTime = 0f;
            foreach (TrailRenderer trail in trailRender)
            {
                trail.emitting = false;
            }
        }
    }

    private void MovingBackTimerMethod()
    {
        // Timer since key was pressed
        if (movingBack)
        {
            if (Input.GetButtonUp("Vertical"))
            {
                reduceMovingBackTimer = true;
            }
        }
        if (reduceMovingBackTimer)
        {
            movingBackTimer -= Time.deltaTime;
        }
        if (movingBackTimer <= 0)
        {
            movingBack = false;
            reduceMovingBackTimer = false;
            movingBackTimer = 0.3f;
        }
    }
}
