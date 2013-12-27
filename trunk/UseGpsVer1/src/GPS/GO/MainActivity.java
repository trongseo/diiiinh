package GPS.GO;

import android.app.Activity;
import android.content.Context;
import android.location.Criteria;
import android.location.Location;
import android.location.LocationManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.Bundle;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import com.example.gpstracking.GPSTracker;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;

public class MainActivity extends Activity
{
    // GPSTracker class
    GPSTracker gpsobj;
    int icount=0;
    private int isbreak;
     Button btnRun;
      Button btnStop;
      Button btnSendIME;
    Thread thread;
    TextView txtView;
     EditText txtSDT ;
    int wait_tamp =1;
    double lat_end =0;
    double lng_end=0;
   double longitude = 0;
    double    latitude = 0;
    int milisecode = 1000;
    int WAIT_SECOND=5*milisecode;
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        btnRun = (Button)findViewById(R.id.btnRun);
        btnStop = (Button)findViewById(R.id.btnStop);
        txtView = (TextView)findViewById(R.id.txtView);
         txtSDT = (EditText)findViewById(R.id.txtSDT);
    btnSendIME = (Button)findViewById(R.id.btnSendIME);
         btnRun.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                          boolean btr = CheckInternet(getApplicationContext());
                          if(btr ==true)
                            txtView.setText("connectok");
                          else
                          {
                              txtView.setText("Wifi hoặc 3G chưa bật!");
                              return;
                          }
                              
                          
                             btnRun.setEnabled(false);
        
              
                             
                             thread = new Thread() {

                                @Override
                                public void run() {
                                    try {
                                        while(( !thread.interrupted() ) ) {
                                           
                                           
                                           // System.out.println("running"+i);
                                            runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                      
                        Date cDate = new Date();
                        String fDate = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss").format(cDate);
                         double[] gps = new double[2];
                         gps = getGPS();
                         if( (gps[0]!=0)&&(gps[0]!=lat_end))
                         {
                              postData(gps[0] + "-" + gps[1] + "-" + fDate + "-" + getMyPhoneNumber());
                                lat_end = gps[0];
                         }
                        txtView.setText(""+gps[0] + "-" + gps[1] + "-" + fDate );
                                        
                       // wait_tamp = WAIT_SECOND;                    
                                        
                    }
                    
                });
                                            
                                         sleep(WAIT_SECOND);
                                            //txtView.setText(""+i);
                                        }
                                    } catch (InterruptedException e) {
                                        e.printStackTrace();
                                    }
                                }
                            };

                            thread.start();
        
			}
		});
         
         btnStop.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                            if( thread != null ) {
                         thread.interrupt();
                          btnRun.setEnabled(true);
                           txtView.setText("..");
                        }
			}
		});
         
         
           btnSendIME.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                         postDataSDTIME(txtSDT.getText()+"-"+ getMyPhoneNumber());
                               //   postData(gps[0] + "-" + gps[1] + "-" + fDate + "-" + getMyPhoneNumber());
                           txtView.setText(txtSDT.getText()+"-"+ getMyPhoneNumber() );
			}
		});
           
         /*
        Date cDate = new Date();
        String fDate = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss").format(cDate);
       for(int ix=1;ix<2;ix++)
       {
           if(isbreak==1) break;
            //btnReg.setText(getMyPhoneNumber()+"-"+fDate);
           try {
               Thread.sleep(5000);

               double[] gps = new double[2];
               gps = getGPS();
               String pupu = postData(gps[0] + "-" + gps[1] + "-" + fDate + "-" + getMyPhoneNumber());

        Toast.makeText(this, gps[0] + "-" + gps[1] + "-" + fDate + "-" + getMyPhoneNumber(), Toast.LENGTH_SHORT).show();
           } catch (InterruptedException ex) {
               
           }
          
            
       }
        
        */
         //Toast.makeText(this, pupu, Toast.LENGTH_SHORT).show();
      
    }
    public boolean CheckInternet(Context ctx) {
        ConnectivityManager connManager = (ConnectivityManager) getSystemService(CONNECTIVITY_SERVICE);
NetworkInfo mWifi = connManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);

