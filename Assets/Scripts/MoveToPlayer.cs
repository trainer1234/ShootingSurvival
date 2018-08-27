using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour {
    float moveSpeed;
    public float minMoveSpeed = 0.05f;
    public float maxMoveSpeed = 0.3f;
    public float attackRange = 3;
    private bool enabled = true;

    GameObject player, lookAtTarget;
    private Transform transform;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        lookAtTarget = GameObject.FindGameObjectWithTag("LookTarget");
        transform = GetComponent<Transform>();
        UpdateMoveSpeed();
	}

    void UpdateMoveSpeed()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        if (player == null || lookAtTarget == null) return;
        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            transform.LookAt(lookAtTarget.transform.position);
            transform.position = Vector3.Lerp(transform.position,
               player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            enabled = false;
            gameObject.GetComponent<Animator>().SetBool("isIdle", true);
            gameObject.GetComponent<ZombieController>().isAttack = true;
            gameObject.GetComponent<ZombieController>().isWalk = false;
            gameObject.GetComponent<MoveToPlayer>().enabled = false;
        }
    }
}
