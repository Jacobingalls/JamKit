using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class BattleSimulation : MonoBehaviour
    {

        public HealthBar healthBar, staminaBar;
        private float currentTime = 1f;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) {
                Tick();
            }
        }

        void Tick() {
            var rand = Random.Range(0, 100);
            currentTime += Random.Range(-0.25f, 0.25f);

            if (healthBar.Value <= 0) {
                deal(-100, -100);
                currentTime += 5.0f;
                return;
            } else if (staminaBar.Value <= 0) {
                // Don't deal heal or anything, but make harder
                // hitting things more likely.
                rand /= 4;
            }

            if (rand > 80) {
                // Nothing!
                currentTime += 0.25f;
            } else if (rand > 20) {
                VeryLightAttack();
                currentTime += 0.25f;
            } else if (rand > 10) {
                LightAttack();
                currentTime += 0.5f;
            } else if (rand > 5) {
                MediumAttack();
                currentTime += 0.5f;
            } else if (rand > 2) {
                MediumStunAttack();
                currentTime += 1.0f;
            } else {
                HeavyAttack();
                currentTime += 2.0f;
            }

            if (staminaBar.Value <= 0) {
                currentTime /= 4;
            }
        }

        void deal(float health, float stamina) {
            healthBar.Value -= health ;
            staminaBar.Value -= stamina;
        }

        public void VeryLightAttack() {
            var scaler = Random.Range(0.8f, 1.2f);
            deal(2 * scaler, 5 * scaler);
        }

        public void LightAttack() {
            var scaler = Random.Range(0.8f, 1.2f);
            deal(5 * scaler, 15 * scaler);
        }

        public void MediumAttack() {
            var scaler = Random.Range(0.8f, 1.2f);
            deal(20 * scaler, 10 * scaler);
        }

        public void MediumStunAttack() {
            var scaler = Random.Range(0.8f, 1.2f);
            deal(10 * scaler, 40 * scaler);
        }

        public void HeavyAttack() {
            var scaler = Random.Range(0.8f, 1.2f);
            deal(40 * scaler, 80 * scaler);
        }
    }

}