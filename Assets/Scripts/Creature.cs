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
    protected Animator animator;
    public Transform attackPoint;
    public Transform carryPoint;
    public LayerMask attackLayers; // The layers this creature can deal damage to
    

    // Stats
    [SerializeField] private int MAX_HEALTH = 100;

    [HideInInspector] public int hp;

    protected bool hitting;
    protected int hitsQueued; // Is another hit queued?

    public float attackRange = 0.5f;
    public float attackRate = 3f; // Times per second this creature can attack
    private float nextAttackTime = 0f;
    public float attackDelay = .2f;
    // public int attackDamage = 10;
    
    // Weapons
    [HideInInspector] public Weapon currentWeapon;

    // Other Stats
    [HideInInspector] public bool isCarryingItem = false;
    [HideInInspector] public Interactable item;
    public float pickupDistance = 4f;
    
    // Delegates
    public event System.Action OnAttack;
    public static event Action<Creature> OnHealthAdded = delegate {};
    public static event Action<Creature> OnHealthRemoved = delegate {};
    public event Action<float> OnHealthChanged = delegate {};


    protected virtual void Start()
    {
        hp = MAX_HEALTH;

        animator = GetComponent<Animator>();
        currentWeapon = GetComponentInChildren<Weapon>();
        // animator.SetInteger("Weapon", 1);
        // animator.SetTrigger("InstantSwitchTrigger");
    }


    private void OnEnable()
    {
        OnHealthAdded(this);
    }
    
    
    protected virtual void Update()
    {
        // Prevent too many hits from queuing up
        if (hitsQueued > 2)
            hitsQueued = 2;

        // Attacking
        if (hitting || hitsQueued > 0)
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("AttackTrigger");
                currentWeapon.Attack();
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }


    void Attack()
    {
        hitting = false;
        hitsQueued--;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, currentWeapon.attackRange, attackLayers);
        
        if(hits.Length == 0) return;

        foreach (Collider enemy in hits)
        {
            StartCoroutine(DoDamage(enemy, attackDelay));

            IEnumerator DoDamage(Collider creature, float delay)
            {
                yield return new WaitForSeconds(delay);

                if (OnAttack != null)
                {
                    OnAttack();
                }

                if (creature) // Make sure creature still exists
                {
                    creature.GetComponent<Creature>().TakeDamage(currentWeapon.damage);
                    // print("Hit " + creature.name + " for " + currentWeapon.damage + " damage!");
                }
            }
        }
    }


    public virtual void TakeDamage(int damage)
    {
        ModifyHealth(damage * -1);
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


    protected virtual void PickUpObject(Interactable obj)
    {
        item = obj;
        isCarryingItem = true;
        obj.transform.SetParent(carryPoint);
        obj.transform.position = carryPoint.position;
        obj.OnPickUp();
    }


    protected virtual void DropObject()
    {

        item.transform.SetParent(null);
        item.OnDrop();

        item = null;
        isCarryingItem = false;
    }


    protected virtual void OnDeath()
    {
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
                // currentWeapon.Attack();
        // hitsQueued--;
        // if (hitsQueued > 0)
        // {
        //     animator.SetTrigger("AttackTrigger");
        //     currentWeapon.Attack();
        // }
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