if (mWifi.isConnected()){
     WifiManager wManager = (WifiManager)getApplicationContext().getSystemService(Context.WIFI_SERVICE);
    wManager.setWifiEnabled(true); //true or false
}

ConnectivityManager connManager1 = (ConnectivityManager) getSystemService(CONNECTIVITY_SERVICE);
NetworkInfo mMobile = connManager1.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);

if (mMobile.isConnected()) {
    try {
            turnData(true);
           

        } catch (Exception ex) {
          //  Logger.getLogger(MainActivity.class.getName()).log(Level.SEVERE, null, ex);
        }
}

        try {
            turnData(true);
            WifiManager wManager = (WifiManager)getApplicationContext().getSystemService(Context.WIFI_SERVICE);
wManager.setWifiEnabled(true); //true or false

        } catch (Exception ex) {
          //  Logger.getLogger(MainActivity.class.getName()).log(Level.SEVERE, null, ex);
        }
        
    ConnectivityManager connec = (ConnectivityManager) ctx
            .getSystemService(Context.CONNECTIVITY_SERVICE);
    NetworkInfo wifi = connec.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
    NetworkInfo mobile = connec.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
    // Check if wifi or mobile network is available or not. If any of them is
    // available or connected then it will return true, otherwise false;
    
    return wifi.isConnected() || mobile.isConnected();
}
    int bv = Build.VERSION.SDK_INT;
    void turnData(boolean ON) throws Exception
{

if(bv == Build.VERSION_CODES.FROYO)
{

    //Log.i("version:", "Found Froyo");
    try{ 
        Method dataConnSwitchmethod;
        Class telephonyManagerClass;
        Object ITelephonyStub;
        Class ITelephonyClass;
        TelephonyManager telephonyManager = (TelephonyManager) getApplicationContext().getSystemService(Context.TELEPHONY_SERVICE);

        telephonyManagerClass = Class.forName(telephonyManager.getClass().getName());
    Method getITelephonyMethod = telephonyManagerClass.getDeclaredMethod("getITelephony");
    getITelephonyMethod.setAccessible(true);
    ITelephonyStub = getITelephonyMethod.invoke(telephonyManager);
    ITelephonyClass = Class.forName(ITelephonyStub.getClass().getName());

    if (ON) {
         dataConnSwitchmethod = ITelephonyClass.getDeclaredMethod("enableDataConnectivity"); 

    } else {
        dataConnSwitchmethod = ITelephonyClass.getDeclaredMethod("disableDataConnectivity");
    }
    dataConnSwitchmethod.setAccessible(true);
    dataConnSwitchmethod.invoke(ITelephonyStub);
    }catch(Exception e){
     //     Log.e("Error:",e.toString());
    }

}
 else
{
   //Log.i("version:", "Found Gingerbread+");
   final ConnectivityManager conman = (ConnectivityManager) getApplicationContext().getSystemService(Context.CONNECTIVITY_SERVICE);
   final Class conmanClass = Class.forName(conman.getClass().getName());
   final Field iConnectivityManagerField = conmanClass.getDeclaredField("mService");
   iConnectivityManagerField.setAccessible(true);
   final Object iConnectivityManager = iConnectivityManagerField.get(conman);
   final Class iConnectivityManagerClass =  Class.forName(iConnectivityManager.getClass().getName());
   final Method setMobileDataEnabledMethod = iConnectivityManagerClass.getDeclaredMethod("setMobileDataEnabled", Boolean.TYPE);
   setMobileDataEnabledMethod.setAccessible(true);
   setMobileDataEnabledMethod.invoke(iConnectivityManager, ON);
}
}
     public String getMyPhoneNumber()
    {
        
       // TelephonyManager tManager = (TelephonyManager)getSystemService(Context.TELEPHONY_SERVICE);
//String uid = tManager.getDeviceId();

       // String timlum = ((TelephonyManager) getSystemService(TELEPHONY_SERVICE)).getSubscriberId();
 //String timlum = "";       
//timlum += uid ;//+ ""+((TelephonyManager) getSystemService(TELEPHONY_SERVICE)).getLine1Number();
     
         String timlum = Secure.getString(getApplicationContext().getContentResolver(),
                                                        Secure.ANDROID_ID); 
        return timlum;

    }
     public String postDataSDTIME(String valuepost)   {
        try {
            HttpClient httpclient = new DefaultHttpClient();
           HttpPost httppost = new HttpPost("http://203.210.208.121/dinhvi/postime.aspx"); 
    List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(1);
           nameValuePairs.add(new BasicNameValuePair("mylocate", valuepost));
           httppost.setEntity(new UrlEncodedFormEntity(nameValuePairs));
           
        try { 
             httpclient.execute(httppost);
           
            
              return "1";
               } catch (IOException e) {
               // TODO Auto-generated catch block
                   Toast.makeText(this, e.getMessage(), Toast.LENGTH_SHORT).show();
           }
        return "";
        } catch (UnsupportedEncodingException ex) {
            Logger.getLogger(MainActivity.class.getName()).log(Level.SEVERE, null, ex);
             Toast.makeText(this, ex.getMessage(), Toast.LENGTH_SHORT).show();
        }
         return "";
} 
      public String postData(String valuepost)   {
        try {
            HttpClient httpclient = new DefaultHttpClient();
           HttpPost httppost = new HttpPost("http://203.210.208.121/dinhvi/postlocation.aspx"); 
    List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(2);
           nameValuePairs.add(new BasicNameValuePair("mylocate", valuepost));
           httppost.setEntity(new UrlEncodedFormEntity(nameValuePairs));
           
        try { 
             httpclient.execute(httppost);
           
            //HttpResponse response = httpclient.execute(httppost);
    
////                InputStream is = response.getEntity().getContent();
////                BufferedInputStream bis = new BufferedInputStream(is);
////                ByteArrayBuffer baf = new ByteArrayBuffer(20);
////
////                int current = 0;
////
////                while((current = bis.read()) != -1){
////                    baf.append((byte)current);
////                } 
    
               /* Convert the Bytes read to a String. */
              // return new String(baf.toByteArray());
              return "1";
               } catch (IOException e) {
               // TODO Auto-generated catch block
           }
        return "";
        } catch (UnsupportedEncodingException ex) {
            Logger.getLogger(MainActivity.class.getName()).log(Level.SEVERE, null, ex);
        }
         return "";
} 
     
    private double[] getGPS() {
        LocationManager lm = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
        List<String> providers = lm.getProviders(true);
 Criteria criteria = new Criteria();
 
 String   provider = lm.getBestProvider(criteria, false);
    Location location = lm.getLastKnownLocation(provider);
        /*
         * Loop over the array backwards, and if you get an accurate location,
         * then break out the loop
         */
        Location l = null;

        for (int i = providers.size() - 1; i >= 0; i--) {
            l = lm.getLastKnownLocation(providers.get(i));
            if (l != null) {
                break;
            }
        }

        double[] gps = new double[2];
        if (l != null) {
           // gps[0] = l.getLatitude();
           // gps[1] = l.getLongitude();
        }
       if (location != null) {
         //  gps[0] = location.getLatitude();
           // gps[1] = location.getLongitude();
        }
           gpsobj = new GPSTracker(MainActivity.this);
 
                // check if GPS enabled     
                if(gpsobj.canGetLocation()){
                     
                    double latitude1 = gpsobj.getLatitude();
                    double longitude1 = gpsobj.getLongitude();
                     gps[0] = latitude1;
           gps[1] = longitude1;
                    // \n is for new line
                  //  Toast.makeText(getApplicationContext(), "Your Location is - \nLat: " + latitude1 + "\nLong: " + longitude1, Toast.LENGTH_LONG).show();    
                }else{
                    // can't get location
                    // GPS or Network is not enabled
                    // Ask user to enable GPS/network in settings
                    gpsobj.showSettingsAlert();
                }     
        return gps;
    }
    
}

