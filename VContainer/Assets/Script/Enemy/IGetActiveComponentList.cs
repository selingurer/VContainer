using System.Collections.Generic;
using UnityEngine;

public interface IGetActiveComponentList
{
    IEnumerable<Component> GetActiveList();
}