using Siccity.GLTFUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingBarObj;
    private Transform loadingBar;
    private string filePath;
    private const string serverUrl = "https://ar3a.app/models/";

    private void Start()
    {

        filePath = Application.persistentDataPath;
        loadingBar = FindBar();
    }

    /*
     * Checks if the model is already in cache, and if it is not, 
     * reveals loading bar and evokes the method to request file from server
     */
    private void DownloadFile(string objname)
    {
        var fullpath = serverUrl + objname + ".glb";

        
        if (File.Exists(filePath + "/" + objname + ".glb")) //Checks if File is On Cache
        {
            LoadModel(objname);
            return;
        }
        loadingBarObj.SetActive(true);
        StartCoroutine(GetFileRequest(fullpath, objname));
        
    }

    /*
     * Downloads model file from the server, updates the loading bar progress, 
     * and loads model from diretory after download from server is completed.
     */
    private IEnumerator GetFileRequest(string url, string objname)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(filePath + "/" + objname + ".glb");
            req.SendWebRequest(); //send request to the server
            while (!req.isDone) 
            {
                Debug.Log(req.downloadProgress);
                //Updates loading bar progress
                loadingBar.localScale = new Vector3(req.downloadProgress, 1, 1); 
                yield return null; //wait every frame until request is finished
            }

            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(req.downloadHandler.text);
            }
            else //In case of sucess, loads model from diretory
            {
                Debug.Log("Sucess!");
                LoadModel(objname);
                loadingBarObj.SetActive(false); //hides the loading bar
            }
        }
    }

    /*
     * Loads model from diretory
    */
    private void LoadModel(string objname)
    {
        AnimationClip[] animClips;
        var importSettings = new ImportSettings();
        importSettings.useLegacyClips = true;
        GameObject model = Importer.LoadFromFile(filePath + "/" + objname + ".glb", importSettings, out animClips);
        InstantiateModel(model);
        PlayAnimation(model, animClips);
    }

    /*
     * Spawns the model in the 3D world
     */
    private void InstantiateModel(GameObject model)
    {
        SetSpawnPosition(model);
        model.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        model.AddComponent<BoxCollider>();
        model.AddComponent<CreateDeleteButton>();
    }

    /*
     * Sets the position of the model to be in front of the user
     */
    private void SetSpawnPosition(GameObject obj)
    {
        obj.transform.position = Camera.main.transform.position + Camera.main.transform.forward;   
    }
  
    /*
     * Plays the animation of models, if they have any
     */
    private void PlayAnimation(GameObject model, AnimationClip [] animClips)
    {
        if (animClips.Length > 0)
        {
            Animation anim = model.AddComponent<Animation>();
            animClips[0].legacy = true;
            anim.AddClip(animClips[0], animClips[0].name);
            anim.clip = anim.GetClip(animClips[0].name);
            anim.wrapMode = WrapMode.Loop;
            anim.Play();

        }
    }

    #region ButtonHandlers
    public void OnBotClick()
    {
        DownloadFile("bot");
    }
    public void OnHelmetClick()
    {
        DownloadFile("helmet");
    }
    public void OnKatanaClick()
    {
        DownloadFile("katana");
    }
    public void OnPlantClick()
    {
        DownloadFile("plant");
    }
    #endregion

    /*
     * Finds the transform to be updated in order to update the loading bar progress
     */
    private Transform FindBar()
    {
        for(int i=0; i < loadingBarObj.transform.childCount; i++)
        {
            if (loadingBarObj.transform.GetChild(i).tag.Equals("Bar"))
            {
                return loadingBarObj.transform.GetChild(i);
            }
        }
        return null;
    }
}