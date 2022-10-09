using UnityEngine;
public class Fruit : MonoBehaviour
{
    public GameObject whole;//Meyvenin b�t�n hali.
    public GameObject sliced;//Meyvenin dilimlenmi� hali.
    //Asl�nda meyve 3 par�a. 2 tane dilimlenmi�(alt ve �st), 1 tane tam. Biz dilimleyince 2 tane olan par�a �al���yor. Mant�k bu.
    private Rigidbody fruitRigidbody;//Meyve i�in rigidbody tan�mlad�k.
    private Collider fruitCollider;//Meyve i�in Collider tan�mlad�k.
    private ParticleSystem juiceParticleEffect;//Kesince suyu ��ks�n diye Partik�l tan�mlad�k.
    public int points = 1;//Skor ekleme i�in.
    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();//Komponent tan�tma.
        fruitCollider = GetComponent<Collider>();//Komponent tan�tma.
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();//Suyu meyvenin �ocu�u olacak �ekilde tan�tt�k.
    }
    private void Slice(Vector3 direction, Vector3 position, float force)//Meyve kesildi�inde olacak i�ler;
    {
        FindObjectOfType<GameManager>().IncreaseScore(points);//
        whole.SetActive(false);//B�t�n meyveyi kapat.
        sliced.SetActive(true);//Dilimlenmi� meyveyi aktif et.
        fruitCollider.enabled = false;//Meyvenin collider'ini false yap.
        juiceParticleEffect.Play();//Meyve suyu partik�l�n� oynat.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//Meyvenin kesildi�inde z ekseninde a��s�.
        sliced.transform.rotation = Quaternion.Euler(0, 0, angle);//Bu a��da dilimlenince hareket edecek.
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();//dilimli meyvelerin rb'sine eri�tik.
        foreach (Rigidbody slice in slices)//2 par�a var. O y�zden foreach ile i�lem yapt�k.
        {
            slice.velocity = fruitRigidbody.velocity;//b�t�n meyvenin h�z�n�, dilimli meyveye ata.
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);//dilimlere parantez i�indeki pozisyon ve y�nde g�� uygula.
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//B��a�a Player tag� atam��t�k. E�er meyve Player tagl� nesne ile �arp���rsa;
        {
            Blade blade = other.GetComponent<Blade>();//Blade scriptine eri�tik.
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
            //Slice fonksiyonunu b��a��n y�n�, pozisyonu ve g�c�nde �al��t�r.
            //Yani meyveler b��a��n etkisi ile ikiye ayr�l�p oraya buraya sa��lacaklar random pozisyon, a�� ve g��te.
        }
    }
}
