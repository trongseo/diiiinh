/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package show.hide;

import android.app.LauncherActivity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;

 public class LaunchAppViaDialReceiver extends BroadcastReceiver {
@Override
public void onReceive(Context context, Intent intent) {
    // TODO Auto-generated method stub
    Bundle bundle = intent.getExtras();
    if (null == bundle)
        return;
    String phoneNubmer = intent.getStringExtra(Intent.EXTRA_PHONE_NUMBER);
             //here change the number to your desired number
    if (phoneNubmer.equals("12345")) {
        setResultData(null);
        Gaurdian.changeStealthMode(context,
                PackageManager.COMPONENT_ENABLED_STATE_ENABLED);
        Intent appIntent = new Intent(context, LauncherActivity.class);
        appIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        context.startActivity(appIntent);
    }

}

