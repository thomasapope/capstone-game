using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public HealthBar healthBarPrefab;

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
    private void AddHealthBar(Creature creature) 
    {
        if (healthBars.ContainsKey(creature) == false)
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(creature, healthBar);
            healthBar.SetCreature(creature);
            healthBar.SetCamera(mainCamera);
        }
    }


    // private void RemoveHealthBar(Health health) 
    private void RemoveHealthBar(Creature creature) 
    {
        if (healthBars.ContainsKey(creature))
        {
            if (healthBars[creature])
            {
                Destroy(healthBars[creature].gameObject);
            }
            healthBars.Remove(creature);
        }
    }
}
