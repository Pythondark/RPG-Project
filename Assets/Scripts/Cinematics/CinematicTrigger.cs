using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {

        bool alreadyTriggered = false;


        private void OnTriggerEnter(Collider otherCollider)
        {
            if ((otherCollider.gameObject.tag == "Player") && !alreadyTriggered)
            {
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }
            
        }

        // === SAVING ===
        object ISaveable.CaptureState()
        {
            return alreadyTriggered;
        }

        void ISaveable.RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
        }
    }
}

