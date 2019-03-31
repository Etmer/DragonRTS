using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject _TestObject;
    private Coroutine _currentCoroutine;

    public void MoveUnit(Unit unit, HexPoint[] line)
    {
        _TestObject.transform.position = CoordinateSystem.HexPointToWorldCoordinate(line[0]);
        _currentCoroutine = StartCoroutine(Move(_TestObject.transform, line));
    }

    private IEnumerator Move(Transform current, HexPoint[] targets)
    {
        current.position = CoordinateSystem.HexPointToWorldCoordinate(targets[0]) + Vector3.up * 0.8f;
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 currentTarget = CoordinateSystem.HexPointToWorldCoordinate(targets[i]) + Vector3.up * 0.8f;
            Vector3 direction = currentTarget - current.position;

            while (current.rotation != Quaternion.LookRotation(direction))
            {
                current.rotation = Quaternion.RotateTowards(current.rotation, Quaternion.LookRotation(direction), 3f);
                yield return null;
            }
            while (current.position != currentTarget)
            {
                current.position = Vector3.MoveTowards(current.position, currentTarget, 3 * Time.deltaTime);
                yield return null;
            }
        }
    }
}
