using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface IAttack
    {
        void Attack();
        void Target(GameObject enemy);
    }
}
  

