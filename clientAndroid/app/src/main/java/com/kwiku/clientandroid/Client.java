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
import java.util.ArrayList;
import android.util.Base64;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class Client {

    private static final int portnumber = 1024;
    private static final String debugString = "meh";

    private static final Socket socket = new Socket();
    private static CommandDeserializer commandDeserializer = new CommandDeserializer();
    private static UserLoggedInListener userLoggedInListener;
    private static LogOutListener logOutListener;
    private static ArrayList<NewMessageListener> newMessageListener = new ArrayList<>();

    private static boolean isRecievingTaskRunning = false;

    private static String login;

    public static void ConnectionToSerwer(String ip, String login, String password) {
        try {
            InetAddress addr = InetAddress.getByName(ip);
            SocketAddress remoteAddr = new InetSocketAddress(addr, portnumber);
            Client.login = login;

            socket.connect(remoteAddr);

            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));

            bw.write(createLoginCommand(login, password));
            bw.flush();
        } catch (IOException e) {
            e.printStackTrace();
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

                for (String[] command : commandDeserializer.deserialize(message)) {
                    performCommand(command);
                }
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

    public static int setOnNewMessageListener(NewMessageListener listener) {
        newMessageListener.add(listener);

        return newMessageListener.size() - 1;
    }

    public static void removeOnNewMessageListener(int i) {
        newMessageListener.remove(i);
    }

    public static void setOnLogOutListener(LogOutListener listener) {
        logOutListener = listener;
    }

    public static void setRecievingTaskRunning() {
        isRecievingTaskRunning = true;

        new Thread(new Runnable() {
            public void run() {
                reciving();
            }
        }).start();
    }

    private static void handleNewUser(String userName) {
        if (userLoggedInListener == null) {
            return;
        }

        userLoggedInListener.onUserLoggedIn(userName);
    }

    private static void handleLogOut(String userName) {
        logOutListener.onLogOut(userName);
    }

    private static void handleNewMessage(String sender, String message) {
        for (NewMessageListener listener : newMessageListener) {
            listener.onNewMessage(sender, message);
        }
    }

    private static void performCommand(String[] command) {
        String commandName = command[0];

        switch (commandName) {
            case "logged" :
                if (command.length <= 1) {
                    return;
                }

                handleNewUser(command[1]);
                break;

            case "sendMsg":
                handleNewMessage(command[1], command[3]);
                break;

            case "loggedOut":
                handleLogOut(command[1]);
                break;
        }
    }
    private static String computeHash(String input) throws NoSuchAlgorithmException, UnsupportedEncodingException {
        MessageDigest digest = MessageDigest.getInstance("SHA-256");
        digest.reset();

        byte[] byteData = digest.digest(input.getBytes("UTF-8"));
        String encoded = Base64.encodeToString(byteData,Base64.DEFAULT);
        encoded=encoded.substring(0,encoded.length()-1);
        return encoded;
    }
    private static String createLoginCommand(String login, String password) {
        Log.e(debugString,password);
        String hash="";
        try {
            hash=computeHash(password);
        }
        catch(NoSuchAlgorithmException e)
        {

            Log.e(debugString,e.getMessage());

        }
        catch(UnsupportedEncodingException ee)
        {
            Log.e(debugString,ee.getMessage());

        }


        String command = "login:" + login + ":" + hash;
        Log.e(debugString,command.length() + command);
        return command.length() + command;
    }

    public static void connectionClose(){
        try {
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}