using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Networking;

public unsafe class unityBroadcastTest {

    private static int MAX_BUFFER_SIZE = 1200;
    public static int SOCKET_NODATA = 0;
    public static int SOCKET_DATA_WAITING = 1;

    private Server server;
    private ushort portNo = 9999;
    private bool running;
    private EndPoint ep;

    private Int32 numQueueOps = 0;

    void Start()
    {
        ep = new EndPoint();
        Thread recvThread;
        Int32 result;

        Debug.Log("Starting Broadcast test");
        server = new Server();
        result = server.Init(portNo);
        if (result != 0)
        {
            Debug.Log("Failed to initialize socket");
        }
        // We need a queue to test this. woooo.
        // Int32 to log number of queue operations per update

        // Initialize TCP Socket
        // Listen() for connection requests
        // Listen socket & accept request
        // Accept() while endPoint pool < 30

        // Once endPoint pool == 30
        // Broadcast a test buffer of size n*n (something large)


        // Close TCP socket
        // Open UDP socket
        // Enter listen loop (recvThrdFunc)

    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        // Dequeue or something? I don't know. 
        // Broadcast something at the end of the update. 
        // Server.Broadcast(); or something like that.
        
    }

    private void recvThrdFunc()
    {
        // Select() socket
        // Do things with buffer
        // As in: add contents of buffer to queue
            // At some point, there will need to be a dequeue


    }
}