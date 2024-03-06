using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPng : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    private Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        if ((player.position - transform.position).magnitude > 5)
        {
            moveDir = (player.position - transform.position).normalized;
            transform.position += moveDir * speed * Time.deltaTime;
            transform.forward = moveDir;
        }

    }
}
