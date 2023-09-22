using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    //Oyunun parasal iþlemlerini yapacaðýz.

    public float money; //1- maaliyetini hesaplayacaðýz.
    public int day; //2- Ýþlem yaptýkça gün gün ilerleyecek.
    public int curPopulation; //3- Þuanki nüfus ne kadar
    public int curJobs; //4- Fabrikalar kuruldukça iþlere bakýcaz.
    public int curFood; //5- Besin olarak çiftlik yaptýkça çiftlik durumunu gözlemleyeceðiz.
    public int maxPopulation; //6- Max nüfusu tutacaðýz.
    public int maxJobs; //7- Ne kadar iþ olabilir.
    public int incomePerJob; //8- Ýþ baþýna ne kadar gelir elde ediyoruz.
    public TextMeshProUGUI statsText; //9- Ýstatistikleri tuttuðumuz textimiz.
    public List<Building> buildings = new List<Building>(); //10-Ekleyeceðimiz yapýlarý listeledik.

    public static City instance; //11- Þehri heryerden çaðýrabilmek için

    public GameObject gameOverPanel;
    private bool gameStart;

    public float bankrupt;
    public float payBankrupt;
    public TextMeshProUGUI leftBankrupt;

    private void Awake()
    {
        instance = this; //12- Bunu kim çaðýrýyorsa, doðrudan gelip eriþsin.
    }

    private void Start()
    {
        UpdateStatText(); //49
        gameStart = false;
    }

    public void OnPlaceBuilding(Building building)
    {//13- Her bina yaptýkça cost diye bir maaliyet eklemiþtik.
        money -= building.preset.cost; //14-Her yaptýðýmýz þeyin maaliyetinden düþülsün.
        maxPopulation += building.preset.population; //15
        maxJobs += building.preset.jobs; //16
        buildings.Add(building); //17- Listeye eleman olarak bunlarý ekleyelim.
        UpdateStatText(); //19- Her bina eklendikçe alttaki metin güncellensin
    }

    public void OnRemoveBuilding(Building building)
    {//20- Remove yaptýkça aþaðýdaki iþlemler paramýzdan düþecek.
        maxPopulation -= building.preset.population; //21
        maxJobs -= building.preset.jobs; //22
        buildings.Remove(building); //23- Listeden binamýzý çýkartalým.
        Destroy(building.gameObject); //24- Gameobjemizi de kaldýralým.
        UpdateStatText(); //25
    }

    void UpdateStatText()
    {//18

        statsText.text = string.Format("Day: {0} \nMoney: {1} \nPop: {2} / {3} \nJobs: {4} / {5} \nFood: {6}", new object[7] {day, money, curPopulation, maxPopulation, curJobs, maxJobs,curFood}); 
        //48- 0,1,2 yazan yerlere 7 elemanlý obje oluþturup, içerisindekiler sýrasýyla gelecek.
    }

    public void EndTurn()
    {//26- Gün dönüþümünün tamamlanacaðý script
        day++; //50- Gün artacak
        CalculateMoney(); //28-
        CalculatePopulation(); //30-
        CalculateJobs(); //32-
        CalculateFood(); //34-
        UpdateStatText(); //35-
        if(gameStart)
            CheckStats();
        gameStart = true;
        LeftBankrupt();
    }

    void CalculateMoney()
    {//27- Parayý hesaplayalým.
        if (bankrupt > 0)
        {
            bankrupt -= payBankrupt;
        }
            
        money += curJobs * incomePerJob - payBankrupt; //36- Ýþ ile iþ gelirini çarp

        foreach(Building building in buildings)
        {//37- Buildingler içerisinden
            money -= building.preset.costPerTurn; //38- Binalarýn dönüþüm maaliyetini paradan düþürdük.
        }
    }

    void CalculatePopulation()
    {//29- Nüfusu hesaplayalým.

        if(curFood>=curPopulation && curPopulation < maxPopulation)
        {//39- Yiyeceðimiz nüfustan çok olsun ki onlarý doyurabilelim. Þehrin toplam nüfusundan da nüfusun az olmasý lazým.
            curFood -= curPopulation/4; //40-Yiyeceðimizi normal nüfusa göre 1/4 oranýnda azaltalým.
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation); 
            //41- Bunlardan hangisi azsa nüfusumuzu ona göre belirliyoruz.
        }
        else if (curFood < curPopulation)
        {//42- Yemeðimiz mevcut nüfustan az ise
            curPopulation = curFood; //43- Food ile populasyonu tutturmaya çalýþýyoruz.
        }
    }

    void CalculateJobs()
    {//31- Ýþleri hesaplayalým

        curJobs = Mathf.Min(curPopulation, maxJobs); //44- Ya maxJobstan ya da curPopulationdan git.

    }

    void CalculateFood()
    { //33- Yiyecekleri hesaplayacaðýz

        curFood = 0; //45-En baþta 0 olsun.
        foreach(Building building in buildings)
        {//46-Her bir building için
            curFood += building.preset.food; //47-Binalarýn içerisindeki yemek bilgisini topluyoruz.
        }
    }

    void LeftBankrupt()
    {
        leftBankrupt.SetText("Left: " + bankrupt + " $");

    }

    void CheckStats()
    {
        if (money<-1000 || curPopulation <= 0 || curJobs <= 0 || curFood <= 0)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
