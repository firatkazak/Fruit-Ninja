using UnityEngine;
public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//Player tag�na sahip nesne(b��ak) ile �arp���rsa;
        {
            FindObjectOfType<GameManager>().Explode();//GameManager scriptindeki Explode fonksiyonunu �al��t�r.
        }
    }
}
