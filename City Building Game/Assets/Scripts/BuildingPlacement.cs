using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    //Obje ekleme ��karma i�lemleri
    private bool currentlyPlacing; //1- Konumda bir�ey var m� yok mu kontrol�.
    private bool currentlyBulldozering; //2- Konumda silinmi� mi kontrol�

    private BuildingPreset curBuildingPreset; //3- Bunu daha �nceden olu�turmu�tuk.

    private float indicatorUpdateTime= 0.05f; //4- Her yapt���m�z hareketi alg�lamas�n diye hassasiyetini azaltaca��z. ��lemci yemesin diye.
    //Burada g�ncelleme s�resi ald�k.

    private float lastUpdateTime; //5- S�reyi durmadan al�p i�lem yapaca��z.
    private Vector3 curIndicatorPos; //6- Bunun da Vector3 pozisyonunu alaca��z.

    public GameObject placementIndicator; //7- Indicator objelerimizin g�r�n�rl���n� ayarlayaca��z.
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
        instance = this; //12- Bunu kim �a��r�yorsa, do�rudan gelip eri�sin.
    }

    public void BeginNewBuildingPlacement(BuildingPreset preset)
    {//9- Yeni bir�ey ekleyece�imiz i�in.
        /*
        //10- Elimizdeki paran�n kontrol�n� yapal�m. City'de instance tan�ml� olmal�!
        if (City.instance.money < preset.cost)
        {//Alaca��m�z instancedeki money, presetteki costtan (maliyetten) azsa b�t�emiz yeterli de�il demektir.
            return;
        }*/

        currentlyPlacing = true; //11- Yerle�imi ald�k.
        curBuildingPreset = preset; //12- Neyi belirlediysek burada onu al�caz.
        placementIndicator.SetActive(true); //13- Mavi indicator� g�r�n�r yapt�k.
    } 

    void CancelBuildingPlacement()
    {//14- Esc tu�una bas�l�rsa
        currentlyPlacing = false; //15- �ptal ettik
        placementIndicator.SetActive(false); //16- G�stermedik.
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
    {//17- Silme i�lemini ger�ekle�tirecek.
        currentlyBulldozering = !currentlyBulldozering; //18-Ne se�ildiyse tam tersi duruma ge�ir.
        bulldozerIndicator.SetActive(currentlyBulldozering); //19- Objenin g�r�n�rl���n� de bulldozeringin durumuna g�re al.

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
    {//Burada durmadan kontrol ger�ekle�tirece�iz.

        if (Input.GetKeyDown(KeyCode.Escape))
        {//20- ESC tu�una bas�ld�ysa
            CancelBuildingPlacement();
        }

        if (Time.time - lastUpdateTime > indicatorUpdateTime)
        {//21- �uanki zaman ge�en s�reyi a�m��sa Update yapabiliriz.
            lastUpdateTime = Time.time; //22- Zaman� g�ncelledik.

            curIndicatorPos = Selector.instance.GetCurTilePosition();
            //23- Eklenecek yerin pozisyonunu selctor instancesini (instance ekleyerek eri�ebiliyorduk) virg�ll� konumu iptal edip k�p i�erisine yerle�tiren k�sm� ald�k.

            //��lemi neyde ger�ekle�tirece�iz? Silme veya Ekleme

            if (currentlyPlacing)
            {//24
                placementIndicator.transform.position = curIndicatorPos;
                //25-Transformunun pozisyonunda yerle�imi ayarl�yoruz.
            }else if (currentlyBulldozering)
            {//26
                bulldozerIndicator.transform.position = curIndicatorPos;//27
            }
            //Bunlar� butonlara i�leyelim
            //---------------------------

            //Mouse sol tu�una bast���m�zda ve y�klenmek i�in de m�saitse i�leme alaca��z.

            if(Input.GetMouseButtonDown(0) && currentlyPlacing)
            {//31- Sol tu�una bas�l�rsa ve y�kleme i�in de uygunsa
                PlaceBuilding(); //32
            }else if (Input.GetMouseButtonDown(0) && currentlyBulldozering)
            {
                Buldozer(); //34 Aksi takdirde buldozer �al��acak.
            }

        }

        void PlaceBuilding()
        {//28- Burada GameObject olu�turulma i�lemi yapaca��z.
            GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos,Quaternion.identity);
            //29-Neyi yerle�tirece�iz, nereye yerle�tirece�iz, y�n� ne olacak

            //City scriptini yazd�ktan sonra yap!!
            City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
            //34- Citydeki n�fus, para gibi i�lemlerin i�lenmi� olmas� i�in yapt�k.
            //------------------------------------

            CancelBuildingPlacement(); //30-Yerle�tirdikten sonra yerle�tirme objesini kaybedelim.
        }

        void Buldozer() //33
        {//Silme i�lemi i�in
            Building buildingToDestroy = City.instance.buildings.Find(x => x.transform.position == curIndicatorPos);
            //35- Silinecek building i�in city'e git, instancesine eri�, buildings listesinde x'i ara. Bu x'in transformunun pozisyonu indicator'e e�it olmal�.

            if (buildingToDestroy != null)
            {//36- B�yle bir�ey varsa
                City.instance.OnRemoveBuilding(buildingToDestroy);
                //37- OnRemoveBuildgteki buildingToDestroy uygula.
            }
        
        }
        
    }

}//34
