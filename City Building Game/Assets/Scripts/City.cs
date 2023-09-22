using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    //Oyunun parasal i�lemlerini yapaca��z.

    public float money; //1- maaliyetini hesaplayaca��z.
    public int day; //2- ��lem yapt�k�a g�n g�n ilerleyecek.
    public int curPopulation; //3- �uanki n�fus ne kadar
    public int curJobs; //4- Fabrikalar kurulduk�a i�lere bak�caz.
    public int curFood; //5- Besin olarak �iftlik yapt�k�a �iftlik durumunu g�zlemleyece�iz.
    public int maxPopulation; //6- Max n�fusu tutaca��z.
    public int maxJobs; //7- Ne kadar i� olabilir.
    public int incomePerJob; //8- �� ba��na ne kadar gelir elde ediyoruz.
    public TextMeshProUGUI statsText; //9- �statistikleri tuttu�umuz textimiz.
    public List<Building> buildings = new List<Building>(); //10-Ekleyece�imiz yap�lar� listeledik.

    public static City instance; //11- �ehri heryerden �a��rabilmek i�in

    public GameObject gameOverPanel;
    private bool gameStart;

    public float bankrupt;
    public float payBankrupt;
    public TextMeshProUGUI leftBankrupt;

    private void Awake()
    {
        instance = this; //12- Bunu kim �a��r�yorsa, do�rudan gelip eri�sin.
    }

    private void Start()
    {
        UpdateStatText(); //49
        gameStart = false;
    }

    public void OnPlaceBuilding(Building building)
    {//13- Her bina yapt�k�a cost diye bir maaliyet eklemi�tik.
        money -= building.preset.cost; //14-Her yapt���m�z �eyin maaliyetinden d���ls�n.
        maxPopulation += building.preset.population; //15
        maxJobs += building.preset.jobs; //16
        buildings.Add(building); //17- Listeye eleman olarak bunlar� ekleyelim.
        UpdateStatText(); //19- Her bina eklendik�e alttaki metin g�ncellensin
    }

    public void OnRemoveBuilding(Building building)
    {//20- Remove yapt�k�a a�a��daki i�lemler param�zdan d��ecek.
        maxPopulation -= building.preset.population; //21
        maxJobs -= building.preset.jobs; //22
        buildings.Remove(building); //23- Listeden binam�z� ��kartal�m.
        Destroy(building.gameObject); //24- Gameobjemizi de kald�ral�m.
        UpdateStatText(); //25
    }

    void UpdateStatText()
    {//18

        statsText.text = string.Format("Day: {0} \nMoney: {1} \nPop: {2} / {3} \nJobs: {4} / {5} \nFood: {6}", new object[7] {day, money, curPopulation, maxPopulation, curJobs, maxJobs,curFood}); 
        //48- 0,1,2 yazan yerlere 7 elemanl� obje olu�turup, i�erisindekiler s�ras�yla gelecek.
    }

    public void EndTurn()
    {//26- G�n d�n���m�n�n tamamlanaca�� script
        day++; //50- G�n artacak
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
    {//27- Paray� hesaplayal�m.
        if (bankrupt > 0)
        {
            bankrupt -= payBankrupt;
        }
            
        money += curJobs * incomePerJob - payBankrupt; //36- �� ile i� gelirini �arp

        foreach(Building building in buildings)
        {//37- Buildingler i�erisinden
            money -= building.preset.costPerTurn; //38- Binalar�n d�n���m maaliyetini paradan d���rd�k.
        }
    }

    void CalculatePopulation()
    {//29- N�fusu hesaplayal�m.

        if(curFood>=curPopulation && curPopulation < maxPopulation)
        {//39- Yiyece�imiz n�fustan �ok olsun ki onlar� doyurabilelim. �ehrin toplam n�fusundan da n�fusun az olmas� laz�m.
            curFood -= curPopulation/4; //40-Yiyece�imizi normal n�fusa g�re 1/4 oran�nda azaltal�m.
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation); 
            //41- Bunlardan hangisi azsa n�fusumuzu ona g�re belirliyoruz.
        }
        else if (curFood < curPopulation)
        {//42- Yeme�imiz mevcut n�fustan az ise
            curPopulation = curFood; //43- Food ile populasyonu tutturmaya �al���yoruz.
        }
    }

    void CalculateJobs()
    {//31- ��leri hesaplayal�m

        curJobs = Mathf.Min(curPopulation, maxJobs); //44- Ya maxJobstan ya da curPopulationdan git.

    }

    void CalculateFood()
    { //33- Yiyecekleri hesaplayaca��z

        curFood = 0; //45-En ba�ta 0 olsun.
        foreach(Building building in buildings)
        {//46-Her bir building i�in
            curFood += building.preset.food; //47-Binalar�n i�erisindeki yemek bilgisini topluyoruz.
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
