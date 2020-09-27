using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineCamera : MonoBehaviour
{
    [SerializeField] private float camRangeValue;

    private float camDistanceRefVariable;

    private Player player;
    private CinemachineVirtualCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();

        player = cam.Follow.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Camera range
        if (player)
        {
            if (player.rb.velocity.magnitude > camRangeValue)
            {
                cam.m_Lens.OrthographicSize = Mathf.SmoothDamp(cam.m_Lens.OrthographicSize, 6.5f, ref camDistanceRefVariable, 0.5f);
            }
            else
            {
                cam.m_Lens.OrthographicSize = Mathf.SmoothDamp(cam.m_Lens.OrthographicSize, 5, ref camDistanceRefVariable, 0.5f);
            }
        }
    }
}
