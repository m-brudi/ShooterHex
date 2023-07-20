using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerImg;
    [SerializeField] float runSpeed = 20.0f;
    [SerializeField] Bomb bomb;
    [SerializeField] int health = 10;

    Rigidbody rb;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    bool right = true;
    bool canMove;

    public float Horizontal {
        get { return horizontal; }
    }
    public float Vertical {
        get { return vertical; }
    }

    public int Health {
        get { return health; }
    }

    public bool Right {
        get { return right; }
        set {
            if (right) {
                playerImg.DORotate(new(-54, 180, 0), .3f);
                right = false;
            } else {
                playerImg.DORotate(new(54, 0, 0), .3f);
                right = true;
            }
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Controller.playMode += PlayModeActive;
        Controller.hexMode += HexModeActive;
        canMove = true;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (canMove) {
            anim.SetFloat("vertical", vertical);
            anim.SetFloat("horizontal", horizontal * (Right ? -1 : 1));
            if (horizontal > 0.1f) right = true;
            else if (horizontal < 0.1f) right = false;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                Right = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                Right = true;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Bomb b = Instantiate(bomb, transform.position, Quaternion.identity);
                b.Plant();
            }
        }
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        //jakas animacja czy co
    }
    void PlayModeActive() {
        canMove = true;
    }
    void HexModeActive() {
        canMove = false;
    }

    private void FixedUpdate() {
        if (horizontal != 0 && vertical != 0){
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        if(canMove) rb.velocity = new Vector3(horizontal * runSpeed, 0, vertical * runSpeed);
    }
}
