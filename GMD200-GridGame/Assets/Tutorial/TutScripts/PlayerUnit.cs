using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private TutGridManager gridManager;
    [SerializeField] private float moveSpeed = 2;

    private void OnEnable()
    {
        gridManager.TileSelected += OnTileSelected;
    }

    private void OnDisable()
    {
        gridManager.TileSelected -= OnTileSelected;
    }

    private void OnTileSelected(TutGridTile obj)
    {
        StopAllCoroutines();
        StartCoroutine(Co_MoveTo(obj.transform.position));
    }

    private IEnumerator Co_MoveTo(Vector3 targetPostion)
    {
        while (Vector3.Distance(transform.position, targetPostion) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPostion, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPostion;
    }
}
