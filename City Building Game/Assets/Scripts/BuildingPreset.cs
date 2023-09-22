using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //2- Ýçerisindeki bilgileri editörlerle oluþturacaðýz.

//Oyun içerisinde oluþturacaðýmýz Prefablara maliyet gibi özellikler ekleyeceðiz.

[CreateAssetMenu(fileName ="Building Preset", menuName = "New Building Preset")]
//3-Building Presets klasöründe sað týklayýp Create seçince New Building Preset özelliði ekliyor olacaðýz.
//Orada House adýnda script objesi oluþturduk.
public class BuildingPreset : ScriptableObject //1- Script özelliklerde bir obje oluþturuyoruz.
{
    public int cost; //4- Oyuna maaliyet ekledik.
    public int costPerTurn; //5- Dönüþüm maaliyeti. Ör. Ev yapmak için ne kadar maaliyet gerekli gibi
    public GameObject prefab; //6-

    public int population; //7- Nüfus ne olsun.
    public int jobs; //8- Fabrika kuracaksak iþle alakalý özellikleri tutarýz.
    public int food; //9- Tarlada iþlemler yaptýðýmýzda bunu getirebiliriz.


}//10-
