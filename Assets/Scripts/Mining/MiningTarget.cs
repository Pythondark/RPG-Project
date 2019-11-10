using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Mining
{    
    public class MiningTarget : MonoBehaviour
    {
        [SerializeField] float targetHealth = 20;
        [SerializeField] float oreRespawnTime = 5f;
        [SerializeField] OreIdentifier oreIdentity = OreIdentifier.Silver;

        bool oreAvailable = true;

        enum OreIdentifier
        {
            Silver, Runic, Mithril, Meteore, Iron, Gold, Bronze, Adamantite
        }


        public bool OreAvailable()
        {
            return oreAvailable;
        }

        public bool OreTakesDamage(float damage)
        {
            targetHealth -= damage;
            if (targetHealth <= 0)
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }

    }
}


