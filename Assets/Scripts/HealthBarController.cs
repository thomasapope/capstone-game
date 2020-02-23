using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBarPrefab;

    private Camera mainCamera;

    // private Dictionary<Health, HealthBar> healthBars = new Dictionary<Health, HealthBar>();
    private Dictionary<Creature, HealthBar> healthBars = new Dictionary<Creature, HealthBar>();


    private void Awake()
    {
        mainCamera = Camera.main;

        // Health.OnHealthAdded += AddHealthBar;
        // Health.OnHealthRemoved += RemoveHealthBar;
        Creature.OnHealthAdded += AddHealthBar;
        Creature.OnHealthRemoved += RemoveHealthBar;
    }


    // private void AddHealthBar(Health health) 
    private void AddHealthBar(Creature health) 
    {
        if (healthBars.ContainsKey(health) == false)
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(health, healthBar);
            healthBar.SetCreature(health);
            healthBar.SetCamera(mainCamera);
        }
    }


    // private void RemoveHealthBar(Health health) 
    private void RemoveHealthBar(Creature health) 
    {
        if (healthBars.ContainsKey(health))
        {
            Destroy(healthBars[health].gameObject);
            healthBars.Remove(health);
        }
    }
}
