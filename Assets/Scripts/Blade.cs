using UnityEngine;
public class Blade : MonoBehaviour
{
    private bool slicing;//Kesiyor mu?
    private float minSliceVelocity = 0.01f;//Býçaðýn hareket hýzý.Aþaðýda if gibi kullandýk. Detaylý açýklamasý aþaðýda.
    public float sliceForce = 5.0f;//Kesim gücü. Fruit class'ýnda kullandýk.
    private Collider bladeCollider;//Býçaðýn collider'ý.
    private TrailRenderer bladeTrail;//Býçaðýn trail'i.
    private Camera mainCamera;//Kamera.
    public Vector3 direction { get; private set; }//Vektör 3 tipinde yön.Baþka class'lardan da eriþilecek.
    private void Awake()
    {
        mainCamera = Camera.main;//Kameranýn ana konumunu atadýk.
        //Bu sayede farenin konumu da kameranýn konumu da dünya pozisyonunda oldu. Çakýþma olmayacak.
        bladeCollider = GetComponent<Collider>();//Komponent atamasý.
        bladeTrail = GetComponentInChildren<TrailRenderer>();//Komponent atamasý.
    }
    private void OnEnable()
    {
        StopSlicing();//Burada mantýk hatasý yok. Kafan karýþmasýn.
        //Çünkü Býçak ilk enable olduðunda bir kesim iþlemi yok. Haliyle iz çýkarmasý, kesim iþlemi yapmasý mantýksýz olur.
    }
    private void OnDisable()
    {
        StopSlicing();//Keza býçak kapalýyken de haliyle kesim iþlemleri çalýþmamalý.
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//fareye bastýðýmýzda
        {
            StartSlicing();//Dilimlemeye baþla.
        }
        else if (Input.GetMouseButtonUp(0))//Fareden elimizi kaldýrdýðýmýzda
        {
            StopSlicing();//Dilimlemeyi durdur.
        }
        else if (slicing)//Eðer dilimliyorsa
        {
            ContinueSlicing();//Bu fonksiyonu çalýþtýracak.
        }
    }
    private void StartSlicing()//Kesim baþladýðýnda çalýþacak fonksiyon.
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Fare Screen Space, Býçak World Space'te. Fare'yi World Space'e çevirdik.
        newPosition.z = 0;//Býçak yukarý aþaðý ve saða sola gidecek. ileri geri gitmeyecek. o yüzden 0 yaptýk z'sini.
        transform.position = newPosition;//Yukarýda çevirdiðimiz mouse pozisyonunu, burada býçaðýn pozisyonuna atadýk.
        //Böylece fare ve býçak ayný yerde hareket edecek.
        slicing = true;//Kesiyor mu? evet.
        bladeCollider.enabled = true;//Býçaðýn collider komponentini açacak.
        bladeTrail.enabled = true;//Kesim izi için iz yapýcýnýn(TrailRenderer) komponentini açacak.
        bladeTrail.Clear();//Býçak izini temizle.
    }
    private void StopSlicing()//Kesim bittiðinde çalýþacak fonksiyon.
    {
        slicing = false;//Kesiyor mu? Hayýr.
        bladeCollider.enabled = false;//Býçaðýn Collider'ýný kapat.
        bladeTrail.enabled = false;//Býçaðýn Trail'ini kapat.(Kesim izi.)
    }
    private void ContinueSlicing()//Kesim iþlemi devam ederken çalýþacak fonksiyon.
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        ////Fare Screen Space, Býçak World Space'te. Fare'yi World Space'e çevirdik.
        newPosition.z = 0;//Býçak yukarý aþaðý ve saða sola gidecek. ileri geri gitmeyecek. o yüzden 0 yaptýk z'sini.
        direction = newPosition - transform.position;//Býçaðýn pozisyonundan, yeni pozisyonu çýkardýk.Mantýksal bir iþlem.
        //Mantýk: Böylece býçaðýn yönünü bulmuþ olduk.
        float velocity = direction.magnitude / Time.deltaTime;//Delta zamanýný yönün büyüklüðüne böldük.
        //Burada býçaðý hýzlýca çekince býçaðýn izi büyük, küçük kestikçe küçük olacak.Bunu yaptýk bu kod ile.
        bladeCollider.enabled = velocity > minSliceVelocity;//Býçaðýn hareket hýzý 0.1'den büyükse aktif olsun dedik.
        //Mantýk: Kesme iþlemi yaparken iz çýkmayacak. Kesme iþlemi yaparken iz çýkacak.
        transform.position = newPosition;//Yeni pozisyonu mevcut pozisyonumuz olarak atadýk. Bilerek en son bunu yazdýk.
        //Çünkü mantýken bize son konumu lazým. Ona göre iþlem yapacak. Býçak kesecek, iz çýkacak vs...
    }
}
