package com.example.testmemoapp2_recyclerview;

import android.app.Application;
import android.widget.Toast;

public class AdapterKeeper extends Application {

    private static RecyclerViewAdapter adapter;

    @Override
    public void onCreate(){
        super.onCreate();

    }

    public static void SetAdapter(RecyclerViewAdapter createdAdapter){
        adapter = createdAdapter;
    }

    public static RecyclerViewAdapter GetAdapter(){
        return adapter;
    }
}
