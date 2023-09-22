using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed; //1- Kamera hareketini burada tutaca��z.
    public float minXRot; //2- A��r� d�nmeler olmas�n diye min ve max kamera a��lar� verelim.
    public float maxXRot; //3-

    private float curXRot; //4- �uanki kamera a��s� bilgisini tutuyoruz.

    public float minZoom; //5- Kameran�n min ve max zoomlama de�erlerini tutuyoruz.
    public float maxZoom; //6-

    public float zoomSpeed; //7- Zoom h�z�n� da tutuyoruz.
    public float rotateSpeed; //8- D�nme h�z�n� da ayarl�yoruz.

    private float curZoom; //9- �uanki zoon bilgisini de de�i�kende tutuyoruz.

    private Camera cam; //10- Kameraya da eri�elim.

    private void Start()
    {//Ba�lang�� de�erlerini de ayarlayal�m.
        cam = Camera.main; //11- �uanki kamera bilgisini alacak.
        curZoom = cam.transform.localPosition.y; //12- Kameradan zoom bilgisini al�yoruz.
        curXRot = -50; //13-�uanki d�nme bilgisini ba�lang�� i�in -50 ayarlad�k.
    }

    private void Update()
    {
        //Mouse zoom bilgisi ile i�lemler yapal�m.
        curZoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed; //14- Mousun tekerle�ine g�re h�zbilgisinin de �arp�m�yla zoom edecek.
        curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom); //15- Kamera zoomlama de�erlerini s�n�rlad�k. minZoom ile maxZoom aras�nda curZoom de�er alacak.

        cam.transform.localPosition = Vector3.up * curZoom; //16- Kamera transformunun lokal pozisyonunu y ekseninde curZoom ile �arpt�k.

        //Mouse sa� butonu ile d�nme i�lemi yapal�m.
        if (Input.GetMouseButton(1)) //17- 1, mouse sa� butonuna denk geliyor.
        {
            //Mouse hareketlerini alal�m.
            float x = Input.GetAxis("Mouse X"); //18-
            float y = Input.GetAxis("Mouse Y"); //19-

            curXRot += -y * rotateSpeed; //20- Xdeki d�nme hareketini ald�k.
            curXRot = Mathf.Clamp(curXRot, minXRot, maxXRot); //21-

            transform.eulerAngles = new Vector3(curXRot, transform.eulerAngles.y + (x * rotateSpeed), 0.0f);
            //22- Xdeki rotationu ayarlad�k. Yde ise kendi ysine x'deki rotatespeed de ekledik.
        }

        //Movement - Hareket Ettirme

        Vector3 forward = cam.transform.forward; //23- Kamera transformunun ileri y�n�
        forward.y = 0.0f; //24- Yukar� y�n�ne kar��mad�k.
        forward.Normalize(); //25- Y s�rekli de�i�ti�inde, ynin 0 de�erine g�re y'nin ileri y�n�n� update et dedik.
        //K�saca, Y s�f�r olacak �ekilde y�n bilgisi g�ncellenecek.

        Vector3 right = cam.transform.right; //26- Kameran�n sa� y�n�ne hareketini ald�k.

        //Klavyedeki hareketler
        float moveX = Input.GetAxisRaw("Horizontal"); //27-  -1 ve +1 de�erleri olacak.
        float moveZ = Input.GetAxisRaw("Vertical"); //28-

        //Kendi lokal bilgisini alal�m.
        Vector3 dir = forward * moveZ + right * moveX; //29- Y�n bilgisini, klavyeden ald���m�z y�n tu�lar�yla, ileri y�n�n� Z ile, sa� solu da X bilgileriyle ald�k.

        dir.Normalize(); //30- Ald���m�z bilgileri de formalize etmesini sa�l�yoruz.

        dir *= moveSpeed * Time.deltaTime;//31- Belirli bir zamanla bunu g�ncelleyelim.

        transform.position += dir; //32- Bunu da objenin transformuna uygulad�k.
    
        
    }
}//33
