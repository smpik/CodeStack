package com.example.testmemoapp2_recyclerview;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.ItemTouchHelper;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.ContentValues;
import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Locale;

public class MainActivity extends AppCompatActivity {

    private static ArrayList<Integer> idList = new ArrayList<>();
    private static ArrayList<String> titleList = new ArrayList<>();
    private static ArrayList<String> detailList = new ArrayList<>();
    private DBOpenHelper helper;
    private static SQLiteDatabase db;
    private static Context context;
    private static RecyclerViewAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //========================================================================================//
        // 初期化処理                                                                             //
        //========================================================================================//
        clearList();//各Listを初期化する(これしないとバックキーで戻ってきたときにaddされまくっちゃう)
        readData();//DBを読み込んでtitleList、detailListに格納する
        context = MainActivity.this;
        //========================================================================================//
        // RecyclerViewの実装                                                                     //
        //========================================================================================//
        final RecyclerView recyclerView = findViewById(R.id.my_recycler_view);

        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView
        recyclerView.setHasFixedSize(true);

        // use a linear layout manager
        RecyclerView.LayoutManager rLayoutManager = new LinearLayoutManager(this);

        recyclerView.setLayoutManager(rLayoutManager);

        // specify an adapter (see also next example)
        final RecyclerView.Adapter rAdapter = new RecyclerViewAdapter(MainActivity.this,idList,titleList);//RecycleView作成に必要なものしか渡さない
        recyclerView.setAdapter(rAdapter);

        adapter = (RecyclerViewAdapter)recyclerView.getAdapter();
        AdapterKeeper.SetAdapter(adapter);

        //==========================================================//
        // タップ処理の実装                                          //
        //==========================================================//
        ItemTouchHelper.SimpleCallback callback = new ItemTouchHelper.SimpleCallback(0,ItemTouchHelper.LEFT | ItemTouchHelper.RIGHT) {
            @Override
            public boolean onMove(@NonNull RecyclerView recyclerView, @NonNull RecyclerView.ViewHolder viewHolder, @NonNull RecyclerView.ViewHolder target) {
                return false;
            }

            @Override
            public void onSwiped(@NonNull RecyclerView.ViewHolder viewHolder, int direction) {
                int swipedPosition = viewHolder.getAdapterPosition();
                Toast.makeText(MainActivity.this,Integer.toString(swipedPosition)+"のアイテムを削除",Toast.LENGTH_LONG).show();


                removeData(swipedPosition);//DB上での削除
                adapter.removeItemOnRecycleview(swipedPosition,titleList);//リストの更新
            }
        };
        (new ItemTouchHelper(callback)).attachToRecyclerView(recyclerView);

