using System.Collections;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    private Collider spawnArea;//Meyve Spawnlanacak alan.
    public GameObject[] fruitPrefabs;//Meyve prefablarýný koyacaðýmýz array.
    public GameObject bombPrefab;//Bomba prefabý için.
    [Range(0f, 1f)] public float bombChance = 0.05f;//0.0 ile 1.0 arasýnda bir deðer verdik. O aralýktaki bir þans ile bomba çýkacak.
    public float minSpawnDelay = 0.25f;//Meyve spawn'ý için min saniye
    public float maxSpawnDelay = 1.0f;//Meyve spawn'ý için max saniye. 0.25 ile 1 sn arasýnda spawnlanacak.
    public float minAngle = -15.0f;//Meyveleri havaya attýðýmýzda dümdüz gelmez. Bunun için farklý açýlarda gelsin diye yaptýk.
    public float maxAngle = 15.0f;//Böylece gerçeklik kattýk minAngle ve maxAngle ile.
    public float minForce = 18.0f;//Meyvenin fýrlatýlacaðý min güç. min'de çok fazla yukarý gitmeyecek.
    public float maxForce = 22.0f;//Meyvenin fýrlatýlacaðý max güç. max'ta daha yukarý gidecek meyve.
    public float maxLifeTime = 5.0f;//Meyvenin oyunda kalacaðý süre. 5 saniyede meyveyi kesmemiz gerek.
    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();//Komponent atamasý.
    }
    private void OnEnable()//Spawner aktifken çalýþacak.
    {
        StartCoroutine(Spawn());//Spawn fonksiyonunu belirli aralýklarla çalýþtýracaðý için coroutine içine yazdýk.
    }
    private void OnDisable()//Spawner aktif deðilken çalýþacak.
    {
        StopAllCoroutines();//Coroutine'leri durduracak.
    }
    private IEnumerator Spawn()//Spawn
    {
        yield return new WaitForSeconds(2.0f);//2 sn'de 1 döndürecek.
        while (enabled)//enabled olduðu sürece.
        {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            //prefab adýnda fruitPrefabs'ýn boyutu aralýðýnda bir prefab daha yarattýk.
            if (Random.value < bombChance)//Rastgele deðer, bomba Þansýndan büyük ise;
            {
                prefab = bombPrefab;//Bomba prefabýný meyve prefabýna ata.
                //Böylece bomba gelmiþ olacak arada bir. Onu kesersek oyun bitecek.
            }
            Vector3 position = new Vector3();//position adýnda Vector3 tanýmladýk.
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            //position'un x,y ve z'sini bu þekilde ayarladýk.
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(minAngle, maxAngle));
            //rotation adýnda random açýda türeyecek bir nesne yarattýk.
            GameObject fruit = Instantiate(prefab, position, rotation);
            //Üstte tanýmladýðýmýz prefabdan, yine üstte tanýmladýðýmýz pozisyon ve açýda üretecek.
            Destroy(fruit, maxLifeTime);//Meyveyi 5 saniye sonra yok edecek. 5 sn'de kesmeliyiz.
            float force = Random.Range(minForce, maxForce);//Meyvenin yukarý fýrlama gücü.
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
            //Üstte tanýmladýðýmýz force gücünde meyvenin rigidbody'sine yukarý doðru güç uygulanacak.
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            //Minimum çeyrek saniye, maksimum 1 saniyede 1 meyve spawnlanacak.
        }
    }
}
