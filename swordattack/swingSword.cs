using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingSword : MonoBehaviour
{

    private bool readyToSwing, swinging;
    // in the unity interface add here: sword attack key, animator component of sword, sword gameobject
    [SerializeField] private KeyCode swingKey = KeyCode.Mouse0;
    [SerializeField] public Animator swordAnimator;
    [SerializeField] public GameObject sword;

    //in the unity interface add here: point of attack(usualy the sword or player transform), range of the sword attack, and layer mask that every enemy has
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackRange = 0.5f;
    [SerializeField] public LayerMask enemyLayers;
    private bool enemyInAttackRange;
    //in the unity interface add here: audio sorces for the swining and impact
    [SerializeField] public AudioSource swing1, swing2, swing3, swing4;
    [SerializeField] public AudioSource swordImpact;

    // Start is called before the first frame update
    void Start()
    {
        readyToSwing = true;
        swinging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(swingKey) && readyToSwing)
        {

            swinging = true;

            //play random sword swinging sounds. 
            //this example includes 4 different sounds, but you may add to or reduce to the amount of sounds and change the switch case logic accordingly.
            int r = Random.Range(1, 5);
            switch (r)
            {
                case 1:
                    swing1.Play();
                    break;
                case 2:
                    swing2.Play();
                    break;
                case 3:
                    swing3.Play();
                    break;
                case 4:
                    swing4.Play();
                    break;
            }

            //play sword swinging animation.
            //this example inclues just one animation, but you may add more animations using the same logic from the sword swining sounds.
            swordAnimator.Play(/*name of the swinging animation*/, 0, 0.0f);
        }

        if (readyToSwing && swinging)
        {
            readyToSwing = false;
            //change the float falue to the amount of time in between swings in seconds.
            Invoke("resetSwing", 0.4f); 
            enemyInAttackRange = Physics.CheckSphere(attackPoint.position, attackRange, enemyLayers);

            if (enemyInAttackRange)
                damageEnemy();
        }
    }

    void resetSwing()
    {
        swinging = false;
        readyToSwing = true;
    }

    public void damageEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        for (int i = 0; i < enemies.Length; i++)
        {
            //damage enemy.
            Invoke("playImpactSound", 0.4f);
            enemies[i].GetComponent</*name of your enemy script containing TakeDamage method*/>().TakeDamage(Random.Range(8, 13));
        }
    }

    void playImpactSound()
    {
        swordImpact.Play();
    }
