using UnityEngine;

[AddComponentMenu("Player/Player Weapon")]
public class PlayerWeapon : MonoBehaviour
{
    const int MAX_WEAPON_INVENTORY_SLOTS = 1;
    const int MIN_WEAPON_INVENTORY_SLOTS = 0;

    private int _currentWeaponInventoryId;
    private Player _player;
    private WeaponItem _currentWeapon;
    private Transform _firePoint;
    private float next;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentWeapon = Database.Instance.GetWeaponById(ItemId.Pistol);
        _firePoint = GameObject.FindGameObjectWithTag("PlayerFirePoint").transform;
        next = 0.0F;
    }

    public void SwitchToRightWeapon()
    {
        if (_currentWeaponInventoryId < MAX_WEAPON_INVENTORY_SLOTS)
        {
            _currentWeaponInventoryId++;
        }

        GameManager.Instance.ChangePlayerModel(_currentWeaponInventoryId);
        _currentWeapon = Database.Instance.GetWeaponById(ItemId.Ak);
        _firePoint = GetCurrentFirePoint();
    }

    public void SwitchToLeftWeapon()
    {
        if (_currentWeaponInventoryId > MIN_WEAPON_INVENTORY_SLOTS)
        {
            _currentWeaponInventoryId--;
        }

        GameManager.Instance.ChangePlayerModel(_currentWeaponInventoryId);
        _currentWeapon = Database.Instance.GetWeaponById(ItemId.Pistol);
        _firePoint = GetCurrentFirePoint();
    }

    public void Use()
    {
        if (Time.time > next && Inventory.Instance.GetAmmoCount(_currentWeapon.bulletType) > 0)
        {
            next = Time.time + _currentWeapon.fireRate;
            GameObject bullet = ObjectPoolManager.Instance.GetNextPooledObjectByTag(_currentWeapon.bulletTag);
            bullet.SetActive(true);
            Inventory.Instance.Remove(_currentWeapon.bulletType, 1);

            if (bullet != null)
            {
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                bullet.transform.position = _firePoint.position;
                bullet.transform.rotation = _firePoint.rotation;

                bullet.SetActive(true);

                rb.AddForce(_firePoint.right * _currentWeapon.bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    private Transform GetCurrentFirePoint()
    {
        GameObject[] FirePoints = GameObject.FindGameObjectsWithTag("PlayerFirePoint");

        foreach (GameObject firePoint in FirePoints)
        {
            if (firePoint.gameObject.activeSelf == true)
            {
                return firePoint.transform;
            }
        }

        return null;
    }
}
