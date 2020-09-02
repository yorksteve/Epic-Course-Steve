using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Manager;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _destination;
    private Animator _anim;

    public int health;
    //[SerializeField] private int _money = 500;
    [SerializeField] private int _mechMoney;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

        _destination = SpawnManager.Instance.RequestDestination();
        _agent.SetDestination(_destination.position);
    }

    void Health()
    {
        //health -= damageAmount; (dependant upon the weapon)

        if (health <= 0)
        {
            // Kill mech and get money
            health = 0;
            _anim.SetBool("Die", true);
            this.gameObject.SetActive(false);

            // Increase War Fund based on value of mech destroyed
        }
    }


    // Mechs can attack soldiers placed in the field (to be added later...probably)
    void Attack()
    {
        _anim.SetBool("Attack", true);
    }
}
