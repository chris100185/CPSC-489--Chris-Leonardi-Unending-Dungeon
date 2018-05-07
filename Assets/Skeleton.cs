using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//behavior specific to the Skeleton enemy


public class Skeleton : MonoBehaviour
{
    //sound
     AudioSource source;
    
    public AudioClip dying, hit;

    float boneThrowFrequency;
    float boneThrowCountdown;
    EnemyBehavior enemy;
    bool boneThrowActive = false;
    GameManager game;
    public GameObject attack;
    bool isDead = false;//set to true to prevent throwing bones after death. 

    // Use this for initialization
    void Start()
    {
        game = GameManager.instance;
        enemy = GetComponent<EnemyBehavior>();
        boneThrowActive = game.boneThrowActive;
        boneThrowFrequency = Random.Range(2.0f, 5.0f);  //randomize bone throw frequency so not all skeletons are throwing at same time. 
        boneThrowCountdown = boneThrowFrequency;
        //find audiosource
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isDead = enemy.isDead;
        if (!isDead&&boneThrowActive) //don't fire projectiles if dead or if Bone Throw has not been activated
        {
            boneThrowCountdown -= Time.deltaTime;
            if (boneThrowCountdown <= 0)
            {
                ThrowBone();
                boneThrowCountdown = boneThrowFrequency;
            }
        }
    }
    void ThrowBone()
    {
        GameObject bone = Instantiate(attack);
        bone.transform.position = enemy.transform.position;
        Projectile proj = bone.GetComponent<Projectile>();
        if (enemy.dir == EnemyBehavior.direction.down)
            proj.setDirection(new Vector3(0, -1, 0));
        else if (enemy.dir == EnemyBehavior.direction.up)
            proj.setDirection(new Vector3(0, 1, 0));
        else if (enemy.dir == EnemyBehavior.direction.right)
            proj.setDirection(new Vector3(1, 0, 0));
        else if (enemy.dir == EnemyBehavior.direction.left)
            proj.setDirection(new Vector3(-1, 0, 0));
    }
    void BoneThrowRandomizer()
    {
        boneThrowFrequency = Random.Range(2.0f, 5.0f);//randomize bone throw frequency so not all skeletons are throwing at same time. 
    }

    public void DyingSound()
    {
        source.PlayOneShot(dying, 1f);
    }
}
