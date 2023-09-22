using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //1- Eklenen þeyleri duyurmak için. Yapýlan þeyin bütün kodun her yerinde eriþilmesi için.


public class Selector : MonoBehaviour
{
    //Mouse hareketlerine göre ekrana objeler ekleyeceðiz.
    //Mousede sola týklanmasýyla da birþeylerin kurulumunu gerçekleþtireceðiz.

    private Camera cam; //2- Kamerayý aldýk.
    public static Selector instance; //4- Heryerden bu dosyaya eriþilebilmesi için bunu yaptýk.

    private void Awake()
    {
        instance = this; //5- Bu sayede, selectorun bu fonksiyonunu git al gibi iþlemler yapabileceðiz.
    }
    private void Start()
    {
        cam = Camera.main; //3- Ana kameraya eriþtik.
    }

    //UI'ya deðil de zemine eriþsin diye fonk yazalým.
    public Vector3 GetCurTilePosition() //6-
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {//7- Þuanda UI'ya deðdiyse, gameObject üzerinde bir yere temas ettiyse
            return new Vector3(0, -99, 0); //8- Alakasýz kordinat verdik. Yani Canvas UI ile iþlem yapmasýný istemedik.
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero); //10- Hayali zemin oluþturduk.
        //Planemizde düz bir zemin oluþturduk. Ölçüsü 1x1'lik 2 boyutlu zemin oluþtu.

        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //11- Farenin dokunduðu yere ýþýn yolladýk.

        float rayOut = 0.0f; //12- Zemine dokununca o zemini tespit etmesi.

        if(plane.Raycast(ray,out rayOut))
        {//13- Zemine dokunmuþ isek
            Vector3 newPos = ray.GetPoint(rayOut)- new Vector3(0.05f,0.0f, 0.05f); //14- Yeþil Groundda dokunduðumuz yerin pozisyonunu aldýk. 0.05f kadar da kaydýrma yaptýk. Objenin zeminde doðru yere oturmasý için.(Offsetteki ayarlar gibi)

            //17- Týkladýðýmýz yer küsüratlý kordinatlardaysa tam sayýya tamamlayalým. Ör 0.98 ise 1'e tamamlayacaðýz.
            newPos= new Vector3(Mathf.CeilToInt(newPos.x),0.0f, Mathf.CeilToInt(newPos.z)); //CeilToInt üste tamamlar.
            //Týkladýðýmýz yerde UI olmasýn, karenin içerisinde bi yer olmasý için kodumuzu yazdýk.


            return newPos; //15- newPos'a git dedik.    
        }

        //return Vector3.back; //9- Fonktaki kýrmýzý çizilme hatasýndan kurtulmak için yaptýk.

        return new Vector3(0, -99, 0); //16- Alakasýz bi yere gitsin diye yazdýk. Böyle bir ihtimali ele almayacaðýz.
    }
     
}   
