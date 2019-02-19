using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public enum EnemyState {
        Patrol,
        Chase
    }

    public EnemyState currentState;

    private List<Vector3> pathPositions;
    private int currentPosition = 0;
    private int nextPosition;
    public Transform path;
    public float walkSpeed = 0.7f;
    public float margen = 0.5f;
    //private bool forwardMovement = true;
    private int direction = 1;
    public Transform player;
    public bool PlayerDetected;

    // Start is called before the first frame update
    void Start()
    {
        path = transform.parent.Find("Path");
        currentState = EnemyState.Patrol;
        pathPositions = new List<Vector3>();

        for (int i = 0; i < path.childCount; i++) {
            pathPositions.Add(path.GetChild(i).position);
        }
        if (pathPositions.Count > 1) {
            nextPosition = currentPosition + 1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState) {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
        }
    }

    private void Patrol() {
        int side = transform.position.x < pathPositions[nextPosition].x ? 1 : -1;

        transform.Translate(new Vector3(side * Time.deltaTime * walkSpeed, 0, 0));
        UpdateCurrentPosition();
        if (PlayerDetected) {
            currentState = EnemyState.Chase;
        }
        
    }

    private void Chase() {
        int side = transform.position.x > player.transform.position.x ? -1 : 1;

        transform.Translate(new Vector3(side * Time.deltaTime * walkSpeed, 0, 0));
        if (!PlayerDetected) {
            currentState = EnemyState.Patrol;
        }
    }

    private void UpdateCurrentPosition() {
        if (Vector3.Distance(transform.position, pathPositions[nextPosition]) <= margen) {
            CalculateNextPosition();
        }
    }

    private void CalculateNextPosition() {
        currentPosition = nextPosition;

        //if (forwardMovement) {
        //    if (currentPosition == pathPositions.Count - 1) {
        //        forwardMovement = false;
        //        nextPosition--;
        //    } else {
        //        nextPosition++;
        //    }
        //} else {
        //    if (currentPosition == pathPositions.Count - 1) {
        //        forwardMovement = true;
        //        nextPosition++;
        //    }
        //    else {
        //        nextPosition--;
        //    }
        //}

        if (currentPosition == pathPositions.Count - 1 || currentPosition == 0) {
            direction = -direction;
            print("Invertimos dirección (" + direction + ")");
        }
        nextPosition += direction;
    }
}


