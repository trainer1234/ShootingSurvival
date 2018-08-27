using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public int dmg = 1;

    public float fireTime = 0.1f;
    private float lastFireTime = 0;
    public float playerHealth = 10;
    public float regenerateSpeed = 0.001f;

    private float playerCurrentHealth;
    public GameObject smoke, gunHead, playerBlood;
    private GameObject gameController;
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip playerDeadSound;
    public Slider healthBar;

    private CharacterController charController;
    public float speed = 2;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
	    UpdateFireTime();
        gunHead.GetComponent<Light>().enabled = true;
        audioSource = gameObject.GetComponent<AudioSource>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerCurrentHealth = playerHealth;
        healthBar.maxValue = playerHealth;
        healthBar.value = playerCurrentHealth;
        healthBar.minValue = 0;
	}

    void UpdateFireTime()
    {
        lastFireTime = Time.time;
    }

    void SetFireAnim(bool isFire)
    {
        anim.SetBool("isShoot", isFire);
    }

    public void GetHit(float dmg)
    {
        InsBlood();
        audioSource.Play();
        playerCurrentHealth -= dmg;
        healthBar.value = playerCurrentHealth;

        if (playerCurrentHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        PlayDeadSound();
        gameController.GetComponent<GameController>().EndGameLose();
    }

    public void PlayDeadSound()
    {
        audioSource.clip = playerDeadSound;
        audioSource.Play();
        //audioSource.PlayOneShot(audioSource.clip);
    }

    void Fire()
    {
        if(Time.time >= (lastFireTime + fireTime)){       
#if UNITY_IOS || UNITY_ANDROID
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // must have collider so the ray point to screen can reflect back
            if (Physics.Raycast(ray, out hit))
            {
                SetFireAnim(true);
                InsSmoke();
                // golden headshot
                if (hit.transform.CompareTag("ZombieHead"))
                {
                    int zomHealth = hit.transform.parent.gameObject.GetComponent<ZombieController>().zombieHealth;
                    //Debug.Log(zombieHealth);
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(zomHealth);
                    //Destroy(hit.transform.parent.gameObject);
                    //Destroy(hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("HeartZombieHead"))
                {
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(dmg+1);
                }
                else if (hit.transform.CompareTag("OrangeZombieHead"))
                {
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(dmg + 2);
                }
                else if (hit.transform.CompareTag("Zombie"))
                {
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(dmg);
                }
            }
#else
            RaycastHit hit;
            // must have collider so the ray point to screen can reflect back
            if (Physics.Raycast(gunHead.transform.position, gunHead.transform.forward, out hit))
            {
                SetFireAnim(true);
                InsSmoke();
                // golden headshot
                if (hit.transform.CompareTag("ZombieHead"))
                {
                    int zomHealth = hit.transform.parent.gameObject.GetComponent<ZombieController>().zombieHealth;
                    //Debug.Log(zombieHealth);
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(zomHealth);
                    //Destroy(hit.transform.parent.gameObject);
                    //Destroy(hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("HeartZombieHead"))
                {
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(dmg+1);
                }
                else if (hit.transform.CompareTag("OrangeZombieHead"))
                {
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(dmg + 2);
                }
                else if (hit.transform.CompareTag("Zombie")
                    || hit.transform.CompareTag("Spider"))
                {
                    hit.transform.parent.gameObject.GetComponent<ZombieController>().GetHit(dmg);
                }
            }
#endif
            UpdateFireTime();
        }
        else
        {
            SetFireAnim(false);
        }
    }

    void InsSmoke()
    {
        GameObject sm = Instantiate(smoke, gunHead.transform.position, gunHead.transform.rotation) as GameObject;
        Destroy(sm, 1f);
    }

    void InsBlood()
    {
        GameObject blood = Instantiate(playerBlood, gunHead.transform.position, gunHead.transform.rotation) as GameObject;
        Destroy(blood, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            Fire();
        }
        StartCoroutine(addHealth());
	}

    IEnumerator addHealth()
    {
        while (true)
        { // loops forever...
            if (playerCurrentHealth < playerHealth)
            { 
                playerCurrentHealth += regenerateSpeed; // increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // just yield 
                yield return null;
            }
            healthBar.value = playerCurrentHealth;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = Camera.main.transform.TransformDirection(movement);
        charController.Move(movement);
        
        //rb.AddForce(movement * speed);
    }
}
