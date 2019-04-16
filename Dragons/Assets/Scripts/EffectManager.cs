using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private  Coroutine _currentCoroutine;
    [SerializeField] private Mesh _newMesh;

    public void FlipRange(HexPoint[] range)
    {
        _currentCoroutine = StartCoroutine(InvokeFunction(range));
    }

    private IEnumerator InvokeFunction(HexPoint[] range)
    {
        int index = 0;
        int temp = 1;

        while (index <= 5)
        {
            foreach (HexPoint p in range)
            {
                GlobalGameManager.instance.Map[p].Levitate(temp, _newMesh);
            }
            temp = (temp + 1) % 5;
            index++;
            yield return null;
        }

        StopCoroutine(_currentCoroutine);
    }
}
