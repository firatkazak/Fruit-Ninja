using UnityEngine;
public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//Player tagýna sahip nesne(býçak) ile çarpýþýrsa;
        {
            FindObjectOfType<GameManager>().Explode();//GameManager scriptindeki Explode fonksiyonunu çalýþtýr.
        }
    }
}
