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
    }

    private void Box_OnBoxBreaked(object sender, EventArgs e)
    {
        // Spawn box breaks
        Rigidbody2D rb;
        float randomPower;

        boxBreakList.ForEach(boxBreak =>
        {
            Transform boxBreakTransform = Instantiate(boxBreak, transform);
            rb = boxBreakTransform.GetComponent<Rigidbody2D>();
            randomPower = UnityEngine.Random.Range(-16f, 16f);
            rb.velocity = new Vector2(randomPower, 10f);
        });

        itemList.ForEach(item =>
        {
            Transform itemTransform = Instantiate(item, transform);
            rb = itemTransform.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 5f;
            rb.freezeRotation = true;
            randomPower = UnityEngine.Random.Range(-12f, 12f);
            rb.velocity = new Vector2(randomPower, 10f);
        });
    }
}
