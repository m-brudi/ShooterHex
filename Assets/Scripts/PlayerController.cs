using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour, IDamageable {

    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] Animator anim;
    [SerializeField] Animator deathEffectAnimator;
    [SerializeField] Transform playerImg;
    [SerializeField] float runSpeed = 20.0f;
    [SerializeField] Bomb bomb;
    [SerializeField] int maxHealth = 10;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject shadowSphere;
    int currentHealth;
    Rigidbody rb;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    bool right = true;
    bool canMove;
    public bool dead;
    public float Horizontal {
        get { return horizontal; }
    }
    public float Vertical {
        get { return vertical; }
    }

    public int Health {
        get { return maxHealth; }
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
    }

    public void StartGame() {
        shadowSphere.SetActive(true);
        playerShoot.StartReload();
        canMove = true;
        dead = false;
        sr.enabled = true;
        currentHealth = maxHealth;
        UiManager.Instance.SetupHpFill(currentHealth, maxHealth);
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
                Death();
            }
        }
    }

    public void Damage(int damage, Vector3 pos) {
        currentHealth -= damage;
        anim.SetTrigger("TakeHit");
        if (currentHealth <= 0) Death();
        UiManager.Instance.SetupHpFill(currentHealth, maxHealth);
    }

    void Death() {
        canMove = false;
        dead = true;
        StartCoroutine(DelayRestart());
    }
    IEnumerator DelayRestart() {
        yield return new WaitForSeconds(.5f);
        sr.enabled = false;
        shadowSphere.SetActive(false);
        deathEffectAnimator.SetTrigger("GO");
        yield return new WaitForSeconds(1);
        Controller.Instance.SwitchToHexMode();
        yield return new WaitForSeconds(2);
        Controller.Instance.StartGame();
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
