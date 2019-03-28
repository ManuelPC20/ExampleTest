using System.Collections;
using System.Collections.Generic;
using ZXing;
using ZXing.QrCode;
using UnityEngine;
using System;


public class QrCodeReader : MonoBehaviour {

    private WebCamTexture camTexture;
    private Rect screenRect;
    public bool detecto;
    public AudioSource Audio;

    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }
    }

    void OnGUI()
    {

        
        GUIStyle letrastyle = new GUIStyle();
        letrastyle.fontSize = 50; 
        // drawing the camera on screen
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
        try
           {
            
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
              camTexture.width, camTexture.height);

            if (result != null && detecto == false)
            {
                detecto = true;
                GUI.TextField(new Rect(150, 650, 500, 50), result.Text, letrastyle);
               Audio.Play();
            }
            else if
            (result!=null && detecto == true)
            {
                detecto = true;
                GUI.TextField(new Rect(150, 650, 500, 50), result.Text, letrastyle);
            }

            if(result== null)
            {
                detecto = false;
            }
            
            }
         catch (Exception ex)
        {
            detecto = false;
            Debug.LogWarning(ex.Message);         
        }

     
    }

}
