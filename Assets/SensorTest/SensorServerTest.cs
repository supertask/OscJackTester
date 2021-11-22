// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using OscJack;

class SensorServerTest : MonoBehaviour
{
    OscServer _server;
    public int port = 6670;

    void Start()
    {
        _server = new OscServer(port); // Port number

        _server.MessageDispatcher.AddCallback(
            "/point", // OSC address
            (string address, OscDataHandle data) => {
                Debug.Log(
                    string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                        data.GetElementAsString(0),
                        data.GetElementAsInt(1),
                        data.GetElementAsFloat(2),
                        data.GetElementAsFloat(3),
                        data.GetElementAsFloat(4),
                        data.GetElementAsFloat(5),
                        data.GetElementAsFloat(6)
                    )
                );
            }
        );
    }

    void OnDestroy()
    {
        _server?.Dispose();
        _server = null;
    }
}
