using System.Collections.Generic;
using Models;
using SelectSystem;
using UnityEngine;

namespace Runtime_sets
{
    [CreateAssetMenu(fileName = "SelectedDotsSet.asset", menuName = "RuntimeSets/SelectedDots", order = 0)]
    public class SelectedDotsSet : RuntimeSet<KeyValuePair<Dot, Selectable>>
    {

    }
}
