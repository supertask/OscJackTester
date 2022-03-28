// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using OscJack;

class LEDClientTest : MonoBehaviour
{
    OscClient[] clients;
    
    //10.10.1.3, 10.10.1.4, 6111, scene
    
    private string[] oscServers = new string[]{ "10.10.1.1", "10.10.1.2", "10.10.1.3", "10.10.1.4"};
    //private string[] oscServers = new string[]{ "127.0.0.1", "127.0.0.1", "127.0.0.1", "127.0.0.1"}; //Test
    private int[] ports = new int [] {6111, 6111, 6111, 6111};
    private string[] paths = new string[] {"/scene", "/scene", "/scene", "/scene"};
    public GameObject ipAddressObj;
    public GameObject portNumberObj;
    public GameObject oscPathObj;
    public GameObject oscArgFloatObj1;

    private IEnumerator lerpSceneTypingSwitcherThread;
    private IEnumerator[] lerpSceneSwitcherThreads;
    
    private Vector2 SCENE_RANGE = new Vector2(1.0f, 0.0f);
    //private Vector2 SCENE_RANGE = new Vector2(0.0f, 1.0f);
    
    public void Start()
    {
        this.lerpSceneSwitcherThreads = new IEnumerator[oscServers.Length];
        
        //ipAddressObj.GetComponent<Text>().text = "127.0.0.1";
        //portNumberObj.GetComponent<Text>().text = "6661";
        //oscPathObj.GetComponent<Text>().text = "scene";
    }

    public void OnStartOSC0() { OnStartOSCByIndex(0); }
    public void OnStartOSC1() { OnStartOSCByIndex(1); }
    public void OnStartOSC2() { OnStartOSCByIndex(2); }
    public void OnStartOSC3() { OnStartOSCByIndex(3); }

    public void OnStartOSCByIndex(int index)
    {
        Debug.Log("Send osc client " + index);
        float fadeTime = float.Parse(oscArgFloatObj1.GetComponent<Text>().text);
        OscClient client = new OscClient(oscServers[index], ports[index]);
        this.lerpSceneSwitcherThreads[index] = LerpSceneSwitcher(client, paths[index], fadeTime);
        StartCoroutine(this.lerpSceneSwitcherThreads[index]);
    }

    
    public void OnStartOSC0And1()
    {
        OnStartOSC0();
        OnStartOSC1();
    }
    public void OnStartOSC2And3()
    {
        OnStartOSC2();
        OnStartOSC3();
    }

    public void OnStartOSCByTyping()
    {
        Debug.Log("Send osc client by typing");

        string ipAddress = ipAddressObj.GetComponent<Text>().text;
        int portNumber = int.Parse(portNumberObj.GetComponent<Text>().text);
        string oscPath = oscPathObj.GetComponent<Text>().text;
        Debug.LogFormat("ip = {0}, port = {1}, path = {2}", ipAddress, portNumber, oscPath);

        OscClient client = new OscClient(ipAddress, portNumber);
        float fadeTime = float.Parse(oscArgFloatObj1.GetComponent<Text>().text);
        this.lerpSceneTypingSwitcherThread = LerpSceneSwitcher(client, oscPath, fadeTime);
        StartCoroutine(this.lerpSceneTypingSwitcherThread);
    }
    

    private IEnumerator LerpSceneSwitcher(OscClient client, string oscPath, float duaration)
    {
        yield return null;
        float t = 0;

        while (t < duaration)
        {
            t += Time.deltaTime;
            float timeRate = t / duaration; //0 - 1
            float sceneRate = Mathf.Lerp(SCENE_RANGE.x, SCENE_RANGE.y, timeRate);

            Debug.LogFormat("Sending sceneRate={0}", sceneRate);
            client.Send(oscPath, sceneRate);

            yield return null;
        }

        client.Send(oscPath, SCENE_RANGE.y);
        client?.Dispose();
        client = null;
    }

    void OnDestroy()
    {
        //for(int index = 0; index < oscServers.Length; index++)
        //{
        //    StopCoroutine(this.lerpSceneSwitcherThreads[index]);
        //    this.lerpSceneSwitcherThreads[index] = null;
        //}

    }
}
