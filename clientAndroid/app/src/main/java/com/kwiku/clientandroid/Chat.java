package com.kwiku.clientandroid;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

public class Chat extends AppCompatActivity {

    private EditText editor;
    private Button buttonSend;
    private TextView messagesDisplay;

    private String receiver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_chat);

        editor = (EditText) findViewById(R.id.message);
        buttonSend = (Button) findViewById(R.id.buttonSend);
        messagesDisplay = (TextView) findViewById(R.id.messages);

        receiver = getIntent().getStringExtra("USER_NAME");

        readMessages();

        buttonSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String message = editor.getText().toString();
                Client.send(receiver, message);
                MessagesContainer.add(receiver, new Message("Ja", message));
                displayMessage("Ja", message);
            }
        });

        Client.setOnNewMessageListener(new NewMessageListener() {
            @Override
            public void onNewMessage(final String sender, final String messageContent) {
                Chat.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        Message message = new Message(sender, messageContent);
                        MessagesContainer.add(sender, message);
                        displayMessage(sender, messageContent);
                    }
                });
            }
        });
    }

    private void displayMessage(String sender, String message) {
        String prevText = messagesDisplay.getText().toString();
        String text = prevText + sender + ": " + message + System.lineSeparator();

        messagesDisplay.setText(text);
    }

    private void readMessages() {
        if (MessagesContainer.getMessages(receiver) == null) {
            return;
        }

        for (Message message : MessagesContainer.getMessages(receiver)) {
            displayMessage(message.getSender(), message.getMessage());
        }
    }
}
