using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth;

    [SerializeField]
    private GameObject chunkParticles;

    public float currentHealth;
    private GameManager GM;
    private HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameManager.getInstance();
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Instantiate(chunkParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        GM.Respawn();
        GetComponent<PlayerCombatController>().FinishAttack();
        gameObject.SetActive(false);
    }
}
