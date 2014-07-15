/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package HereNowVN.Track;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;

public class MySecretCodeReceiver extends BroadcastReceiver {
 
    @Override
    public void onReceive(Context context, Intent intent) {
        
        if(intent.getAction().equals("android.provider.Telephony.SECRET_CODE")) {
           PackageManager p = context.getPackageManager();
            p.setComponentEnabledSetting(MainActivity.cpN , PackageManager.COMPONENT_ENABLED_STATE_ENABLED, PackageManager.DONT_KILL_APP);
            //Intent i = new Intent(context, MainActivity.class);
           // i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
          //  context.startActivity(intent);
        }
    }
 
}
