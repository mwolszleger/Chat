package com.kwiku.clientandroid;

/**
 * Created by Kwiku on 2017-01-17.
 */

public class Message {
    private String sender;
    private String message;

    public Message(String sender, String message) {
        this.sender = sender;
        this.message = message;
    }

    public String getSender() {
        return sender;
    }

    public String getMessage() {
        return message;
    }
}
