using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    //Obje ekleme çýkarma iþlemleri
    private bool currentlyPlacing; //1- Konumda birþey var mý yok mu kontrolü.
    private bool currentlyBulldozering; //2- Konumda silinmiþ mi kontrolü

    private BuildingPreset curBuildingPreset; //3- Bunu daha önceden oluþturmuþtuk.

    private float indicatorUpdateTime= 0.05f; //4- Her yaptýðýmýz hareketi algýlamasýn diye hassasiyetini azaltacaðýz. Ýþlemci yemesin diye.
    //Burada güncelleme süresi aldýk.

    private float lastUpdateTime; //5- Süreyi durmadan alýp iþlem yapacaðýz.
    private Vector3 curIndicatorPos; //6- Bunun da Vector3 pozisyonunu alacaðýz.

    public GameObject placementIndicator; //7- Indicator objelerimizin görünürlüðünü ayarlayacaðýz.
    public GameObject bulldozerIndicator; //8-

    private bool isActiveBuildings;
    public GameObject BuildingsButtons;

    private bool isActiveStats;
    public GameObject StatsPanel;

    private bool isActiveBankruptPanel;
    public GameObject BankruptPanel;

    public int bankrupt1000Press;
    public Button bankrupt1000Button;

    public static BuildingPlacement instance;

    private void Awake()
    {
        instance = this; //12- Bunu kim çaðýrýyorsa, doðrudan gelip eriþsin.
    }

    public void BeginNewBuildingPlacement(BuildingPreset preset)
    {//9- Yeni birþey ekleyeceðimiz için.
        /*
        //10- Elimizdeki paranýn kontrolünü yapalým. City'de instance tanýmlý olmalý!
        if (City.instance.money < preset.cost)
        {//Alacaðýmýz instancedeki money, presetteki costtan (maliyetten) azsa bütçemiz yeterli deðil demektir.
            return;
        }*/

        currentlyPlacing = true; //11- Yerleþimi aldýk.
        curBuildingPreset = preset; //12- Neyi belirlediysek burada onu alýcaz.
        placementIndicator.SetActive(true); //13- Mavi indicatorü görünür yaptýk.
    } 

    void CancelBuildingPlacement()
    {//14- Esc tuþuna basýlýrsa
        currentlyPlacing = false; //15- Ýptal ettik
        placementIndicator.SetActive(false); //16- Göstermedik.
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TryGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleBuldozer()
    {//17- Silme iþlemini gerçekleþtirecek.
        currentlyBulldozering = !currentlyBulldozering; //18-Ne seçildiyse tam tersi duruma geçir.
        bulldozerIndicator.SetActive(currentlyBulldozering); //19- Objenin görünürlüðünü de bulldozeringin durumuna göre al.

    }

    public void ActivatedBuildings()
    {
        isActiveBuildings = !isActiveBuildings;
        BuildingsButtons.SetActive(isActiveBuildings);
        
    }

    public void ActivatedStats()
    {
        isActiveStats = !isActiveStats;
        StatsPanel.SetActive(isActiveStats);
    }

    public void ActivatedBankrupt()
    {
        isActiveBankruptPanel = !isActiveBankruptPanel;
        BankruptPanel.SetActive(isActiveBankruptPanel);
    }

    public void Bankrupt1000()
    {
        if (bankrupt1000Press < 3)
        {
            City.instance.money += 1000;
            City.instance.bankrupt += 1000;
            City.instance.payBankrupt += 1000 / 100;
            bankrupt1000Press++;
            
        }
        else
        {
            bankrupt1000Button.interactable = false;
        }
    }

    public void PayBankrupt()
    {
        City.instance.money -= City.instance.bankrupt;
        City.instance.bankrupt = 0;
        City.instance.payBankrupt = 0;
    }

    private void Start()
    {
        isActiveBuildings = false;
        isActiveStats = false;
        isActiveBankruptPanel = false;
        bankrupt1000Press = 0;
    }

    private void Update()
    {//Burada durmadan kontrol gerçekleþtireceðiz.

        if (Input.GetKeyDown(KeyCode.Escape))
        {//20- ESC tuþuna basýldýysa
            CancelBuildingPlacement();
        }

        if (Time.time - lastUpdateTime > indicatorUpdateTime)
        {//21- Þuanki zaman geçen süreyi aþmýþsa Update yapabiliriz.
            lastUpdateTime = Time.time; //22- Zamaný güncelledik.

            curIndicatorPos = Selector.instance.GetCurTilePosition();
            //23- Eklenecek yerin pozisyonunu selctor instancesini (instance ekleyerek eriþebiliyorduk) virgüllü konumu iptal edip küp içerisine yerleþtiren kýsmý aldýk.

            //Ýþlemi neyde gerçekleþtireceðiz? Silme veya Ekleme

            if (currentlyPlacing)
            {//24
                placementIndicator.transform.position = curIndicatorPos;
                //25-Transformunun pozisyonunda yerleþimi ayarlýyoruz.
            }else if (currentlyBulldozering)
            {//26
                bulldozerIndicator.transform.position = curIndicatorPos;//27
            }
            //Bunlarý butonlara iþleyelim
            //---------------------------

            //Mouse sol tuþuna bastýðýmýzda ve yüklenmek için de müsaitse iþleme alacaðýz.

            if(Input.GetMouseButtonDown(0) && currentlyPlacing)
            {//31- Sol tuþuna basýlýrsa ve yükleme için de uygunsa
                PlaceBuilding(); //32
            }else if (Input.GetMouseButtonDown(0) && currentlyBulldozering)
            {
                Buldozer(); //34 Aksi takdirde buldozer çalýþacak.
            }

        }

        void PlaceBuilding()
        {//28- Burada GameObject oluþturulma iþlemi yapacaðýz.
            GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos,Quaternion.identity);
            //29-Neyi yerleþtireceðiz, nereye yerleþtireceðiz, yönü ne olacak

            //City scriptini yazdýktan sonra yap!!
            City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
            //34- Citydeki nüfus, para gibi iþlemlerin iþlenmiþ olmasý için yaptýk.
            //------------------------------------

            CancelBuildingPlacement(); //30-Yerleþtirdikten sonra yerleþtirme objesini kaybedelim.
        }

        void Buldozer() //33
        {//Silme iþlemi için
            Building buildingToDestroy = City.instance.buildings.Find(x => x.transform.position == curIndicatorPos);
            //35- Silinecek building için city'e git, instancesine eriþ, buildings listesinde x'i ara. Bu x'in transformunun pozisyonu indicator'e eþit olmalý.

            if (buildingToDestroy != null)
            {//36- Böyle birþey varsa
                City.instance.OnRemoveBuilding(buildingToDestroy);
                //37- OnRemoveBuildgteki buildingToDestroy uygula.
            }
        
        }
        
    }

}//34
