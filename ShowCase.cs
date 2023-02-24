using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShowCase : Item
{
    public abstract override void Use();

    public GameObject bulletImpactPrefab;
}
