using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Stack<GameObject> Pool = new Stack<GameObject>();

    private void Start()
    {
        foreach (Transform t in this.transform)
        {
            Pool.Push(t.gameObject);
            t.gameObject.SetActive(false);
        }
        Debug.Log(string.Format("Pool length is {0}", Pool.Count));
    }
}
