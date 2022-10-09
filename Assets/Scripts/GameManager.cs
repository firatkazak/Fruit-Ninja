using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;//Skor tutucu Text.
    public Image fadeImage;//Oyun bitip yeniden baþlarken geçiþ resmi.
    private Blade blade;//Blade class'ýnda iþlem yapacaðýz.
    private Spawner spawner;//Spawner class'ýnda iþlem yapacaðýz.
    private int score; //Skor tutucu deðiþken.
    private void Awake()
    {
        blade = FindObjectOfType<Blade>();//Komponent atamasý.
        spawner = FindObjectOfType<Spawner>();//Komponent atamasý.
    }
    private void Start()
    {
        NewGame();//Baþlangýçta NewGame fonksiyonunu çalýþtýrýyoruz.
    }
    private void NewGame()//Yeni oyun fonksiyonu.
    {
        Time.timeScale = 1.0f;//
        blade.enabled = true;//Býçaðý aktif et.
        spawner.enabled = true;//Meyve spawn etmeye baþla.
        score = 0;//Skoru 0 yap.
        scoreText.text = scoreText.ToString();//Skoru string'e çevirecek ekrana yazdýrabilmek için.
        ClearScene();//Sahneyi temizle. Baþlangýç haline getirecek.
    }
    public void ClearScene()//Sahneyi temizleme fonksiyonu.
    {
        //Bütün meyveleri ve bombayý yok edecek. Tek tek yazmadým.
        Fruit[] fruits = FindObjectsOfType<Fruit>();
        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
        //Bütün meyveleri ve bombalarý yok edecek. Tek tek yazmadým.
    }
    public void IncreaseScore(int amount)//Fruit class'ýndan eriþtik. Meyve kesilince bu fonksiyon çalýþacak.
    {
        score += amount;//Bu iþlem ile her kesimde skora +1 eklenecek.
        scoreText.text = score.ToString();//Skoru string'e çevir.
    }
    public void Explode()//Bombayý kestiðimizde olacaklar. (Oyun bitecek);
    {
        blade.enabled = false;//Býçaðý kapat.
        spawner.enabled = false;//Meyve spawnlamayý durdur.
        StartCoroutine(ExplodeSequence());//ExplodeSequence fonksiyonunu çalýþtýr.
    }
    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;//elapsed'ten duration'a yarým saniye geçecek.
        float duration = 0.5f;//Yarým saniyelik bir fade geçiþi yapacaðýz.
        while (elapsed < duration)//Yarým saniye boyunca aþaðýdaki kod çalýþacak;
        {
            float t = Mathf.Clamp01(elapsed / duration);//duration ile elapsed arasýnda geçen süreyi hesapladýk.
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);//Temizleyip beyaza geçecek.
            //t zamanda ekraný temizleyip beyaz renge geçirecek.
            Time.timeScale = 1f - t;//Geçen süreyi 1'den çýkarttýk.
            elapsed += Time.unscaledDeltaTime;//Son kareden geçen süreyi elapsed'e atadýk.
            yield return null;//null döndür.
        }
        yield return new WaitForSecondsRealtime(1f);//1 saniye döndür.
        NewGame();//Yeni oyun baþlat.
        elapsed = 0f;//geçen süreyi sýfýrla.
        while (elapsed < duration)//Yukarýdaki ile ayný.
        {
            float t = Mathf.Clamp01(elapsed / duration);//Yukarýdaki ile ayný.
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);//Yukarýda beyaz olmuþtu, þimdi temizleyip ana ekrana dönecek.
            elapsed += Time.unscaledDeltaTime;//Yukarýdaki ile ayný.
            yield return null;//Yukarýdaki ile ayný.
        }
    }
}
