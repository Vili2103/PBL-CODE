using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float delay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f; //So we don't check the obstacles every frame.

    [SerializeField]
    private float attackRange = 0.5f;

    public UnityEvent OnAttack;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;
    [SerializeField]
    private ContextSolver movementDirSolver;

    bool followPlayer = false;

    void Start()
    {
        InvokeRepeating("PerformDetection", 0, delay);
    }

    private void PerformDetection()
    {
        foreach(Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
          
    }
    private void Update()
    {
        if (aiData.currentTarget != null)
        {
            OnPointerInput?.Invoke(aiData.currentTarget.position); // Checks if the OnPointerInput is null. If it isn't it will Invoke the method
            if (followPlayer == false)
            {
                followPlayer = true;
                StartCoroutine(ChaseAndAttack());
            }else if (aiData.getTargetCount() > 0)
            {
                aiData.currentTarget = aiData.targets[0];
            }
            OnMovementInput?.Invoke(movementInput);
        }
    }
    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            movementInput = Vector2.zero;
            followPlayer = false;
            yield break; //Stops the coroutine
        }else {
            float dist = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (dist < attackRange)
            {
                movementInput = Vector2.zero;
                OnAttack?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                movementInput = movementDirSolver.GetDir(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }

    }

}
