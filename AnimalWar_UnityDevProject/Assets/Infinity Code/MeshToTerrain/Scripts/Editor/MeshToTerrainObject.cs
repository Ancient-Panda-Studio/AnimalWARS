/*       INFINITY CODE 2013 - 2019         */
/*     http://www.infinity-code.com        */

using UnityEngine;

public class MeshToTerrainObject
{
    public GameObject gameobject;
    public int layer;
    public MeshCollider tempCollider;
    public Transform originalParent;

    public MeshToTerrainObject(GameObject gameObject)
    {
        gameobject = gameObject;
        layer = gameObject.layer;
    }
}