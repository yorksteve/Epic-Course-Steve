using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface IAttack
    {
        void Attack(bool attack);
        void Target(GameObject enemy);
        float Damage();
    }
}
  

