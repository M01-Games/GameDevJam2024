using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : MonoBehaviour
{
    public NavMeshAgent ai;
    public GameObject playerObj;
    public Transform player;
    Vector3 dest;
    public float aiSpeed;
    public float chaseDistance;
    public bool canChasePlayer = false;
    public Transform neck;
    private Animator animator;
    private bool initialAnimatorEnabled = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(EnableAnimatorForDuration(2f));
    }

    private void Update()
    {
        if (initialAnimatorEnabled)
        {
            return;
        }

        if (canChasePlayer)
        {
            animator.enabled = true;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > chaseDistance)
            {
                ai.speed = aiSpeed;
                dest = player.position;
                ai.destination = dest;

                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aiSpeed);

                neck.rotation = Quaternion.Slerp(neck.rotation, lookRotation, Time.deltaTime * aiSpeed);
            }
        }
        else
        {
            animator.enabled = false;
            RotateNeckTowardsPlayer();
        }
    }

    private void RotateNeckTowardsPlayer()
    {
        if (neck != null && player != null)
        {
            Vector3 direction = (player.position - neck.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            neck.rotation = Quaternion.Slerp(neck.rotation, lookRotation, Time.deltaTime * aiSpeed);
        }
    }

    private IEnumerator EnableAnimatorForDuration(float duration)
    {
        if (animator != null)
        {
            animator.enabled = true;
            yield return new WaitForSeconds(duration);
            initialAnimatorEnabled = false;
            animator.enabled = false; // Disable the animator after the duration
        }
    }
}
