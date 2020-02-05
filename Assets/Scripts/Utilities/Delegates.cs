using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venus.Delegates
{
    public delegate void VoidDelegate();

    public delegate void UiStateUpdateDelegate(UiStateEnum uiState);

    public delegate void Vector3Delegate(Vector3 vector3);

    public delegate void RotationDelegate(Quaternion quaternion);

    public delegate void QuestChangeDelegate(string questStringID);
}