        //========================================================================================//
        // Buttonの実装                                                                           //
        //========================================================================================//
        Button addButton = findViewById(R.id.my_button1);
        addButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Toast.makeText(MainActivity.this,"ボタンタップ",Toast.LENGTH_LONG).show();
                //RecyclerViewAdapter adapter = (RecyclerViewAdapter)recyclerView.getAdapter();
                add("new Memo","new Detail");
            }
        });
    }
    //========================================================================//
    // Listの初期化                                                            //
    //========================================================================//
    private void clearList(){
        idList.clear();
        titleList.clear();
        detailList.clear();
    }
    //========================================================================//
    // 読み込み(画面生成時のみ)                                                 //
    //========================================================================//
    private void readData(){
        if(helper == null){
            helper = new DBOpenHelper(getApplicationContext());
        }

        if(db == null){
            db = helper.getReadableDatabase();
        }

        Cursor cursor = db.query(
                "MemoTable",
                new String[]{"_id","MemoTitleColumn","MemoDetailColumn"},
                null,
                null,
                null,
                null,
                null
        );

        cursor.moveToFirst();

        for(int i=0;i<cursor.getCount();i++){
            idList.add(cursor.getInt(0));//DBのIDと、RecyclerView内のItemのIDと対応付けるために使用する
            titleList.add(cursor.getString(1));//1:TitleColumn
            detailList.add(cursor.getString(2));//2:DetailColumn
            cursor.moveToNext();
        }

        cursor.close();
    }

    //============================================================================================//
    // メモ新規作成                                                                                //
    //============================================================================================//
    public static void add(String enteredTitle, String enteredDetail){
        /* Listの更新  */
        titleList.add(enteredTitle);
        detailList.add(enteredDetail);
        /* DBの更新    */
        int insertPosition = titleList.size() - 1;
        ContentValues cv = new ContentValues();
        cv.put("MemoTitleColumn",titleList.get(insertPosition));
        cv.put("MemoDetailColumn",detailList.get(insertPosition));
        db.insert("MemoTable",null,cv);
        /* idListの更新(idは自動でインクリメントされるためDB更新後にidListに反映させる   */
        int newId = 0;//idListに追加する要素。for分にて必ず上書きされるから初期値0でも問題なし

        Cursor cursor = db.query(
                "MemoTable",
                new String[]{"_id","MemoTitleColumn","MemoDetailColumn"},
                null,
                null,
                null,
                null,
                null
        );

        cursor.moveToFirst();

        for(int i=0; i<cursor.getCount(); i++){
            newId = cursor.getInt(0);
            cursor.moveToNext();
        }

        idList.add(newId);

        /* RecycleViewの更新   */
        if(AdapterKeeper.GetAdapter() != null){
            AdapterKeeper.GetAdapter().InsertItemOnRecycleview(insertPosition,titleList);
        }else{
            Toast.makeText(context,"null",Toast.LENGTH_SHORT).show();
        }
    }
    //============================================================================================//
    // メモ編集                                                                                    //
    //============================================================================================//
    public static void editData(int editDataId, String editedTitle, String editedDetail){//RecycleView上のIDが渡される
        if(titleList.get(editDataId)==null){
            Toast.makeText(context,"null",Toast.LENGTH_LONG).show();
        }
        else{
            /* Listの編集  */
            titleList.set(editDataId,editedTitle);//こいつらは0~でありRecycleViewの番号とリンクしているので変換不要
            detailList.set(editDataId,editedDetail);
            /* DBの編集    */
            String args = Integer.toString(idList.get(editDataId));//DB上のIDに変換
            ContentValues val = new ContentValues();
            val.put("MemoTitleColumn",editedTitle);
            val.put("MemoDetailColumn",editedDetail);
            db.update("MemoTable",val,"_id = ?",new String[]{args});
            /* RecycleViewの更新   */
            AdapterKeeper.GetAdapter().UpdateItemOnRecyclervier(editDataId);
        }

    }
    //============================================================================================//
    // メモ削除                                                                                    //
    //============================================================================================//
    private void removeData(int removeTargetId) {//RecycleView上のIDが渡される
        /* Listの更新  */
        titleList.remove(removeTargetId);//こいつらは0~でありRecycleViewの番号とリンクしているので返還不要
        detailList.remove(removeTargetId);
        /* DBの更新    */
        String args = Integer.toString(idList.get(removeTargetId));//DB上の削除対象となるidに変換
        db.delete("MemoTable", "_id = ?", new String[]{args});
        /* idListの更新(idListはDB更新時にはまだ削除してはいけないので、ここで消す)    */
        idList.remove(removeTargetId);
    }
    //============================================================================================//
    // getter                                                                                     //
    //============================================================================================//
    public static String GetTitleData(int tappedPosition){//渡されてきたRecycleView上のIDに対応したTitleを返す
        return titleList.get(tappedPosition);
    }

    public static String GetDetailData(int tappedPosition){
        return detailList.get(tappedPosition);
    }
    //============================================================================================//
    // バックキーの実装                                                                             //
    //============================================================================================//
    @Override
    public void onBackPressed(){//バックキーを押したときの挙動を上書き
        Toast.makeText(context, "このアクティビティではバックキーは無効です",Toast.LENGTH_SHORT).show();
    }
}
