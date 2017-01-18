package com.example.kwiku.chatofficial;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import java.io.BufferedWriter;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;
import java.io.OutputStreamWriter;


public class Logowanie extends AppCompatActivity {

    private static final String hostname = "192.168.1.101";
    private static final int portnumber = 1024;

    private static final String debugString = "meh";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_logowanie);

    }

    public void send_login(View view) {

        new Thread(new Runnable() {
            public void run() {
                tryLogIn(hostname);
            }
        }).start();
    }
    public void reciving(Socket socket)
    {
        try {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            byte[] content = new byte[1024];
            int bytesRead = -1;
            while ((bytesRead = socket.getInputStream().read(content)) != -1) {
                baos.write(content, 0, bytesRead);
                Log.e("odebrano", baos.toString());
                final String message=baos.toString();
                runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(getApplicationContext(), message, Toast.LENGTH_SHORT).show();
                    }
                });
                baos.reset();

            }
        }
        catch(IOException e)
        {


        }
    }




    public void tryLogIn(String ip)
    {
        final Socket socket = new Socket();
        try {
            ConnectivityManager connectivityManager
                    = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
            NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
            if (activeNetworkInfo != null && activeNetworkInfo.isConnected())
                Log.i(debugString, "som internety");
            else
                Log.i(debugString, "nie ma internetow");


            InetAddress addr = InetAddress.getByName(ip);
            SocketAddress remoteAddr = new InetSocketAddress(addr, portnumber);

            Log.i(debugString, "proba laczenia");
            socket.connect(remoteAddr);
            Log.i(debugString, "polaczono" + ip);


            new Thread(new Runnable() {
                public void run() {
                    reciving(socket);
                }
            }).start();
            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));
            bw.write("12login:test:d");
            //bw.newLine();
            bw.flush();
            String message;
        }

        catch (IOException e)
        {
            Log.e("exceptiom",e.getMessage());

        }




    }

}




