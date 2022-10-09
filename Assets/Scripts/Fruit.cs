using UnityEngine;
public class Fruit : MonoBehaviour
{
    public GameObject whole;//Meyvenin bütün hali.
    public GameObject sliced;//Meyvenin dilimlenmiþ hali.
    //Aslýnda meyve 3 parça. 2 tane dilimlenmiþ(alt ve üst), 1 tane tam. Biz dilimleyince 2 tane olan parça çalýþýyor. Mantýk bu.
    private Rigidbody fruitRigidbody;//Meyve için rigidbody tanýmladýk.
    private Collider fruitCollider;//Meyve için Collider tanýmladýk.
    private ParticleSystem juiceParticleEffect;//Kesince suyu çýksýn diye Partikül tanýmladýk.
    public int points = 1;//Skor ekleme için.
    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();//Komponent tanýtma.
        fruitCollider = GetComponent<Collider>();//Komponent tanýtma.
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();//Suyu meyvenin çocuðu olacak þekilde tanýttýk.
    }
    private void Slice(Vector3 direction, Vector3 position, float force)//Meyve kesildiðinde olacak iþler;
    {
        FindObjectOfType<GameManager>().IncreaseScore(points);//
        whole.SetActive(false);//Bütün meyveyi kapat.
        sliced.SetActive(true);//Dilimlenmiþ meyveyi aktif et.
        fruitCollider.enabled = false;//Meyvenin collider'ini false yap.
        juiceParticleEffect.Play();//Meyve suyu partikülünü oynat.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//Meyvenin kesildiðinde z ekseninde açýsý.
        sliced.transform.rotation = Quaternion.Euler(0, 0, angle);//Bu açýda dilimlenince hareket edecek.
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();//dilimli meyvelerin rb'sine eriþtik.
        foreach (Rigidbody slice in slices)//2 parça var. O yüzden foreach ile iþlem yaptýk.
        {
            slice.velocity = fruitRigidbody.velocity;//bütün meyvenin hýzýný, dilimli meyveye ata.
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);//dilimlere parantez içindeki pozisyon ve yönde güç uygula.
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//Býçaða Player tagý atamýþtýk. Eðer meyve Player taglý nesne ile çarpýþýrsa;
        {
            Blade blade = other.GetComponent<Blade>();//Blade scriptine eriþtik.
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
            //Slice fonksiyonunu býçaðýn yönü, pozisyonu ve gücünde çalýþtýr.
            //Yani meyveler býçaðýn etkisi ile ikiye ayrýlýp oraya buraya saçýlacaklar random pozisyon, açý ve güçte.
        }
    }
}
