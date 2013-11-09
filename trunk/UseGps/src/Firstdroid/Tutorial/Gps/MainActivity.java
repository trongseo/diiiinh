package Firstdroid.Tutorial.Gps;

import android.app.Activity;
import android.content.Context;
import android.location.Location;
import android.location.LocationManager;
import android.os.Bundle;
import android.telephony.TelephonyManager;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;
import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.util.ByteArrayBuffer;

public class MainActivity extends Activity
{
    int icount=0;
    private int isbreak;
     Button btnRun;
      Button btnStop;
    Thread thread;
    TextView txtView;
    int wait_tamp =1;
    double lat_end =0;
    double lng_end=0;
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
     
         btnRun.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
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
    
     public String getMyPhoneNumber()
    {
        String timlum = ((TelephonyManager) getSystemService(TELEPHONY_SERVICE)).getSubscriberId();
        timlum +=""+((TelephonyManager) getSystemService(TELEPHONY_SERVICE)).getLine1Number();
        return timlum;

    }
     
      public String postData(String valuepost)   {
        try {
            HttpClient httpclient = new DefaultHttpClient();
           HttpPost httppost = new HttpPost("http://nuocsachtayninh.vn/itemimage/uploadfile/postlocation.aspx"); 
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
            gps[0] = l.getLatitude();
            gps[1] = l.getLongitude();
        }
        return gps;
    }
    
}
