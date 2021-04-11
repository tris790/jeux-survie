using UnityEngine;

public class HealthItemBehaviour : MonoBehaviour
{
    [SerializeField]
    private ConsumableItem _consumableItem;

    private SpriteRenderer spriteRender;

    void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRender.sprite = Database.Instance.GetConsumableItem(ItemId.TroussePS).itemIcon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _consumableItem.Use();
            spriteRender.gameObject.SetActive(false);
        }
    }
}