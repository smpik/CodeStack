package com.example.testmemoapp2_recyclerview;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.KeyEvent;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

public class ItemDetail extends AppCompatActivity {

    EditText titleEditText;
    EditText detailEditText;
    int selectedItemId;//タップされたRecycleView上のitemのID

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_item_detail);

        /* 前画面からのデータの受け取り   */
        Intent intent = this.getIntent();
        selectedItemId = intent.getIntExtra("com.example.testmemoapp2_recyclerview.SelectedItemId",99);
        Toast.makeText(ItemDetail.this,Integer.toString(selectedItemId),Toast.LENGTH_LONG).show();

        /* EditTextを取得  */
        titleEditText = findViewById(R.id.my_title_edit_text);//Title入力用のEditTextを取得
        detailEditText= findViewById(R.id.my_detail_edit_text);//Detail入力用のEditTextを取得

        /* EditTextに文字をセット  */
        String selectedItemTitle = MainActivity.GetTitleData(selectedItemId);//タップされたItemのTitleを取得
        String selectedItemDetail= MainActivity.GetDetailData(selectedItemId);//タップされたItemのDetailを取得
        titleEditText.setText(selectedItemTitle);
        detailEditText.setText(selectedItemDetail);
    }

    //========================================================================================//
    // バックキーの処理                                                                        //
    //========================================================================================//
    @Override
    public void onBackPressed(){//バックキーを押したときの挙動を上書き
        saveEditTexts();//現在のEditTextの内容を保存
        Intent intent = new Intent(ItemDetail.this,MainActivity.class);
        ItemDetail.this.startActivity(intent);
    }
    private void saveEditTexts()
    {
        /* 入力されている文字の取得 */
        String enteredTitle = titleEditText.getText().toString();
        String enteredDetail= detailEditText.getText().toString();

        MainActivity.editData(selectedItemId,enteredTitle,enteredDetail);//RecycleView上のIDを渡す
        Toast.makeText(ItemDetail.this,"保存",Toast.LENGTH_LONG).show();
    }
}
