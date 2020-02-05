using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Delegates;

namespace Venus.QuestSystem
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuestObject
    {
        VoidDelegate GetQuestDoneDelegate();

        void QuestDone();

        string GetQuestStringID();
    }
}