using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interfaces
{
    public interface ITower
    {
        int WarFundsRequired { get; set; }
        GameObject CurrentModel { get; set; }
        GameObject UpgradeModel { get; }
    }
}

