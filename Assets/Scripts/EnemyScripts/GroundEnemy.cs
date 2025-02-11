using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundEnemy : Enemy
{
    [Header("Pathing Specifications")]
    [SerializeField] public PathStats pathStats;
    [SerializeField] public float pathDistanceTrigger = 0.1f;
    [SerializeField] public float minMoveAgainDelay = 0.1f;
    [SerializeField] public float maxMoveAgainDelay = 1f;

    [Header("DEBUG]")]
    [SerializeField] public Animator animator;
    public Coroutine attackOrder;
    [SerializeField] public bool targetDead;
    [SerializeField] public bool coroutineStarted;
    public float distanceToTarget;
    public Transform target;
    public int pathIndex;


    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coroutineStarted = false;
        targetDead = true;
    }

    protected override void Start()
    {
        base.Start();
        setTargetPath(pathIndex);
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        
        faceDirection(target);

        if (targetDead)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (targetDead)
        {
            if (coroutineStarted == true)
            {
                //Debug.Log("Stopped coroutine");
                StopCoroutine(attackOrder);
                coroutineStarted = false;
            }
            Move();
        }
    }

    public IEnumerator AttackNode(INode targetNode)
    {
        //Debug.Log("Target node: " + targetNode);
        while (gameState.IsPaused()) yield return null;

        if (targetNode.IsUnityNull())
        {
            //Debug.Log("went in loop: " + targetNode);
            yield return new WaitForSecondsRealtime(
                Random.Range(minMoveAgainDelay, maxMoveAgainDelay));
            targetDead = true;
        }

        else
        {
            animator.Play(attackAnimation, 0, 0);
            DoDamage(targetNode);
            yield return new WaitForSecondsRealtime(attackSpeed);
            attackOrder = StartCoroutine(AttackNode(targetNode));
        }
    }

    public void DoDamage(INode targetNode)
    {
        targetNode.takeHealthDamage(damage);
    }

    public void Move()
    {
        distanceToTarget = Vector2.Distance(target.position, transform.position);

        if (distanceToTarget <= pathDistanceTrigger)
        {
            pathIndex++;

            if (pathIndex >= GetPathLength())
            {
                //EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                setTargetPath(pathIndex);
                //target = LevelManage.main.Path[pathIndex];
            }
        }
    }
    public void setTargetPath(int _pathIndex)
    {
        target = pathStats.GetGroundPath(pathAssignment).pointList[_pathIndex];
    }
    public int GetPathLength()
    {
        return pathStats.GetGroundPath(pathAssignment).GetPathLength();
    }

}
