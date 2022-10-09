using System.Collections;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    private Collider spawnArea;//Meyve Spawnlanacak alan.
    public GameObject[] fruitPrefabs;//Meyve prefablar�n� koyaca��m�z array.
    public GameObject bombPrefab;//Bomba prefab� i�in.
    [Range(0f, 1f)] public float bombChance = 0.05f;//0.0 ile 1.0 aras�nda bir de�er verdik. O aral�ktaki bir �ans ile bomba ��kacak.
    public float minSpawnDelay = 0.25f;//Meyve spawn'� i�in min saniye
    public float maxSpawnDelay = 1.0f;//Meyve spawn'� i�in max saniye. 0.25 ile 1 sn aras�nda spawnlanacak.
    public float minAngle = -15.0f;//Meyveleri havaya att���m�zda d�md�z gelmez. Bunun i�in farkl� a��larda gelsin diye yapt�k.
    public float maxAngle = 15.0f;//B�ylece ger�eklik katt�k minAngle ve maxAngle ile.
    public float minForce = 18.0f;//Meyvenin f�rlat�laca�� min g��. min'de �ok fazla yukar� gitmeyecek.
    public float maxForce = 22.0f;//Meyvenin f�rlat�laca�� max g��. max'ta daha yukar� gidecek meyve.
    public float maxLifeTime = 5.0f;//Meyvenin oyunda kalaca�� s�re. 5 saniyede meyveyi kesmemiz gerek.
    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();//Komponent atamas�.
    }
    private void OnEnable()//Spawner aktifken �al��acak.
    {
        StartCoroutine(Spawn());//Spawn fonksiyonunu belirli aral�klarla �al��t�raca�� i�in coroutine i�ine yazd�k.
    }
    private void OnDisable()//Spawner aktif de�ilken �al��acak.
    {
        StopAllCoroutines();//Coroutine'leri durduracak.
    }
    private IEnumerator Spawn()//Spawn
    {
        yield return new WaitForSeconds(2.0f);//2 sn'de 1 d�nd�recek.
        while (enabled)//enabled oldu�u s�rece.
        {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            //prefab ad�nda fruitPrefabs'�n boyutu aral���nda bir prefab daha yaratt�k.
            if (Random.value < bombChance)//Rastgele de�er, bomba �ans�ndan b�y�k ise;
            {
                prefab = bombPrefab;//Bomba prefab�n� meyve prefab�na ata.
                //B�ylece bomba gelmi� olacak arada bir. Onu kesersek oyun bitecek.
            }
            Vector3 position = new Vector3();//position ad�nda Vector3 tan�mlad�k.
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            //position'un x,y ve z'sini bu �ekilde ayarlad�k.
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(minAngle, maxAngle));
            //rotation ad�nda random a��da t�reyecek bir nesne yaratt�k.
            GameObject fruit = Instantiate(prefab, position, rotation);
            //�stte tan�mlad���m�z prefabdan, yine �stte tan�mlad���m�z pozisyon ve a��da �retecek.
            Destroy(fruit, maxLifeTime);//Meyveyi 5 saniye sonra yok edecek. 5 sn'de kesmeliyiz.
            float force = Random.Range(minForce, maxForce);//Meyvenin yukar� f�rlama g�c�.
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
            //�stte tan�mlad���m�z force g�c�nde meyvenin rigidbody'sine yukar� do�ru g�� uygulanacak.
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            //Minimum �eyrek saniye, maksimum 1 saniyede 1 meyve spawnlanacak.
        }
    }
}
