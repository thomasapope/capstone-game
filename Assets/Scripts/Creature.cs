using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Creature.cs

    The superclass of all living creatures. This is not meant to be used
    as is, but to be inherited from.
*/

// [RequireComponent(typeof(Health))]
public abstract class Creature : MonoBehaviour
{
    // Component References
    // public Health stats;
    Renderer rend;
    protected Animator animator;
    public Animator attackAnimator;
    public Transform attackPoint;
    public Transform carryPoint;
    public LayerMask attackLayers; // The layers this creature can deal damage to
    

    // Stats
    [SerializeField]
    private int MAX_HEALTH = 100;

    public int hp;

    protected bool hitting;

    public float attackRange = 0.5f;
    public float attackRate = 2f; // Times per second this creature can attack
    private float nextAttackTime = 0f;
    public float attackDelay = .2f;
    public int attackDamage = 10;

    // Other Stats
    // private float hitTime = 1f;
    // private Material defMat;
    // public static Material hitMat;
    [HideInInspector]
    public bool isCarryingItem = false;
    public Interactable item;
    
    // Delegates
    public event System.Action OnAttack;
    public static event Action<Creature> OnHealthAdded = delegate {};
    public static event Action<Creature> OnHealthRemoved = delegate {};
    public event Action<float> OnHealthChanged = delegate {};


    protected virtual void Start()
    {
        hp = MAX_HEALTH;

        // stats = gameObject.GetComponent<Health>();
        animator = GetComponent<Animator>();

        // if (hitMat == null)
        //     hitMat = Resources.Load<Material>("HitMat");
    }


    private void OnEnable()
    {
        OnHealthAdded(this);
    }
    
    
    protected virtual void Update()
    {
        // Attacking
        if (Time.time >= nextAttackTime)
        {
            if (hitting)
            {
                // Debug.Log("Hitting");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // Hit feedback
        // if (hitTime < 1)
        // {
        //     hitTime += Time.deltaTime;
        // }
        // else
        // {
        //     if (rend.material != defMat)
        //     {
        //         rend.material = defMat;
        //     }
        // }
    }


    void Attack()
    {
        hitting = false;

        if (attackAnimator != null)
        {
            attackAnimator.SetTrigger("swing");
        }

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, attackLayers);
        
        if(hits.Length == 0) return;
        // Debug.Log("There are " + hits.Length);

        foreach (Collider enemy in hits)
        {
            // Debug.Log(enemy.name);
            // Creature enemyCreature = enemy.GetComponent<Creature>().TakeDamage(attackDamage);
            StartCoroutine(DoDamage(enemy, attackDelay));
            // enemy.GetComponent<Creature>().TakeDamage(attackDamage);

            IEnumerator DoDamage(Collider creature, float delay)
            {
                yield return new WaitForSeconds(delay);

                if (OnAttack != null)
                {
                    OnAttack();
                }

                creature.GetComponent<Creature>().TakeDamage(attackDamage);
                // creature.TakeDamage(attackDamage);
            }
        }
    }


    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
        ModifyHealth(damage * -1);
        
        // hitTime = 0;
        // rend.material = hitMat;
    }

    
    public void ModifyHealth(int amount)
    {
        hp += amount;
        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        float hpPercent = (float)hp / MAX_HEALTH;
        OnHealthChanged(hpPercent); // update health bar

        if (hp <= 0) // check if dead
        {
            OnDeath();
        }
    }


    protected void PickUpObject(Interactable obj)
    {
        item = obj;
        // Debug.Log("Picked up a " + obj.gameObject.name);
        isCarryingItem = true;
        obj.transform.SetParent(carryPoint);
        obj.transform.position = carryPoint.position;
        obj.OnPickUp();
    }


    protected void DropObject()
    {

        item.transform.SetParent(null);
        item.OnDrop();

        item = null;
        isCarryingItem = false;

        
        // Transform[] children = GetComponentsInChildren<Transform>();
        // foreach (Transform child in children)
        // {
        //     if (child.CompareTag("holdable"))
        //     {
        //         child.SetParent(null);
        //     }
        // }
    }


    protected virtual void OnDeath()
    {
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }
    

    protected virtual void OnDisable()
    {
        OnHealthRemoved(this);
    }

    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public int health 
    {
        get { return hp; }
        set { hp = value; }
    }

    // For animation
    public void Hit(){
    }

    public void Shoot(){
    }

    public void FootR(){
    }

    public void FootL(){
    }

    public void Land(){
    }
}
