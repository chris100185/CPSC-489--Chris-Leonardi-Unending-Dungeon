using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    //should only be one player
    public static PlayerManager instance = null;


    public int maxHealth;
    public int health, defense, attack;
    private Animator anim; //The parent animator.
    private Rigidbody2D rigid;
    public float knockStrength; //the strength of the knockback

    private float knockTimer=0.08f;//length of time of knockback
    private float knockCount;//countdown timer for remaining knockback.

    //Player statuses
    public bool attackLock = false;
    public bool knockLock = false; //locks controls if in the middle of a lockback
    public bool fallLock = false;//locks control and acts as a flag while player is falling down pit. 
    public bool deadLock = false; //used to prevent movement and to let the game know the player is dead.
    public bool active = false; //ued to ensure that certain effects don't trigger until the game is properly loaded

    //sound
    AudioSource source;
    public AudioClip sword;


    Vector3 lerpStart;
    Vector3 lerpEnd;
    private float fallTimer=0;
    private float posX,posY; //used to hold the facing while getting knocked back.
    private enum direction { up, down, left, right,upRight,upLeft,downRight,downLeft};
    private direction dir;

    //handle player damage
    float playerInvulnTime = 0.2f;
    float playerInvulnCountdown;
    bool PlayerInvuln=false; //true immediately upon taking damage to prevent multiple damage occuring from same contact. 

    GameManager game;


    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        game = GameManager.instance;
        anim = transform.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        lerpStart = new Vector3(1, 1, 1);
        lerpEnd = new Vector3(0, 0, 0);
    }
    //ensure that only one player is active, and that it lasts between levels. 
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
       // Spawn();
    }



    // Update is called once per frame
    void Update ()
    {
        //update facing values for various calculations
        posX = anim.GetFloat("moveX");
        posY = anim.GetFloat("moveY");

        //check if player is alive
        if (health <= 0) deadLock = true;
        //update player facing
        UpdateDirection();
        HandleAttackState();//check attack state and make updates to values accordingly

        if (knockLock == true) Knockback(); //if under attack update knockback
        if (fallLock == true)  Falling();//if falling, continue to fall. wheeeeeeee
        if (deadLock == true) killPlayer();


        //update damage invulnerability
        Invulnerability();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if we've been knocked into a wall, if so, stop. 
        if(collision.gameObject.tag=="wall")
        {
            knockLock = false;
            anim.SetBool("isKnocked", false);
        }
    
        //print(collision.collider.name);
        //print("collided with "+collision.gameObject.name);
        //if collide with enemy damage character.
        if (collision.gameObject.tag=="Enemy"&&knockLock==false)//if hitting an enemy and not already getting knocked back
        {
           /* EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
            health -= enemy.attack;            
            knockTimer = 0.09f;
            knockLock = true;
            anim.SetBool("isKnocked", true);
            //check posX,posY values from animator to determine facing, and launch player in opposite direction at 
            //at a high rate of speed.            
            //no need for if else for each direction, just invert the direction values for movement*/
            
            
        } 
        if(collision.gameObject.tag=="pit")//we've touched the collison box set up so enemies can't walk through pits
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
           
        }
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print((collision.gameObject.tag));
        if(collision.gameObject.tag=="pit") //player has entered a pit. This should hurt
        {
            //fallLock = true;
            //anim.SetBool("isFalling", true);
        }
        if(collision.gameObject.tag=="projecticle")
        {

        }
        //print("Collided with trigger");
        //  Debug.Log("Collided with trigger on hero");
    }
    //custom methods
    public void HandleAttackState() //check attack state and disable weapon if not attacking, or enable and set if attacking
    {
        CapsuleCollider2D weapon = GetComponent<CapsuleCollider2D>();
        bool isAttacking = anim.GetBool("isAttacking");
        if(isAttacking)
        {
            SetWeaponCollider();
            weapon.enabled = true;
        }
        else
        {
            weapon.enabled = false;
        }
    }
    public void SetWeaponCollider()
    {
        //testing for method
        //print("entered set weapon collider");

        CapsuleCollider2D weapon = GetComponent<CapsuleCollider2D>();
        Vector2 size=new Vector2(0,0);  
        Vector2 offset=new Vector2(0,0);
        //left facing sizeX=.1,sizeY=1, offsetX=-.45, offsetY=.5 direction vertical
        if (dir == direction.left)
        {
            size =new Vector2(0.1f, 1);
            offset = new Vector2(-0.5f,0.5f);           
            weapon.direction = CapsuleDirection2D.Vertical;
        }
        //right facing sizeX=.1,sizeY=1, offsetX=-.45, offsetY=.5 direction vertical
        else if (dir==direction.right)
        {
            size = new Vector2(0.1f, 1);
            offset = new Vector2(0.5f, 0.5f);
            weapon.direction= CapsuleDirection2D.Vertical;
        }
        //down facing sizeX=1,sizeY=.1, offsetX=0, offsetY=0 direction horizontal
        else if (dir==direction.down||dir==direction.downLeft||dir==direction.downRight)
        {
            size = new Vector2(1f, 0.1f);
            offset = new Vector2(0f, 0f);
            weapon.direction = CapsuleDirection2D.Horizontal;

        }
        //up facing sizeX=1,sizeY=.1, offsetX=0, offsetY=.9 direction horizontal
        else if (dir == direction.up || dir == direction.upLeft || dir == direction.upRight)
        {
            size = new Vector2(1f, 0.1f);
            offset = new Vector2(0f, 0.9f);
            weapon.direction = CapsuleDirection2D.Horizontal;
        }

        

        weapon.size = size;
        weapon.offset = offset;
    }
    public void UpdateDirection()
    {
        //update player facing for info purposes

        if (posX == 0.0f && posY == -1.0f)
        {
            dir = direction.down;
        }
        else if (posX == 0.0f && posY == 1.0f)
        {
            dir = direction.up;
        }
        else if (posX == 1.0f && posY == 0.0f)
        {
            dir = direction.right;
        }
        else if (posX == -1.0f && posY == 0.0f)
        {
            dir = direction.left;
        }
        else if (posX == 1.0f && posY == 1.0f)
        {
            dir = direction.upRight;
        }
        else if (posX == -1.0f && posY == -1.0f)
        {
            dir = direction.downLeft;
        }
        else if (posX == 1.0f && posY == -1.0f)
        {
            dir = direction.downRight;
        }
        else if (posX == -1.0f && posY == 1.0f)
        {
            dir = direction.upLeft;
        }
    }
    public void DamagePlayer(int damage,bool knockBack,bool falling)//public function for external forces and this manager to access and damage player, with optional effects
    {
        //ensure that repeated damage doesn't occur
        if (PlayerInvuln)
            return;
        PlayerInvuln = true;
        playerInvulnCountdown = playerInvulnTime; 

        health -=damage;//first apply the damage
        //now apply any effects
        if (knockBack)
        {
            knockLock = true;
            knockCount = knockTimer;
        }
        if (falling)
        {
            fallLock = true;
            anim.SetBool("isFalling", true);
        }

    }
    private void Knockback()//knockback player if under attack
    {
        // check posX,posY values from animator to determine facing, and launch player in opposite direction at
        //at a high rate of speed.            
        anim.SetBool("isKnocked", true);
        //change to rigidbody movement for proper collision detection
        Vector2 curPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 facing = new Vector2(-posX, -posY);
        Vector2 newPos = curPosition + (facing * knockStrength * Time.deltaTime);
        rigid.MovePosition(newPos);

        knockCount -= Time.deltaTime;
        if (knockCount <= 0)
        {
            knockLock = false;
            anim.SetBool("isKnocked", false);
        }
    }
    private void Falling() //player falls down pit and goes boom. 
    {
        fallTimer += Time.deltaTime / 2;//Take 3 seconds to complete falling. 
        transform.localScale = Vector3.Lerp(lerpStart, lerpEnd,fallTimer);
        if(transform.localScale.x<=0)
        {
            Spawn();
            fallLock = false;
            anim.SetBool("isFalling", false);
            transform.localScale = lerpStart;
            fallTimer = 0.0f;
        }
    }
    private void killPlayer()
    {
        anim.SetBool("isDead", true);

    }
    public void Spawn() //places the player at spawn either after falling or at start of level. 
    {
        transform.position = GameObject.Find("Spawn").transform.position;
    }
    public void GameOver()
    {
        game.GameOver();
    }
    void Invulnerability()
    {
        if(PlayerInvuln)
        {
            playerInvulnCountdown -= Time.deltaTime;
          /*  if (transform.localScale.x == 1)
                transform.localScale = new Vector3(0, 1, 1);
            else
                //transform.localScale = new Vector3(1, 1, 1);*/

            if(playerInvulnCountdown<=0)
            {
                PlayerInvuln = false;
                //transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void Healing(int percentage)
    {
        int healAmount =Mathf.RoundToInt( ((float)maxHealth*((float)percentage*.01F)));
        health += healAmount;
        if (health > maxHealth)
            health = maxHealth;
        print("healing by " + healAmount);
    }
    public void SwordSlash()
    {
        source.PlayOneShot(sword); 
    }
    public void UnLockAttack()
    {
        attackLock =false;
    }
}
