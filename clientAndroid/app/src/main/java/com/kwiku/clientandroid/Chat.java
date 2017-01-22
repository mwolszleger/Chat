package com.kwiku.clientandroid;

import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.Html;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

public class Chat extends AppCompatActivity {

    private EditText editor;
    private Button buttonSend;
    private TextView messagesDisplay;
    private int NewMessageListenerId;
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
                editor.setText("");
            }
        });

        NewMessageListenerId = Client.setOnNewMessageListener(new NewMessageListener() {
            @Override
            public void onNewMessage(final String sender, final String messageContent) {
                Chat.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        Message message = new Message(sender, messageContent);
                        MessagesContainer.add(sender, message);
                        displayMessage(sender, messageContent);

//                        showNotification(sender, messageContent);
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

    @Override
    public void onPause() {
        super.onPause();
        Client.removeOnNewMessageListener(NewMessageListenerId);
        finish();
    }

    //    public void showNotification(String userName, String message) {
//
//        NotificationCompat.Builder builder = new NotificationCompat.Builder(getApplicationContext());
//
//        builder.setSmallIcon(R.mipmap.ic_launcher);
//        builder.setContentTitle(userName + " napisał do Ciebie");
//        builder.setContentText(message);
//
//        Intent resultIntent = new Intent(this, Chat.class);
//        resultIntent.putExtra("USER_NAME", userName);
//        TaskStackBuilder stackBuilder = TaskStackBuilder.create(this);
//        stackBuilder.addParentStack(Chat.class);
//
//        // Adds the Intent that starts the Activity to the top of the stack
//        stackBuilder.addNextIntent(resultIntent);
//        PendingIntent resultPendingIntent = stackBuilder.getPendingIntent(0,PendingIntent.FLAG_UPDATE_CURRENT);
//        builder.setContentIntent(resultPendingIntent);
//
//        NotificationManager mNotificationManager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
//
//        // notificationID allows you to update the notification later on.
//        mNotificationManager.notify(0, builder.build());
//    }
}
