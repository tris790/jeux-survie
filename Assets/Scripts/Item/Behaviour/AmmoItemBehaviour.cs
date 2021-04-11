using UnityEngine;

public class AmmoItemBehaviour : MonoBehaviour
{
    [SerializeField]
    private AmmoItem _ammoItem;

    private SpriteRenderer spriteRender;

    void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRender.sprite = Database.Instance.GetAmmoItems(ItemId.AkAmmo).itemIcon;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _ammoItem.Use();
            spriteRender.gameObject.SetActive(false);
        }
    }
}
