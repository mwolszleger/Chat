package com.kwiku.clientandroid;

import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

public class Login extends AppCompatActivity {

    String IP_SERWERA = "";
    int PORT_SERWERA = 1024;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        IP_SERWERA = getIntent().getStringExtra("hasloIP");
        //PORT_SERWERA = getIntent().getStringExtra("hasloPORT");
        //int PORT_SERWERA_INT = Integer.parseInt(PORT_SERWERA);

        TextView tv_IP = (TextView)findViewById(R.id.textView_ipSerwera_2);
        tv_IP.setText(IP_SERWERA);

        //  System.out.println(IP_SERWERA);
        //  System.out.println(PORT_SERWERA);
    }

    //  System.out.println(PORT_SERWERA);


    public void onOpcje(View view) {
        Intent intent_opcje = new Intent(this, Options.class);
        startActivity(intent_opcje);
    }

    public void onZaloguj(View view) {

        EditText etLogin = (EditText) findViewById(R.id.editText_login);
        final String login = etLogin.getText().toString();

        EditText etHaslo = (EditText) findViewById(R.id.editText_haslo);
        final String haslo = etLogin.getText().toString();

        if(IP_SERWERA == null)
        {
            Context context = getApplicationContext();
            CharSequence wiadomosc_blad = "Nie podano adresu ip";
            int czas = Toast.LENGTH_SHORT;

            Toast toast = Toast.makeText(context, wiadomosc_blad , czas);
            toast.show();
        }

        else {
            Thread thread = new Thread(new Runnable() {
                public void run() {
                    Client.ConnectionToSerwer(IP_SERWERA, login, haslo);
                }
            });

            thread.start();

            try {
                thread.join();
            }
            catch (Exception e) {
                e.printStackTrace();
            }

            Intent intent_lista = new Intent(this, List.class);
            startActivity(intent_lista);
        }
    }
}
