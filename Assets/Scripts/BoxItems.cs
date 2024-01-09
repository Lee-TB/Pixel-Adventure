using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxItems : MonoBehaviour
{
    [SerializeField] Transform boxItemsTransform;
    [SerializeField] List<Transform> boxBreakList;
    [SerializeField] List<Transform> itemList;
    [SerializeField] float explosionRadius = 8f;
    [SerializeField] float explosionHeight = 10f;

    private Box box;

    private void Awake()
    {
        box = GetComponent<Box>();
    }

    private void Start()
    {
        box.OnBoxBreaked += Box_OnBoxBreaked;
        box.OnBoxBouncing += Box_OnBoxBouncing;
    }

    private void Box_OnBoxBouncing(object sender, EventArgs e)
    {
        if (itemList.Count > 0) SpawnItem(itemList[UnityEngine.Random.Range(0, itemList.Count)]);
    }

    private void Box_OnBoxBreaked(object sender, EventArgs e)
    {
        boxBreakList.ForEach(boxBreak => SpawnBoxBreak(boxBreak));

        // Spawn an Item every waitTime seconds.
        float waitTime = 0.04f;
        if (box.GetBoxType() != Box.BoxType.Bouncing)
        {
            StartCoroutine(SpawnItemRoutine(itemList, waitTime));
        }
    }

    private IEnumerator SpawnItemRoutine(List<Transform> item, float waitTime)
    {
        for (int i = 0; i < item.Count; i++)
        {
            yield return new WaitForSeconds(waitTime);
            SpawnItem(item[i]);
        };
    }

    private void SpawnItem(Transform item)
    {
        Transform itemTransform = Instantiate(item, boxItemsTransform);

        Rigidbody2D rb = itemTransform.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 5f;
        rb.freezeRotation = true;

        CircleCollider2D[] circleCollider2DArray = itemTransform.GetComponents<CircleCollider2D>();
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask nothingMask = LayerMask.GetMask("Nothing");
        float waitTime = 0.2f;
        foreach (var collider2D in circleCollider2DArray)
        {
            if (collider2D.isTrigger == true)
            {
                collider2D.excludeLayers = playerMask;
                StartCoroutine(WaitRoutine(() => collider2D.excludeLayers = nothingMask, waitTime));
            }
        }

        float randPower = UnityEngine.Random.Range(-explosionRadius, explosionRadius);
        rb.velocity = new Vector2(randPower, explosionHeight);
    }

    private IEnumerator WaitRoutine(Action action, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    private void SpawnBoxBreak(Transform boxBreak)
    {
        Transform boxBreakTransform = Instantiate(boxBreak, boxItemsTransform);
        Rigidbody2D rb = boxBreakTransform.GetComponent<Rigidbody2D>();

        float randPower = UnityEngine.Random.Range(-explosionRadius, explosionRadius);
        rb.velocity = new Vector2(randPower, explosionHeight);
    }


}
