using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //1- Eklenen �eyleri duyurmak i�in. Yap�lan �eyin b�t�n kodun her yerinde eri�ilmesi i�in.


public class Selector : MonoBehaviour
{
    //Mouse hareketlerine g�re ekrana objeler ekleyece�iz.
    //Mousede sola t�klanmas�yla da bir�eylerin kurulumunu ger�ekle�tirece�iz.

    private Camera cam; //2- Kameray� ald�k.
    public static Selector instance; //4- Heryerden bu dosyaya eri�ilebilmesi i�in bunu yapt�k.

    private void Awake()
    {
        instance = this; //5- Bu sayede, selectorun bu fonksiyonunu git al gibi i�lemler yapabilece�iz.
    }
    private void Start()
    {
        cam = Camera.main; //3- Ana kameraya eri�tik.
    }

    //UI'ya de�il de zemine eri�sin diye fonk yazal�m.
    public Vector3 GetCurTilePosition() //6-
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {//7- �uanda UI'ya de�diyse, gameObject �zerinde bir yere temas ettiyse
            return new Vector3(0, -99, 0); //8- Alakas�z kordinat verdik. Yani Canvas UI ile i�lem yapmas�n� istemedik.
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero); //10- Hayali zemin olu�turduk.
        //Planemizde d�z bir zemin olu�turduk. �l��s� 1x1'lik 2 boyutlu zemin olu�tu.

        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //11- Farenin dokundu�u yere ���n yollad�k.

        float rayOut = 0.0f; //12- Zemine dokununca o zemini tespit etmesi.

        if(plane.Raycast(ray,out rayOut))
        {//13- Zemine dokunmu� isek
            Vector3 newPos = ray.GetPoint(rayOut)- new Vector3(0.05f,0.0f, 0.05f); //14- Ye�il Groundda dokundu�umuz yerin pozisyonunu ald�k. 0.05f kadar da kayd�rma yapt�k. Objenin zeminde do�ru yere oturmas� i�in.(Offsetteki ayarlar gibi)

            //17- T�klad���m�z yer k�s�ratl� kordinatlardaysa tam say�ya tamamlayal�m. �r 0.98 ise 1'e tamamlayaca��z.
            newPos= new Vector3(Mathf.CeilToInt(newPos.x),0.0f, Mathf.CeilToInt(newPos.z)); //CeilToInt �ste tamamlar.
            //T�klad���m�z yerde UI olmas�n, karenin i�erisinde bi yer olmas� i�in kodumuzu yazd�k.


            return newPos; //15- newPos'a git dedik.    
        }

        //return Vector3.back; //9- Fonktaki k�rm�z� �izilme hatas�ndan kurtulmak i�in yapt�k.

        return new Vector3(0, -99, 0); //16- Alakas�z bi yere gitsin diye yazd�k. B�yle bir ihtimali ele almayaca��z.
    }
     
}   
