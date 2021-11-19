// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System.Collections;
using OscJack;

class WaterClientTest : MonoBehaviour
{
    OscClient _client;
    
    public int port = 10003;

    [Range(0.0f, 1.0f)] public float waterVolumeWall = 0.5f;
    [Range(0.0f, 1.0f)] public float waterVolumeFloor = 0.5f;
    
    IEnumerator Start()
    {
        // IP address, port number
        _client = new OscClient("127.0.0.1", port);

        // Send two-component float values ten times.
        //for (var i = 0; i < 1000000; i++) {
        while (true) {
            yield return new WaitForSeconds(0.5f); //2FPS
            _client.Send("/waterVolume",       // OSC address
                         waterVolumeWall,     // First element
                         waterVolumeFloor); // Second element
        }
    }

    void OnDestroy()
    {
        _client?.Dispose();
        _client = null;
    }
}
