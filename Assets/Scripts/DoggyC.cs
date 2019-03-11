using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoggyC : MonoBehaviour {

    Animator anim;
    SpriteRenderer flipper;

    private Rigidbody2D rb2d;
    private int count;
    private int life;

    public float speed;
    public float jumpForce;

    public Text countText;
    public Text winText;
    public Text lifeText;
    public Text dieText;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    //SETUP-------------------------------------------
    void Start()
    {
        anim = GetComponent<Animator>();
        flipper = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        count = 0;
        life = 3;
        winText.text = "";
        dieText.text = "";
        SetCountText();

        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    //MOVEMENT-------------------------------------------
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);
    }

    //JUMP------------------------------------------------
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }


    //PICKUP AND ENEMY-------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        else if
            (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            life = life - 1;
            SetCountText();
        }
        //DEATH OR LVL UP----------------------------
        if (count == 4)
        {
            Camera.main.transform.position = new Vector3(21.0f, Camera.main.transform.position.y, -10.0f);
            transform.position = new Vector2(13.0f, transform.position.y);
            life = 3;
            SetCountText();

            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.SetActive(false);
                life = life - 1;
                SetCountText();
            }
        }

        if (life == 0)
        {
            Destroy(this.gameObject);
        }
    }

    //COUNT TEXT---------------------------------------
    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        lifeText.text = "Lives: " + life.ToString();

        //WIN TEXT------------------------------------
        if (count == 8)
        {
            winText.text = "You Win!";

            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
        else if (life <= 0)
        {
            dieText.text = "You Died!";
        }
    }

    void Update()
    {
        //idle -> walk -> idle(RIGHT)------------------------------------
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 1);
            flipper.flipX = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }

        //idle -> walk -> idle(LEFT)------------------------
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 1);
            flipper.flipX = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 0);
        }

        //JUMP---------------------------------------------

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }

}
