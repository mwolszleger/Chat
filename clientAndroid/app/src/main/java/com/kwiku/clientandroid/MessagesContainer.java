package com.kwiku.clientandroid;

import android.util.ArrayMap;

import java.util.ArrayList;

/**
 * Created by Kwiku on 2017-01-17.
 */

public final class MessagesContainer {
    private static ArrayMap<String, ArrayList<Message>> conversations = new ArrayMap<>();
    private static ArrayList<Message> messages = new ArrayList<Message>();

    public static ArrayList<Message> getMessages(String userName) {
        return conversations.get(userName);
    }

    public static void add(String receiver, Message message) {
        if (!conversations.containsKey(receiver)) {
            conversations.put(receiver, new ArrayList<Message>());
        }

        conversations.get(receiver).add(message);
    }
}
