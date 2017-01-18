package com.kwiku.clientandroid;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import java.util.ArrayList;

public class List extends AppCompatActivity {

    private java.util.List users = new ArrayList();
    private ListView listView;

    ArrayAdapter<String> adapter;
    ArrayList<String> itemList;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_list);

//        Klient.send("pawel", "Witam");

        adapter = new ArrayAdapter<String>(List.this, android.R.layout.simple_list_item_1, users);
        ListView listV=(ListView)findViewById(R.id.usersList);
        listV.setAdapter(adapter);

        listV.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
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


        //System.out.println(users);
    }

    @Override
    public void onDestroy() {

        Client.connectionClose();
        super.onDestroy();
    }
}
