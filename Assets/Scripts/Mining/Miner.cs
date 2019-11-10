using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;

namespace RPG.Mining
{
    public class Miner : MonoBehaviour, IAction
    {

        [SerializeField] float pickAxeRange = 2f;
        [SerializeField] float timeBetweenSwings = 3f;
        [SerializeField] float pickDamage = 5f;
        [SerializeField] float pickaxeAngleOffset = 50f;

        float timeSinceLastSwing = Mathf.Infinity;


        MiningTarget target;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastSwing += Time.deltaTime;

            if (target == null) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                MiningBehaviour();
            }

        }

        private void MiningBehaviour()
        {
            // Look at target
            transform.LookAt(target.transform);
            transform.Rotate(Vector3.up, pickaxeAngleOffset);

            if (timeSinceLastSwing >= timeBetweenSwings)
            {
                // This will trigger the PickaxeHit() function through the animation event.
                TriggerPickaxeSwing();
                timeSinceLastSwing = 0f;
            }
        }

        private void TriggerPickaxeSwing()
        {
            GetComponent<Animator>().ResetTrigger("stopMining");
            GetComponent<Animator>().SetTrigger("mineTarget");
        }

        private void StopSwingingPickaxe()
        {
            GetComponent<Animator>().ResetTrigger("mineTarget");
            GetComponent<Animator>().SetTrigger("stopMining");
        }


        public void Mine(GameObject miningTarget)
        {
            //print("Mine: " + miningTarget);
            GetComponent<ActionScheduler>().StartAction(this);
            target = miningTarget.GetComponent<MiningTarget>();

        }

        // Animation Event
        public void PickaxeHit()
        {
            if (target == null) { return; }
            // returns true if ore was destroyed.
            if (target.OreTakesDamage(pickDamage))
            {
                Cancel();
            }
        }


        public void Cancel()
        {
            StopSwingingPickaxe();
            target = null;
            GetComponent<Mover>().Cancel();
        }


        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < pickAxeRange;

        }
    }
}

