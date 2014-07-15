/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Simple.Tracker;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 *
 * @author Rong Viet
 */
 public class BootUpReceiver extends BroadcastReceiver{

        @Override
        public void onReceive(Context context, Intent intent) {
                Intent i = new Intent(context, MainActivity.class);  
                i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                context.startActivity(i);  
        }

}
