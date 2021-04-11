using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    private GameObject _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = GameManager.Instance.player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _startPosition.transform.position) > 132)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HealthComponent>().AddOrRemove(-Damage);
            gameObject.SetActive(false);
        }

        if (collision.gameObject.tag != "CurrentPlayerModel")
        {
            gameObject.SetActive(false);
        }
    }
}
