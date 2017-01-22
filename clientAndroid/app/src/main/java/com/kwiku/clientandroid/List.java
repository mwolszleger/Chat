package com.kwiku.clientandroid;

import android.content.Intent;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.provider.CalendarContract;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import java.util.ArrayList;

public class List extends AppCompatActivity {

    private java.util.List users = new ArrayList();
    private ListView listView;

    ArrayAdapter<String> adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_list);

        adapter = new ArrayAdapter<String>(List.this, android.R.layout.simple_list_item_1, users);
        listView = (ListView)findViewById(R.id.usersList);
        listView.setAdapter(adapter);

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                view.setBackgroundColor(Color.TRANSPARENT);

                Intent conversation = new Intent(List.this, Chat.class);
                conversation.putExtra("USER_NAME", (String) adapterView.getItemAtPosition(i));
                startActivity(conversation);
            }
        });

        Client.setOnUserLoggedInListener(new UserLoggedInListener() {
            @Override
            public void onUserLoggedIn(final String userName) {
                List.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        users.add(userName);
                        adapter.notifyDataSetChanged();
                    }
                });
            }
        });

        Client.setOnLogOutListener(new LogOutListener() {
            @Override
            public void onLogOut(final String userName) {
                List.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        users.remove(userName);
                        adapter.notifyDataSetChanged();
                    }
                });
            }
        });

        Client.setOnNewMessageListener(new NewMessageListener() {
            @Override
            public void onNewMessage(final String sender, final String message) {
                List.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        showNotification(sender, message);
                        MessagesContainer.add(sender, new Message(sender, message));
                    }
                });
            }
        });

        Client.setRecievingTaskRunning();
    }

    public void showNotification(String userName, String message) {
        int pos = adapter.getPosition(userName);
        listView.getChildAt(pos).setBackgroundColor(Color.GRAY);

        //Toast.makeText(getApplicationContext(), userName + " napisał wiadomość", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onDestroy() {
        Client.connectionClose();
        super.onDestroy();
    }
}
