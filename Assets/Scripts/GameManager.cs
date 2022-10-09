using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;//Skor tutucu Text.
    public Image fadeImage;//Oyun bitip yeniden ba�larken ge�i� resmi.
    private Blade blade;//Blade class'�nda i�lem yapaca��z.
    private Spawner spawner;//Spawner class'�nda i�lem yapaca��z.
    private int score; //Skor tutucu de�i�ken.
    private void Awake()
    {
        blade = FindObjectOfType<Blade>();//Komponent atamas�.
        spawner = FindObjectOfType<Spawner>();//Komponent atamas�.
    }
    private void Start()
    {
        NewGame();//Ba�lang��ta NewGame fonksiyonunu �al��t�r�yoruz.
    }
    private void NewGame()//Yeni oyun fonksiyonu.
    {
        Time.timeScale = 1.0f;//
        blade.enabled = true;//B��a�� aktif et.
        spawner.enabled = true;//Meyve spawn etmeye ba�la.
        score = 0;//Skoru 0 yap.
        scoreText.text = scoreText.ToString();//Skoru string'e �evirecek ekrana yazd�rabilmek i�in.
        ClearScene();//Sahneyi temizle. Ba�lang�� haline getirecek.
    }
    public void ClearScene()//Sahneyi temizleme fonksiyonu.
    {
        //B�t�n meyveleri ve bombay� yok edecek. Tek tek yazmad�m.
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
        //B�t�n meyveleri ve bombalar� yok edecek. Tek tek yazmad�m.
    }
    public void IncreaseScore(int amount)//Fruit class'�ndan eri�tik. Meyve kesilince bu fonksiyon �al��acak.
    {
        score += amount;//Bu i�lem ile her kesimde skora +1 eklenecek.
        scoreText.text = score.ToString();//Skoru string'e �evir.
    }
    public void Explode()//Bombay� kesti�imizde olacaklar. (Oyun bitecek);
    {
        blade.enabled = false;//B��a�� kapat.
        spawner.enabled = false;//Meyve spawnlamay� durdur.
        StartCoroutine(ExplodeSequence());//ExplodeSequence fonksiyonunu �al��t�r.
    }
    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;//elapsed'ten duration'a yar�m saniye ge�ecek.
        float duration = 0.5f;//Yar�m saniyelik bir fade ge�i�i yapaca��z.
        while (elapsed < duration)//Yar�m saniye boyunca a�a��daki kod �al��acak;
        {
            float t = Mathf.Clamp01(elapsed / duration);//duration ile elapsed aras�nda ge�en s�reyi hesaplad�k.
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);//Temizleyip beyaza ge�ecek.
            //t zamanda ekran� temizleyip beyaz renge ge�irecek.
            Time.timeScale = 1f - t;//Ge�en s�reyi 1'den ��kartt�k.
            elapsed += Time.unscaledDeltaTime;//Son kareden ge�en s�reyi elapsed'e atad�k.
            yield return null;//null d�nd�r.
        }
        yield return new WaitForSecondsRealtime(1f);//1 saniye d�nd�r.
        NewGame();//Yeni oyun ba�lat.
        elapsed = 0f;//ge�en s�reyi s�f�rla.
        while (elapsed < duration)//Yukar�daki ile ayn�.
        {
            float t = Mathf.Clamp01(elapsed / duration);//Yukar�daki ile ayn�.
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);//Yukar�da beyaz olmu�tu, �imdi temizleyip ana ekrana d�necek.
            elapsed += Time.unscaledDeltaTime;//Yukar�daki ile ayn�.
            yield return null;//Yukar�daki ile ayn�.
        }
    }
}
