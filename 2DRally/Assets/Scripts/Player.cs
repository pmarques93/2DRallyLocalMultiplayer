﻿using UnityEngine;
using Cinemachine;
using static UnityEngine.InputSystem.InputAction;


public class Player : MonoBehaviour
{
    [SerializeField] private float speedVariable;
    [SerializeField] private float turnSpeedVariable;
    private float handBreak;
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

    [SerializeField] private LayerMask roughtTerrainLayer;

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
        frontDirection = front.position - back.position;

        MovingBackTimerMethod();

    }

    public void Turn(CallbackContext context)
    {
        turn = context.ReadValue<Vector2>().x;
    }
    public void GasReverse(CallbackContext context)
    {
        acceleration = context.ReadValue<Vector2>().y;
    }
    public void Break(CallbackContext context)
    {
        handBreak = context.ReadValue<Vector2>().y;
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
        if (handBreak > 0.1f)
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

            if (rb.velocity.magnitude > 2)
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
            if (acceleration > -0.1f)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            rb.mass = 2.2f;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            rb.mass = 1f;
        }
    }
}
