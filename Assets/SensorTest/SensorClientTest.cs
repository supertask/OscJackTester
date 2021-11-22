// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System.Collections;
using OscJack;

class SensorClientTest : MonoBehaviour
{
    OscClient _client;
    
    public int port = 6670;

    public string areaId = "X1";
    public int trackingId = 0;
    [Range(0.0f, 1.0f)] public float centerX = 0.5f;
    [Range(0.0f, 1.0f)] public float centerY = 0.5f;
    public float isStopped; 
    [Range(0.0f, 1800.0f)] public float sizeWidth = 1800f;
    [Range(0.0f, 2070.0f)] public float sizeHeight = 2070f; 
    
    IEnumerator Start()
    {
        // IP address, port number
        _client = new OscClient("127.0.0.1", port);

        // Send two-component float values ten times.
        //for (var i = 0; i < 1000000; i++) {
        while (true) {
            yield return new WaitForSeconds(0.5f); //2FPS
            _client.Send("/point",
                         areaId,
                         trackingId,
                         centerX,
                         centerY,
                         isStopped,
                         sizeWidth,
                         sizeHeight);
        }
    }

    void OnDestroy()
    {
        _client?.Dispose();
        _client = null;
    }
}
