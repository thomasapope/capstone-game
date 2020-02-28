using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;
    
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    [SerializeField]
    private float positionOffset = 2;

    private Camera mainCamera;

    // private Health health;
    private Creature creature;

    
    // public void SetHealth(Health health) 
    public void SetCreature(Creature creature) 
    {
        this.creature = creature;
        creature.OnHealthChanged += HandleHealthChanged;
    }


    public void SetCamera(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }


    private void Start()
    {
        // Get healthbar foreground. Not necessary, but removes an warning from the console.
        foregroundImage = transform.GetChild(1).GetComponent<Image>();
    }


    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }


    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = pct;
    }


    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
        transform.position = creature.transform.position + Vector3.up * positionOffset;
        //transform.position = Camera.main.WorldToScreenPoint(health.transform.position + Vector3.up * positionOffset);
        //transform.position = Camera.main.WorldToScreenPoint(health.transform.position);
        // transform.position = health.transform.position;
    }


    private void OnDestroy() 
    {
        creature.OnHealthChanged -= HandleHealthChanged;
    }
}
