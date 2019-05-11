using UnityEngine;
using System.Collections;

public class DrawHelper : MonoBehaviour
{
    void OnDrawGizmos()
    {
        if (StatPara.ShowHelper)
        {
            Vector3 center = StatPara.Size;
            center.x *= 0.5f;
            center.y = StatPara.Height;
            center.z *= 0.5f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, new Vector3(StatPara.BattleFieldSize, 1, StatPara.BattleFieldSize));
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, new Vector3(StatPara.CitySize, 1, StatPara.CitySize));
        }
    }
}
