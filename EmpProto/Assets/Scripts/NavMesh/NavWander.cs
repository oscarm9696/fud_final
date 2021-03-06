using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavWander : MonoBehaviour
{
    [SerializeField]
    bool navWaiting;

    [SerializeField]
    List<Waypoint> pOI;

    NavMeshAgent kyleTheSickBoi;
    int currentPOI;
    bool isWalking;

    [SerializeField]
    bool isBusy;

    bool forward;

    [SerializeField]
    float waitLength;

    [SerializeField]
    float totalWaitTime;

    [SerializeField]
    KyleAnimations kyleAnimations;

    public void Start()
    {
        kyleTheSickBoi = GetComponent<NavMeshAgent>();
        kyleAnimations = GetComponent<KyleAnimations>();
    }
    public void Update()
    {
        CheckDistanceLeft();
    }

    public void Die () {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine () {
        kyleAnimations.Die();

        while (true) {
            kyleTheSickBoi.isStopped = true;

            yield return new WaitForEndOfFrame();
        }
    }

    private void CheckDistanceLeft()
    {
        if (isWalking && kyleTheSickBoi.remainingDistance <= 1f)
        {
            isWalking = false;
            if (navWaiting)
            {
                isBusy = true;
                waitLength = 0f;

                kyleAnimations.Idle();
            }
            else
            {
                MoveToNextPOI();
                SetDestination();
            }
        }

        if (isBusy)
        {
            waitLength += Time.deltaTime;
            if(waitLength >= totalWaitTime)
            {
                isBusy = false;
                MoveToNextPOI();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if(pOI != null)
        {
            Vector3 target = pOI[currentPOI].transform.position;
            kyleTheSickBoi.SetDestination(target);
            isWalking = true;
        }
    }

    private void MoveToNextPOI()
    {
        kyleAnimations.Walk();

        if (UnityEngine.Random.Range(0f, 1f) <= .2f)
        {
            forward = !forward;
        }
        if (forward)
        {
            currentPOI++;
            if(currentPOI  >= pOI.Count)
            {
                currentPOI = 0;
            }
        }
        else
        {
            currentPOI--;
            if(currentPOI < 0)
            {
                currentPOI = pOI.Count - 1;
            }
        }
    }
}
