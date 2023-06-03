using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;
    private float timeUntilAttack;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject particles;

    private int enemyHP = 10;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        timeUntilAttack = 2f;
    }

    private void Update() {
        MoveToPlayer();

        if(agent.velocity.magnitude > 0.1f) {
            RotateToPlayer();
        }

        if(agent.velocity.magnitude < 0.1f) {
            animator.SetBool("isWalking", false);
            AttackPlayer();
        }
    }

    private void MoveToPlayer() {
        agent.SetDestination(target.position);
        animator.SetBool("isWalking", true);
    }

    private void RotateToPlayer() {
        // too jittery
        // transform.LookAt(target);

        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = rotation;
    }

    private void AttackPlayer() {
        timeUntilAttack += Time.deltaTime;

		if (timeUntilAttack < 2f) {
			return;
		}

        animator.SetTrigger("isAttacking");
        timeUntilAttack = 0;
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.CompareTag("Left Hand")) {
            enemyHP--;

            if(enemyHP == 0) {
                SpawnParticles(transform.position);
                Destroy(gameObject);
                GameManager.Instance.enemiesPunched++;
                GameManager.Instance.SpawnEnemy();
            }
        }
    }

    private void SpawnParticles(Vector3 position) {
        position.y += 1;
        Instantiate(particles, position, new Quaternion(0, 0, 0, 0));
    }
    
}
