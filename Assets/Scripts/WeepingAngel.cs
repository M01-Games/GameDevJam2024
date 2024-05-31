using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    Vector3 dest;
    public Camera playerCamera;
    public float aiSpeed;
    public bool canChasePlayer;
    public Transform neck;
    private Animator animator;
    private bool initialAnimatorEnabled = true;
    public GameObject pickupItem;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(EnableAnimatorForDuration(2f));
    }


    private void Update()
    {
        if (pickupItem != null && IsDescendantOf(pickupItem.transform, player))
        {
            canChasePlayer = true;
        }

        if (initialAnimatorEnabled)
        {
            return;
        }

        if (canChasePlayer)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);

            if (GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponent<Renderer>().bounds))
            {
                ai.speed = 0;
                ai.SetDestination(transform.position);
            }
            else
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer > 1)
                {
                    ai.speed = aiSpeed;
                    dest = player.position;
                    ai.destination = dest;

                    Vector3 direction = (player.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aiSpeed);

                    neck.rotation = Quaternion.Slerp(neck.rotation, lookRotation, Time.deltaTime * aiSpeed);
                }
                else
                {
                    ai.speed = 0;
                    ai.SetDestination(transform.position);
                }
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

    private bool IsDescendantOf(Transform item, Transform potentialParent)
    {
        if (item == null)
            return false;

        if (item == potentialParent)
            return true;

        return IsDescendantOf(item.parent, potentialParent);
    }

    private IEnumerator EnableAnimatorForDuration(float duration)
    {
        if (animator != null)
        {
            animator.enabled = true;
            yield return new WaitForSeconds(duration);
            initialAnimatorEnabled = false;
        }
    }
}
