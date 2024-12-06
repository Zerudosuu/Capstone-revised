using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTransform : MonoBehaviour
{
    public List<Transform> TransformList;

    public List<string> PossibleDropAreas;

    [SerializeField]
    public Item item;

    public void SetItem(Item item)
    {
        this.item = item;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        print(
            "Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name
        );
    }
}
