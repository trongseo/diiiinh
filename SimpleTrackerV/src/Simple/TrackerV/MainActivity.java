package Simple.TrackerV;


import android.app.Activity;
import android.app.ActivityManager;
import android.app.LauncherActivity;
import android.content.ComponentName;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import java.util.List;

public class MainActivity extends Activity
{
    public static ComponentName cpN ;
     Button btnHide;
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        btnHide = (Button)findViewById(R.id.btnHide);
         btnHide.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                
                         Intent startMain = new Intent(Intent.ACTION_MAIN);
startMain.addCategory(Intent.CATEGORY_HOME);
startMain.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
startActivity(startMain);



 cpN =  getComponentName();
  PackageManager p = getPackageManager();
  p.setComponentEnabledSetting(cpN, PackageManager.COMPONENT_ENABLED_STATE_DISABLED, PackageManager.DONT_KILL_APP);

 
 
    
                        }
		});
    }
}
