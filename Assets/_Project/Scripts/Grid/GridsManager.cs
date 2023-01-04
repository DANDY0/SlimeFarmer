using Sirenix.OdinInspector;
using UnityEngine;
public class GridsManager : Singleton<GridsManager>
{
    public GridContainer[] Grids;

    [Button]
    private void SetRefs()
    {
        Grids = GetComponentsInChildren<GridContainer>();
        for (int i = 0; i < Grids.Length; i++)
            Grids[i].GridID = i;
    }
    
    
}
