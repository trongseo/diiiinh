package com.mkyong.android;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.ImageView;
import android.view.View;
import android.view.View.OnClickListener;

public class MyAndroidAppActivity extends Activity {

	Button button;
	ImageView image;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);

		addListenerOnButton();

	}

	public void addListenerOnButton() {

		image = (ImageView) findViewById(R.id.imageView1);

		button = (Button) findViewById(R.id.btnChangeImage);
		button.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				image.setImageResource(R.drawable.android3d);
			}

		});

	}

}