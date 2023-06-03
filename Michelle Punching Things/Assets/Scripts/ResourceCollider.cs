using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollider : MonoBehaviour {

    [SerializeField] private Item resource;
    [SerializeField] private GameObject particles;
    private int resourceHP = 5;

    private void OnTriggerEnter(Collider collision) {
        if(collision.CompareTag("Left Hand")) {
            resourceHP--;

            if(resourceHP == 0) {
                SpawnParticles(transform.position);
                InventoryManager.Instance.Add(resource);
                Destroy(gameObject);

                if(gameObject.CompareTag("Tree")) {
                    GameManager.Instance.treesPunched++;
                } else if(gameObject.CompareTag("Stone")) {
                    GameManager.Instance.stonePunched++;
                }
            }
        }
    }

    private void SpawnParticles(Vector3 position) {
        position.y += 1;
        Instantiate(particles, position, new Quaternion(0, 0, 0, 0));
    }

}
