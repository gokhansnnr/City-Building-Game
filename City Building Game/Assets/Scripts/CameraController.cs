using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed; //1- Kamera hareketini burada tutacaðýz.
    public float minXRot; //2- Aþýrý dönmeler olmasýn diye min ve max kamera açýlarý verelim.
    public float maxXRot; //3-

    private float curXRot; //4- Þuanki kamera açýsý bilgisini tutuyoruz.

    public float minZoom; //5- Kameranýn min ve max zoomlama deðerlerini tutuyoruz.
    public float maxZoom; //6-

    public float zoomSpeed; //7- Zoom hýzýný da tutuyoruz.
    public float rotateSpeed; //8- Dönme hýzýný da ayarlýyoruz.

    private float curZoom; //9- Þuanki zoon bilgisini de deðiþkende tutuyoruz.

    private Camera cam; //10- Kameraya da eriþelim.

    private void Start()
    {//Baþlangýç deðerlerini de ayarlayalým.
        cam = Camera.main; //11- Þuanki kamera bilgisini alacak.
        curZoom = cam.transform.localPosition.y; //12- Kameradan zoom bilgisini alýyoruz.
        curXRot = -50; //13-Þuanki dönme bilgisini baþlangýç için -50 ayarladýk.
    }

    private void Update()
    {
        //Mouse zoom bilgisi ile iþlemler yapalým.
        curZoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed; //14- Mousun tekerleðine göre hýzbilgisinin de çarpýmýyla zoom edecek.
        curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom); //15- Kamera zoomlama deðerlerini sýnýrladýk. minZoom ile maxZoom arasýnda curZoom deðer alacak.

        cam.transform.localPosition = Vector3.up * curZoom; //16- Kamera transformunun lokal pozisyonunu y ekseninde curZoom ile çarptýk.

        //Mouse sað butonu ile dönme iþlemi yapalým.
        if (Input.GetMouseButton(1)) //17- 1, mouse sað butonuna denk geliyor.
        {
            //Mouse hareketlerini alalým.
            float x = Input.GetAxis("Mouse X"); //18-
            float y = Input.GetAxis("Mouse Y"); //19-

            curXRot += -y * rotateSpeed; //20- Xdeki dönme hareketini aldýk.
            curXRot = Mathf.Clamp(curXRot, minXRot, maxXRot); //21-

            transform.eulerAngles = new Vector3(curXRot, transform.eulerAngles.y + (x * rotateSpeed), 0.0f);
            //22- Xdeki rotationu ayarladýk. Yde ise kendi ysine x'deki rotatespeed de ekledik.
        }

        //Movement - Hareket Ettirme

        Vector3 forward = cam.transform.forward; //23- Kamera transformunun ileri yönü
        forward.y = 0.0f; //24- Yukarý yönüne karýþmadýk.
        forward.Normalize(); //25- Y sürekli deðiþtiðinde, ynin 0 deðerine göre y'nin ileri yönünü update et dedik.
        //Kýsaca, Y sýfýr olacak þekilde yön bilgisi güncellenecek.

        Vector3 right = cam.transform.right; //26- Kameranýn sað yönüne hareketini aldýk.

        //Klavyedeki hareketler
        float moveX = Input.GetAxisRaw("Horizontal"); //27-  -1 ve +1 deðerleri olacak.
        float moveZ = Input.GetAxisRaw("Vertical"); //28-

        //Kendi lokal bilgisini alalým.
        Vector3 dir = forward * moveZ + right * moveX; //29- Yön bilgisini, klavyeden aldýðýmýz yön tuþlarýyla, ileri yönünü Z ile, sað solu da X bilgileriyle aldýk.

        dir.Normalize(); //30- Aldýðýmýz bilgileri de formalize etmesini saðlýyoruz.

        dir *= moveSpeed * Time.deltaTime;//31- Belirli bir zamanla bunu güncelleyelim.

        transform.position += dir; //32- Bunu da objenin transformuna uyguladýk.
    
        
    }
}//33
