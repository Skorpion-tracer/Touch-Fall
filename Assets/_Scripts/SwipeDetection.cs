﻿using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;
    [SerializeField] private GameObject trail;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private Coroutine coroutine;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        Debug.Log(inputManager.PrimaryPosition());
    }

    private void OnEnable()
    {
        inputManager.StartTouch += OnStartSwipe;
        inputManager.EndTouch += OnEndSwipe;
    }

    private void OnDisable()
    {
        inputManager.StartTouch -= OnStartSwipe;
        inputManager.EndTouch -= OnEndSwipe;
    }

    private void OnStartSwipe(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        Debug.Log(inputManager.PrimaryPosition());
        //trail.transform.position = position;
        //trail.SetActive(true);
        coroutine = StartCoroutine(Trail());
    }

    private void Update()
    {
        Debug.Log(inputManager.PrimaryPosition());
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void OnEndSwipe(Vector2 position, float time)
    {
        //trail.SetActive(false);
        StopCoroutine(coroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime)
        {
            Debug.Log("SwipeDetected");
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
        }
    }
}
