using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //2- ��erisindeki bilgileri edit�rlerle olu�turaca��z.

//Oyun i�erisinde olu�turaca��m�z Prefablara maliyet gibi �zellikler ekleyece�iz.

[CreateAssetMenu(fileName ="Building Preset", menuName = "New Building Preset")]
//3-Building Presets klas�r�nde sa� t�klay�p Create se�ince New Building Preset �zelli�i ekliyor olaca��z.
//Orada House ad�nda script objesi olu�turduk.
public class BuildingPreset : ScriptableObject //1- Script �zelliklerde bir obje olu�turuyoruz.
{
    public int cost; //4- Oyuna maaliyet ekledik.
    public int costPerTurn; //5- D�n���m maaliyeti. �r. Ev yapmak i�in ne kadar maaliyet gerekli gibi
    public GameObject prefab; //6-

    public int population; //7- N�fus ne olsun.
    public int jobs; //8- Fabrika kuracaksak i�le alakal� �zellikleri tutar�z.
    public int food; //9- Tarlada i�lemler yapt���m�zda bunu getirebiliriz.


}//10-
