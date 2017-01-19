package com.kwiku.clientandroid;

import android.util.ArrayMap;

import java.util.ArrayList;

public class CommandDeserializer {
    public ArrayList<String[]> deserialize(String stream) {
        ArrayList<String[]> commands = new ArrayList<>();

        while (stream.length() > 0) {
            int length = getLength(stream);
            int digitLength = String.valueOf(length).length();
            int start = digitLength;

            String temp = stream.substring(start, start + length);
            String[] command = temp.split(":");

            stream = stream.substring(start + length, stream.length());

            commands.add(command);
        }

        return commands;
    }

    private int getLength(String command) {
        int length = 0;

        for(int i = 0; i < command.length(); i++) {
            String x = String.valueOf(command.charAt(i));

            try {
                int digit = Integer.parseInt(x);
                length = length * 10 + digit;
            }
            catch (Exception e) {
                break;
            }
        }

        return length;
    }
}
