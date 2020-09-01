using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Manager;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _destination;
    private Animator _anim;

    public int health;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

        _destination = GameObject.Find("Destination").transform.position;
        _agent.SetDestination(_destination);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyMech()
    {
        this.gameObject.SetActive(false);
    }

    void TakeDamage()
    {
        //health -= damageAmount;

        if (health <= 0)
        {
            _anim.Play("Dying");
            DestroyMech();
        }
    }

    void Attack()
    {
        _anim.Play("Attacking");
    }
}
