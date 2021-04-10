using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    public Heart(GameObject gameObject)
    {
        base.name = "heart";
        base.physicalItem = gameObject;
        base.pourcentAffichage = 1;
    }
}
