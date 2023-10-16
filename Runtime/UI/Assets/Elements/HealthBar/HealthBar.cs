using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace info.jacobingalls.jamkit
{
    public class HealthBar : MonoBehaviour
    {

        private float health;

        public float StartingHealth = 100.0f;
        public float MaxHealth = 100.0f;

        private float recentDamage = 0.0f;
        public AnimationCurve DamageSpeed;
        private float recentDamageTime = 0.0f;
        public float RecentDamageMaxTime = 0.0f;

        public bool UsePsudoRecentDamage = true;
        private float pseudoRecentDamage = 0.0f;
        public AnimationCurve PseudoDamageSpeed;
        private float pseudoRecentDamageTime = 0.0f;
        public float PseudoRecentDamageMaxTime = 0.0f;

        private float recentHealing = 0.0f;
        public AnimationCurve HealingSpeed;
        private float recentHealingTime = 0.0f;
        public float RecentHealingMaxTime = 0.0f;

        public bool UsePsudoRecentHealing = true;
        private float pseudoRecentHealing = 0.0f;
        public AnimationCurve PseudoHealingSpeed;
        private float pseudoRecentHealingTime = 0.0f;
        public float PseudoRecentHealingMaxTime = 0.0f;

        public RectTransform container;
        public LayoutElement healthBar, recentHealingBar, recentDamageBar;

        public UnityEvent<float> OnDamage, OnHeal;

        // Start is called before the first frame update
        void Start()
        {
            health = StartingHealth;
        }

        // Update is called once per frame
        void Update()
        {

            // Display
            var width = container.sizeDelta.x;
            

            // Damage
            recentDamageTime = Mathf.Clamp(recentDamageTime + Time.deltaTime, 0, RecentDamageMaxTime);
            float maxDamageMovement = DamageSpeed.Evaluate(recentDamageTime) * MaxHealth;
            recentDamage = Mathf.MoveTowards(recentDamage, 0, Time.deltaTime * maxDamageMovement);

            var recentDamageDelta = 0.0f;
            var damageWidth = 0.0f;
            if (UsePsudoRecentDamage)
            {
                pseudoRecentDamageTime = Mathf.Clamp(pseudoRecentDamageTime + Time.deltaTime, 0, PseudoRecentDamageMaxTime);
                maxDamageMovement = PseudoDamageSpeed.Evaluate(pseudoRecentDamageTime) * MaxHealth;
                pseudoRecentDamage = Mathf.MoveTowards(pseudoRecentDamage, recentDamage, Time.deltaTime * maxDamageMovement);
                recentDamageDelta = recentDamage - pseudoRecentDamage;
                damageWidth = width * (pseudoRecentDamage / MaxHealth);
                
            } else
            {
                damageWidth = width * (recentDamage / MaxHealth);
            }

            recentDamageBar.preferredWidth = damageWidth;
            recentDamageBar.gameObject.SetActive(damageWidth > 0);

            // Healing
            recentHealingTime = Mathf.Clamp(recentHealingTime + Time.deltaTime, 0, RecentHealingMaxTime);
            float maxHealingMovement = HealingSpeed.Evaluate(recentHealingTime) * MaxHealth;
            recentHealing = Mathf.MoveTowards(recentHealing, 0, Time.deltaTime * maxHealingMovement);

            var healingWidth = 0.0f;
            if (UsePsudoRecentHealing)
            {
                pseudoRecentHealingTime = Mathf.Clamp(pseudoRecentHealingTime + Time.deltaTime, 0, PseudoRecentHealingMaxTime);
                maxHealingMovement = PseudoHealingSpeed.Evaluate(pseudoRecentHealingTime) * MaxHealth;
                pseudoRecentHealing = Mathf.MoveTowards(pseudoRecentHealing, recentHealing, Time.deltaTime * maxHealingMovement);
                healingWidth = width * (pseudoRecentHealing / MaxHealth);
            } else
            {
                healingWidth = width * (recentHealing / MaxHealth);
            }

            recentHealingBar.preferredWidth = healingWidth;
            recentHealingBar.gameObject.SetActive(healingWidth > 0);

            healthBar.preferredWidth = width * (Mathf.Max(0, health - recentHealing + recentDamageDelta) / MaxHealth);
        }

        public void HealAmount(float amount)
        {
            float maxAmount = Mathf.Min(amount, Mathf.Max(MaxHealth - health, 0));
            if (maxAmount <= 0) { return; }

            health += maxAmount;
            recentHealing += maxAmount;
            recentDamage = Mathf.Max(recentDamage - maxAmount, 0);
            recentHealingTime = 0;
            OnHeal.Invoke(health);
        }

        public void HealPercentage(float percentage)
        {
            HealAmount(health * (percentage / 100.0f));
        }

        public void DamageAmount(float amount)
        {
            float maxAmount = Mathf.Min(amount, health);
            if (maxAmount <= 0) { return; }

            health -= maxAmount;
            recentDamage += maxAmount;
            recentHealing = Mathf.Max(recentHealing - maxAmount, 0);
            recentDamageTime = 0;
            OnDamage.Invoke(health);
        }

        public void DamagePercentage(float percentage)
        {
            DamagePercentage(health * (percentage / 100.0f));
        }
    }
}
