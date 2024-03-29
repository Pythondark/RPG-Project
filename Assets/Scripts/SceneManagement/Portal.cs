﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }


        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider otherCollider)
        {            
            if (otherCollider.tag == "Player")
            {
                StartCoroutine(Transition());                
            }            
        }

        
        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            // Fade out
            yield return StartCoroutine(fader.FadeOut(fadeOutTime));

            // Save current 
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            // Load new Scene          
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Load current level
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();
            
            // Wait for a few seconds
            // load?
            yield return new WaitForSeconds(fadeWaitTime);

            // Fade In
            yield return StartCoroutine(fader.FadeIn(fadeInTime));

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;                
                return portal;                              
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            //player.GetComponent<NavMeshAgent>().enabled = false;
            //player.transform.position = otherPortal.spawnPoint.position;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            //player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}


