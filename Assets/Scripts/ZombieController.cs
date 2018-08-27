using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    public int zombieHealth = 3;
    private Animator anim;

    private float lastShootenTime = 0;
    public float shootTime =  0.5f;
    public float attackTime = 0.1f;
    private float lastAttackTime = 0;
    public float damage = 1;

    private bool isShooten, isDead = false;
    public bool isAttack = false, isWalk;

    private AudioSource audioSource;
    public AudioClip zombieDeadSound;

    private GameObject player, gameController;

    private MoveToPlayer moveToPlayer;

    public bool IsShooten
    {
        get { return isShooten; }
        set
        {
            isShooten = value;
            ShootenAnim(isShooten);
            UpdateShootenTime();
        }
    }

    void UpdateShootenTime()
    {
        lastShootenTime = Time.time;
    }

    void UpdateAttackTime()
    {
        lastAttackTime = Time.time;
    }

	// Use this for initialization
	void Start () {
        moveToPlayer = gameObject.GetComponent<MoveToPlayer>();
        anim = gameObject.GetComponent<Animator>();
        IsShooten = false;
        anim.SetBool("isDead", false);
        anim.SetBool("isWalk", true);
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");
	}

    void ShootenAnim(bool isShooten)
    {
        anim.SetBool("isShooten", isShooten);
    }

    void AttackAnim(bool isAttack)
    {
        anim.SetBool("isAttack", isAttack);
    }

    void WalkingAnim(bool isWalk)
    {
        anim.SetBool("isWalk", isWalk);
    }

    public void GetHit(int dmg)
    {
        if (isDead) return;
        IsShooten = true;
        zombieHealth -= dmg;
        audioSource.Play();
        if (zombieHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        isDead = true;
        audioSource.clip = zombieDeadSound;
        audioSource.Play();
        anim.SetBool("isDead", true);
        anim.SetBool("isWalk", false);
        gameController.GetComponent<GameController>().GetPoint(1);
        Destroy(gameObject, 2f);
    }

    void Attack()
    {
        if (Time.time >= (lastAttackTime + attackTime))
        {
            AttackAnim(true);
            WalkingAnim(false);
            UpdateAttackTime();
            player.GetComponent<PlayerController>().GetHit(damage);
        }
        else
        {
            AttackAnim(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (IsShooten && Time.time >= (lastShootenTime + shootTime))
        {
            IsShooten = false;
        }
        if (Vector3.Distance(transform.position, player.transform.position) > moveToPlayer.attackRange)
        {
            moveToPlayer.enabled = true;
            gameObject.GetComponent<Animator>().SetBool("isIdle", false);
            gameObject.GetComponent<Animator>().SetBool("isWalk", true);
            isWalk = true;
            isAttack = false;
        }
        if (isAttack)
        {
            Attack();
        }
	}
}
