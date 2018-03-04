using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Networking;


/**
 * Simple echo server implementation using our Networking library.
 * This class is used as a Unity script which can be attached to a prefab for 
 * testing.
 */
public unsafe class unityEchoServer : MonoBehaviour
{

    private static int MAX_BUFFER_SIZE = 1200;
    public static int SOCKET_NODATA = 0;
    public static int SOCKET_DATA_WAITING = 1;

    private Server server;
    private ushort portNo = 9999;
    private bool running;
    private EndPoint ep;

    /*
     * I think this is called first? UNITY. 
     * 
     * Creates and initializes the Server object.
     * 
     */
    void Start()
    {
        Debug.Log("Starting Server-Echo test");

        // Creates a blank EndPoint which will be filled by the Server.Recv call.
        ep = new EndPoint();
        Thread recvThread;
        Int32 result;

        // Create and initialize a server.
        server = new Server();
        result = server.Init(portNo);
        if (result != 0)
        {
            Debug.Log("Failed to initialize socket");
        }

        recvThread = new Thread(recvThrdFunc);
        running = true;
        recvThread.Start();

        //System.Threading.Thread.Sleep(5000);
        //running = false;

    } // End of Start()

    /*
     * Thread function to read incoming packets.
     * 
     * Currently works when tested using lab computers.
     * Initially, the server would recv only the first packet. 
     * 
     * The listen loop was modified to sleep for 10ms if data is not waiting
     * at the socket. Still need to stress test to see if the server can handle
     * at least 1920 incoming packets/s.
     * 
     * Sleep time works down to 1ms with larger packets (>600B)
     *
     */
    private void recvThrdFunc()
    {
        Int32 result;
        Int32 totalRecv;

        byte[] recvBuffer = new byte[MAX_BUFFER_SIZE];
        Int32 numRecv;

        while (running)
        {
            result = server.Poll();
            // If there is data waiting to be read
            if (result == SOCKET_DATA_WAITING)
            {
                fixed (EndPoint* ep_ptr = &ep)
                {
                    numRecv = server.Recv(ep_ptr, recvBuffer, MAX_BUFFER_SIZE);
                    if (numRecv <= 0)
                    {
                        Debug.Log("Received Nothing.");
                    }
                    else
                    {
                        string contents = System.Text.Encoding.UTF8.GetString(recvBuffer);

                        Debug.Log("Received: " + numRecv);
                        Debug.Log("Contents: " + contents);
                        Debug.Log("From EP: " + ep.addr.Byte3 + '.' + ep.addr.Byte2 + '.' + ep.addr.Byte1 + '.' + ep.addr.Byte0);

                        // Echo received data back
                        server.Send(ep, recvBuffer, MAX_BUFFER_SIZE);
                    }
                }
            }
            // If there's no data, sleep.
            else
            {
                // Sleeeeep.
                System.Threading.Thread.Sleep(10);
            }

        }
        Debug.Log("Exited loop");
    } // End of recvThrdFunc()

    // Update is called once per frame
    void Update()
    {

    }
}