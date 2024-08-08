using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float strength = 5;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float maxXPositionValue;
    [SerializeField] private float sideWaysMovmentSpeed;
    [SerializeField] private float maxHeight;

    public event EventHandler onPlayerTouchASpickeOrDeathCollider;

    Vector3 birdDirection;
    bool isAlive = true;
    float xDirection;

    private void Start()
    {
        xDirection = Mathf.Sign(UnityEngine.Random.Range(-1f, 1f));
    }

    private void Update()
    {

        if (!isAlive) { return; }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ApplyYStrengh();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ApplyYStrengh();
        }

        birdDirection.y += gravity * Time.deltaTime;

        birdDirection.x += xDirection * sideWaysMovmentSpeed * Time.deltaTime;

        if (transform.position.x >= maxXPositionValue)
        {
            xDirection = -1f;
        }
        else if (transform.position.x <= -maxXPositionValue)
        {
            xDirection = 1f;
        }

        transform.position += birdDirection * Time.deltaTime;
    }

    private void ApplyYStrengh()
    {
        if (transform.position.y >= maxHeight)
        {
            return;
        }

        birdDirection = Vector3.up * strength;
    }

    public float GetXDirection()
    {
        return xDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathCollider") || collision.gameObject.CompareTag("Spike"))
        {
            onPlayerTouchASpickeOrDeathCollider?.Invoke(this, null);
        }
    }
}
