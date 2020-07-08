package com.example.testmemoapp2_recyclerview;

import android.content.ContentValues;
import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.ItemTouchHelper;
import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;

public class RecyclerViewAdapter extends RecyclerView.Adapter<RecyclerViewAdapter.ViewHolder>{

    private Context context;
    private static ArrayList<Integer> idList = new ArrayList<>();
    private static ArrayList<String> titleList = new ArrayList<>();//アイテム生成時にテキスト設定する際にしか使用しない
                                                                    //なぜならこのクラスはリサイクルビュー生成のためだけのクラス
    //private static ArrayList<String> detailList = new ArrayList<>();

    // Provide a reference to the views for each data item
    // Complex data items may need more than one view per item, and
    // you provide access to all the views for a data item in a view holder
    static class ViewHolder extends RecyclerView.ViewHolder {

        // each data item is just a string in this case
        TextView mTextView;

        ViewHolder(View v) {
            super(v);
            mTextView = (TextView)v.findViewById(R.id.text_view);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    RecyclerViewAdapter(Context context,ArrayList<Integer> myIdList, ArrayList<String> myTitleList){
        this.context = context;
        idList = myIdList;
        titleList = myTitleList;
    }

    // Create new views (invoked by the layout manager)
    @Override
    @NonNull
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        // create a new view
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.list_item, parent, false);

        // set the view's size, margins, paddings and layout parameters

        return new ViewHolder(view);
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, final int position) {
        // - get element from your dataset at this position
        // - replace the contents of the view with that element
        //holder.mTextView.setText(dataList.get(position));
        holder.mTextView.setText(titleList.get(position));
        final int itemId = position;

        //=================================================//
        // Itemをタップした時の処理を実装                    //
        //=================================================//
        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                /* 画面遷移処理   */
                Intent intent = new Intent(context,ItemDetail.class);
                intent.putExtra("com.example.testmemoapp2_recyclerview.SelectedItemId",itemId);//タップされたItemのIDを渡す
                context.startActivity(intent);
            }
        });
    }

    //========================================================================================//
    // 以下、リストの処理                                                                       //
    //========================================================================================//
    @Override
    public int getItemCount() {
        return titleList.size();
    }

    public void removeItemOnRecycleview(int removedItemPosition,ArrayList<String> newTitleList){
        titleList = newTitleList;//DB上のデータを反映
        notifyItemRemoved(removedItemPosition);
        notifyItemRangeChanged(removedItemPosition,titleList.size());//これを呼ぶことでリバインドしてくれる
        //↑これを使えばInsertItemOnRecyclerView()で、間にメモを追加したりもできる
    }

    public void InsertItemOnRecycleview(int insertItemPosition,ArrayList<String> newTitleList){//RecycleView上のアイテムを更新する
        titleList = newTitleList;//DB上のデータを反映
        notifyItemInserted(insertItemPosition);
    }

    public void UpdateItemOnRecyclervier(int editedPosition){
        notifyItemChanged(editedPosition);
    }
}
