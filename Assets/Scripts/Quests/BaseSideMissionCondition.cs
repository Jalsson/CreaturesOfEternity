using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSideMissionCondition : MonoBehaviour {

        public virtual bool CheckKillInfo(WeaponClass weaponClass)
    {
        return false;
    }
}
