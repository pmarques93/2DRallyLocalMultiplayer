using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private float camRangeValue;
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

    private CinemachineVirtualCamera cam;
    private float camDistanceRefVariable;

    private bool pressingTurnLong;
    private float pressingTurnTime;
    [SerializeField] private TrailRenderer[] trailRender;
    [SerializeField] private ParticleSystem dust;
    private Rigidbody2D rb;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("p1Camera").GetComponent<CinemachineVirtualCamera>();

        cam.Follow = this.transform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        movingBackTimer = 0.3f;
        reduceMovingBackTimer = false;

        pressingTurnTime = 0f;
    }


    void FixedUpdate()
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

        // R
        if (acceleration < 0)
        {
            if (rb.velocity.magnitude < 2)
            {
                movingBack = true;

                rb.AddForce(frontDirection * acceleration * Time.fixedDeltaTime * speedVariable/4);

                if (turn != 0)
                {
                    transform.eulerAngles += new Vector3(0f, 0f, turn) * Time.fixedDeltaTime * (turnSpeedVariable / 2);
                }
            }
        }

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

    private void Update()
    {
        // Keys
        acceleration = Input.GetAxis("Vertical");
        turn = Input.GetAxisRaw("Horizontal");
        handBreak = Input.GetKey(KeyCode.Space);

        frontDirection = front.position - back.position;

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

        // Camera range
        if (rb.velocity.magnitude > camRangeValue)
        {
            cam.m_Lens.OrthographicSize = Mathf.SmoothDamp(cam.m_Lens.OrthographicSize, 6.5f, ref camDistanceRefVariable, 0.5f);
        }
        else
        {
            cam.m_Lens.OrthographicSize = Mathf.SmoothDamp(cam.m_Lens.OrthographicSize, 5, ref camDistanceRefVariable, 0.5f);
        }
    }
}
