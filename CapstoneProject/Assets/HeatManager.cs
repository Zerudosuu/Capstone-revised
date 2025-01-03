using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatManager : MonoBehaviour
{
    public delegate void HeatTransferHandler(float heatAmount);
    public static event HeatTransferHandler OnHeatTransfer;

    public static void TransferHeat(float heatAmount, Transform target)
    {
        OnHeatTransfer?.Invoke(heatAmount);

        Slot slot = target.GetComponent<Slot>();
        if (slot != null)
        {
            slot.PropagateHeat(heatAmount);
        }
    }
}
