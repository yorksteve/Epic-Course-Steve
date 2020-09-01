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
    [SerializeField] private ParticleSystem _dust;

    public int health;
    [SerializeField] private int _money = 500;
    [SerializeField] private int _mechMoney;

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

    void TakeDamage()
    {
        //health -= damageAmount;

        if (health <= 0)
        {
            // Kill mech and get money
            _anim.SetBool("Die", true);
            this.gameObject.SetActive(false);

            _money += _mechMoney;
        }
    }

    void Attack()
    {
        _anim.SetBool("Attack", true);
    }
}
