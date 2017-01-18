package com.kwiku.clientandroid;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

public class Options extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_options);
    }

    public void onSave(View view) {

        EditText textIP = (EditText) findViewById(R.id.editText_ip);
        String IP_SERWERA = textIP.getText().toString();

        EditText textPORT = (EditText) findViewById(R.id.editText_port);
        String PORT_SERWERA = textPORT.getText().toString();

        Intent intent_logowanie = new Intent(this, Login.class);
        intent_logowanie.putExtra("hasloIP", IP_SERWERA);
        intent_logowanie.putExtra("hasloPORT", PORT_SERWERA);

        startActivity(intent_logowanie);
    }
}
