using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxItems : MonoBehaviour
{
    [SerializeField] Box box;
    [SerializeField] List<Transform> boxBreakList;
    [SerializeField] List<Transform> itemList;

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

        if (box.GetBoxType() != Box.BoxType.Bouncing)
        {
            itemList.ForEach(item => SpawnItem(item));
        }
    }

    private void SpawnItem(Transform item)
    {
        Transform itemTransform = Instantiate(item, transform);

        Rigidbody2D rb = itemTransform.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 5f;
        rb.freezeRotation = true;

        float randomPower = UnityEngine.Random.Range(-12f, 12f);
        rb.velocity = new Vector2(randomPower, 10f);
    }

    private void SpawnBoxBreak(Transform boxBreak)
    {
        Transform boxBreakTransform = Instantiate(boxBreak, transform);
        Rigidbody2D rb = boxBreakTransform.GetComponent<Rigidbody2D>();
        float randomPower = UnityEngine.Random.Range(-16f, 16f);
        rb.velocity = new Vector2(randomPower, 10f);
    }
}
