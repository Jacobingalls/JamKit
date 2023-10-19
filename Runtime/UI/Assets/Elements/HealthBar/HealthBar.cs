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

        private float _health;
        public float Value {
            get {
                return _health;
            }

            set {
                var delta = _health - value;
                if (delta > 0) {
                    DamageAmount(delta);
                } else if (delta < 0) {
                    HealAmount(-delta);
                }
            }
        }

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
            _health = StartingHealth;
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

            healthBar.preferredWidth = width * (Mathf.Max(0, _health - recentHealing + recentDamageDelta) / MaxHealth);
        }

        public void HealAmount(float amount)
        {
            float maxAmount = Mathf.Min(amount, Mathf.Max(MaxHealth - _health, 0));
            if (maxAmount <= 0) { return; }

            _health += maxAmount;
            recentHealing += maxAmount;
            recentDamage = Mathf.Max(recentDamage - maxAmount, 0);
            recentHealingTime = 0;
            OnHeal.Invoke(_health);
        }

        public void HealPercentage(float percentage)
        {
            HealAmount(_health * (percentage / 100.0f));
        }

        public void DamageAmount(float amount)
        {
            float maxAmount = Mathf.Min(amount, _health);
            if (maxAmount <= 0) { return; }

            _health -= maxAmount;
            recentDamage += maxAmount;
            recentHealing = Mathf.Max(recentHealing - maxAmount, 0);
            recentDamageTime = 0;
            OnDamage.Invoke(_health);
        }

        public void DamagePercentage(float percentage)
        {
            DamagePercentage(_health * (percentage / 100.0f));
        }
    }
}
