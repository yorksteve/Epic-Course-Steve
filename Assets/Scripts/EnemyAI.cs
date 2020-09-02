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
    //[SerializeField] private int _money = 500;
    [SerializeField] private int _mechMoney;

    private float _reachRange = 1.0f;
    private float _distanceFromDes;

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
        DestinationReached();
    }

    void DestinationReached()
    {
        _distanceFromDes = Vector3.Distance(transform.position, _destination);
        if (_distanceFromDes <= _reachRange)
        {
            Debug.Log("Destination Reached");
            this.gameObject.SetActive(false);
            // Reduce player's life count
        }
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
