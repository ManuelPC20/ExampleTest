using System.Collections;
using System.Collections.Generic;
using ZXing;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using LitJson;
using System.IO;
using Crosstales.RTVoice;

public class QrCodeReader2 : MonoBehaviour
{

    public bool mujer;
    private Rect screenRect;
    public DatosInvitados datosinvitados;
    private WebCamTexture camTexture;
    public bool activado;
    private string resultado;
    private string oldResultado;
    public string[] splited;
    public string[] splitedfinal;
    public string[] splitnombre;
    private AudioSource audiocamera;


    private Animator imagen1;

    public List<string> detectardni = new List<string>();
    public bool detected;
    public AudioSource Audio;
    public Text mostrarResultado;
    public GameObject canvasTexto;
    JsonData dataJson;




    void Start()
    {
        imagen1 = GameObject.Find("Imagen1").GetComponent<Animator>();
        audiocamera = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }
    }

    private void Update()
    {
        Reaccion();
    }

    public void Reaccion()
    {
        IBarcodeReader barcodeReader = new BarcodeReader();
        var result = barcodeReader.Decode(camTexture.GetPixels32(),
        camTexture.width, camTexture.height);
        resultado = Convert.ToString(result);

        if (result != null)
        {
            if (oldResultado == resultado)
            {
                if (activado == false)
                {
                    StartCoroutine(RepetirImagen());
                    activado = true;
                    return;
                }

            }
            else
            {

                StartCoroutine(CambiarImagen());
                Debug.Log(resultado);
                oldResultado = resultado;
                canvasTexto.SetActive(true);
                splited = resultado.Split(':', '\n', '\r');


                datosinvitados = new DatosInvitados(splited[1], splited[4], splited[7], splited[10], splited[13]);

                splitnombre = splited[1].Split(' ', '\r');


                dataJson = JsonMapper.ToJson(datosinvitados);


                File.AppendAllText(Application.dataPath + "/Resources/datosclientes.json", dataJson.ToString() + ",", Encoding.UTF8);
                StartCoroutine(SpeakerON());

            }

        }

    }


    public IEnumerator SpeakerON()
    {

        yield return new WaitForSeconds(1.0f);


        if (!audiocamera.isPlaying && mujer == false)
        {
            mostrarResultado.text = "Dr." + " " + splitnombre[1];
            Speaker.Speak("Bienvenido Doctor" + splitnombre[1], audiocamera, Speaker.VoiceForName("Microsoft Helena Desktop"), true, 0.8f, 1);
        }
        else if (!audiocamera.isPlaying && mujer == true)
        {
            mostrarResultado.text = "Dra." + " " + splitnombre[1];
            Speaker.Speak("Bienvenida Doctora" + splitnombre[1], audiocamera, Speaker.VoiceForName("Microsoft Helena Desktop"), true, 0.8f, 1);
            mujer = false;
        }


    }


    public void MujerBoton()
    {
        if (mujer == false)
        {
            mujer = true;
        }
    }

    public IEnumerator CambiarImagen()
    {
        imagen1.SetBool("Cambiar", true);
        yield return new WaitForSeconds(4.0f);
        mostrarResultado.text = "";
        imagen1.SetBool("Cambiar", false);

    }

    public IEnumerator RepetirImagen()
    {
        mostrarResultado.text = "Dr." + " " + splitnombre[1];
        if (!audiocamera.isPlaying && mujer == false)
        {
            Speaker.Speak("Bienvenido Doctor" + splitnombre[1], audiocamera, Speaker.VoiceForName("Microsoft Helena Desktop"), true, 0.8f, 1);
        }
        else if (!audiocamera.isPlaying && mujer == true)
        {
            mostrarResultado.text = "Dra." + " " + splitnombre[1];
            Speaker.Speak("Bienvenida Doctora" + splitnombre[1], audiocamera, Speaker.VoiceForName("Microsoft Helena Desktop"), true, 0.8f, 1);
            mujer = false;
        }
        imagen1.SetBool("Cambiar", true);
        yield return new WaitForSeconds(4.0f);
        imagen1.SetBool("Cambiar", false);
        mostrarResultado.text = "";
        activado = false;

    }

}

public class DatosInvitados
{
    public string NombresApellidos;
    public string Dni;
    public string ColegioMedico;
    public string Correo;
    public string Celular;

    public DatosInvitados(string NombresApellidos, string Dni, string ColegioMedico, string Correo, string Celular)
    {

        this.NombresApellidos = NombresApellidos;
        this.Dni = Dni;
        this.ColegioMedico = ColegioMedico;
        this.Correo = Correo;
        this.Celular = Celular;

    }

}