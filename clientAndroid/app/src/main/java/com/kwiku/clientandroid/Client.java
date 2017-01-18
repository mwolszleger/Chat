package com.kwiku.clientandroid;

import android.util.Log;

import java.io.BufferedWriter;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;

/**
 * Created by Kwiku on 2017-01-17.
 */

public class Client {

    private static final int portnumber = 1024;
    private static final String debugString = "meh";

    private static final Socket socket = new Socket();
    private static UserLoggedInListener userLoggedInListener;
    private static NewMessageListener newMessageListener;

    private static boolean isRecievingTaskRunning = false;

    private static String login;

    public static void ConnectionToSerwer(String ip, String login, String password) {
        try {
            InetAddress addr = InetAddress.getByName(ip);
            SocketAddress remoteAddr = new InetSocketAddress(addr, portnumber);
            Client.login = login;

            socket.connect(remoteAddr);

            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));

            bw.write("12login:" + login + ":a");
            bw.flush();
        } catch (IOException e) {
            Log.e("exceptiom", e.getMessage());

        }
    }

    public static void reciving() {
        try {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            byte[] content = new byte[1024];
            int bytesRead = -1;
            while ((bytesRead = socket.getInputStream().read(content)) != -1) {
                baos.write(content, 0, bytesRead);
                Log.e("odebrano", baos.toString());
                final String message = baos.toString();
                baos.reset();

                System.out.println(message);

                handleNewUser(message);
                handleNewMessage(message);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static void send(String receiver, String msg) {
        try {
            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));
            String header = "sendMsg";
            String output = header + ":" + login + ":" + receiver + ":" + msg;
            bw.write(output.length() + output);
            bw.flush();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static void setOnUserLoggedInListener(UserLoggedInListener listener) {
        userLoggedInListener = listener;
        setRecievingTaskRunning();
    }

    public static void setOnNewMessageListener(NewMessageListener listener) {
        newMessageListener = listener;
    }

    private static void setRecievingTaskRunning() {
        isRecievingTaskRunning = true;

        new Thread(new Runnable() {
            public void run() {
                reciving();
            }
        }).start();
    }

    private static void handleNewUser(String command) {
        if (userLoggedInListener == null) {
            return;
        }

        if (command.contains("logged")) {
            String userName = command.substring(command.lastIndexOf(":") + 1);
            userLoggedInListener.onUserLoggedIn(userName);
        }
    }

    private static void handleNewMessage(String command) {
        if (newMessageListener == null) {
            return;
        }

        if (command.contains("sendMsg")) {
            String[] x = command.split(":");
            String sender = x[1];
            String message = x[x.length - 1];
            newMessageListener.onNewMessage(sender, message);
        }
    }

    public static void connectionClose(){
        try {
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}