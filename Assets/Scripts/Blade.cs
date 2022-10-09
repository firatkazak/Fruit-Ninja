using UnityEngine;
public class Blade : MonoBehaviour
{
    private bool slicing;//Kesiyor mu?
    private float minSliceVelocity = 0.01f;//B��a��n hareket h�z�.A�a��da if gibi kulland�k. Detayl� a��klamas� a�a��da.
    public float sliceForce = 5.0f;//Kesim g�c�. Fruit class'�nda kulland�k.
    private Collider bladeCollider;//B��a��n collider'�.
    private TrailRenderer bladeTrail;//B��a��n trail'i.
    private Camera mainCamera;//Kamera.
    public Vector3 direction { get; private set; }//Vekt�r 3 tipinde y�n.Ba�ka class'lardan da eri�ilecek.
    private void Awake()
    {
        mainCamera = Camera.main;//Kameran�n ana konumunu atad�k.
        //Bu sayede farenin konumu da kameran�n konumu da d�nya pozisyonunda oldu. �ak��ma olmayacak.
        bladeCollider = GetComponent<Collider>();//Komponent atamas�.
        bladeTrail = GetComponentInChildren<TrailRenderer>();//Komponent atamas�.
    }
    private void OnEnable()
    {
        StopSlicing();//Burada mant�k hatas� yok. Kafan kar��mas�n.
        //��nk� B��ak ilk enable oldu�unda bir kesim i�lemi yok. Haliyle iz ��karmas�, kesim i�lemi yapmas� mant�ks�z olur.
    }
    private void OnDisable()
    {
        StopSlicing();//Keza b��ak kapal�yken de haliyle kesim i�lemleri �al��mamal�.
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//fareye bast���m�zda
        {
            StartSlicing();//Dilimlemeye ba�la.
        }
        else if (Input.GetMouseButtonUp(0))//Fareden elimizi kald�rd���m�zda
        {
            StopSlicing();//Dilimlemeyi durdur.
        }
        else if (slicing)//E�er dilimliyorsa
        {
            ContinueSlicing();//Bu fonksiyonu �al��t�racak.
        }
    }
    private void StartSlicing()//Kesim ba�lad���nda �al��acak fonksiyon.
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Fare Screen Space, B��ak World Space'te. Fare'yi World Space'e �evirdik.
        newPosition.z = 0;//B��ak yukar� a�a�� ve sa�a sola gidecek. ileri geri gitmeyecek. o y�zden 0 yapt�k z'sini.
        transform.position = newPosition;//Yukar�da �evirdi�imiz mouse pozisyonunu, burada b��a��n pozisyonuna atad�k.
        //B�ylece fare ve b��ak ayn� yerde hareket edecek.
        slicing = true;//Kesiyor mu? evet.
        bladeCollider.enabled = true;//B��a��n collider komponentini a�acak.
        bladeTrail.enabled = true;//Kesim izi i�in iz yap�c�n�n(TrailRenderer) komponentini a�acak.
        bladeTrail.Clear();//B��ak izini temizle.
    }
    private void StopSlicing()//Kesim bitti�inde �al��acak fonksiyon.
    {
        slicing = false;//Kesiyor mu? Hay�r.
        bladeCollider.enabled = false;//B��a��n Collider'�n� kapat.
        bladeTrail.enabled = false;//B��a��n Trail'ini kapat.(Kesim izi.)
    }
    private void ContinueSlicing()//Kesim i�lemi devam ederken �al��acak fonksiyon.
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        ////Fare Screen Space, B��ak World Space'te. Fare'yi World Space'e �evirdik.
        newPosition.z = 0;//B��ak yukar� a�a�� ve sa�a sola gidecek. ileri geri gitmeyecek. o y�zden 0 yapt�k z'sini.
        direction = newPosition - transform.position;//B��a��n pozisyonundan, yeni pozisyonu ��kard�k.Mant�ksal bir i�lem.
        //Mant�k: B�ylece b��a��n y�n�n� bulmu� olduk.
        float velocity = direction.magnitude / Time.deltaTime;//Delta zaman�n� y�n�n b�y�kl���ne b�ld�k.
        //Burada b��a�� h�zl�ca �ekince b��a��n izi b�y�k, k���k kestik�e k���k olacak.Bunu yapt�k bu kod ile.
        bladeCollider.enabled = velocity > minSliceVelocity;//B��a��n hareket h�z� 0.1'den b�y�kse aktif olsun dedik.
        //Mant�k: Kesme i�lemi yaparken iz ��kmayacak. Kesme i�lemi yaparken iz ��kacak.
        transform.position = newPosition;//Yeni pozisyonu mevcut pozisyonumuz olarak atad�k. Bilerek en son bunu yazd�k.
        //��nk� mant�ken bize son konumu laz�m. Ona g�re i�lem yapacak. B��ak kesecek, iz ��kacak vs...
    }
}
