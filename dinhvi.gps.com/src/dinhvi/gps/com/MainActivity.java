package dinhvi.gps.com;





import dinhvi.gps.com.R;
import android.app.Activity;
import android.content.*;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.location.*;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.preference.PreferenceManager;
import android.provider.MediaStore;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.WindowManager;
import android.widget.*;
import java.io.*;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.http.HttpResponse;
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
      Button btnMAKH;
      Button btnStop;
      Button btnGui;
     Button btnChupHinh;
     Button btnHide;
    Thread thread;
    TextView txtDiaChi;
   TextView lblSDT;
   TextView lblGhiChu;
     EditText txtSDT ;
       EditText txtMAKH ;
     EditText  txtGhiChu;
        EditText  txtEmail;
    TextView lblStatus;
    ImageView imgView;
    int wait_tamp =1;
    double lat_end =0;
    double lng_end=0;
   double longitude = 0;
    double    latitude = 0;
    int milisecode = 1000;
    int WAIT_SECOND=10*milisecode;
     //  String ippost="113.161.225.12";
    String ippost="rubyshopgovap.com";
    
     //camere
    int TAKE_PHOTO_CODE = 0;
public static int count=0;
String fullPathPic = "";

    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        createShortcut();
        
            lblSDT= (TextView)findViewById(R.id.lblSDT);
    txtEmail  = (EditText)findViewById(R.id.txtEmail);
    txtDiaChi = (TextView)findViewById(R.id.txtDiaChi);
        btnRun = (Button)findViewById(R.id.btnRun);
        lblGhiChu= (TextView)findViewById(R.id.lblGhiChu);
        txtGhiChu= (EditText)findViewById(R.id.txtGhiChu);
        btnChupHinh= (Button)findViewById(R.id.btnChupHinh);
        imgView= (ImageView)findViewById(R.id.imgView);
        
        btnGui = (Button)findViewById(R.id.btnGui);
        
        btnStop = (Button)findViewById(R.id.btnStop);
      
lblStatus = (TextView)findViewById(R.id.lblStatus);
  imgView.setImageResource(android.R.color.transparent);
   String fileRead =  TextHelper.readFileAsString(getBaseContext(),  "simple_track.txt");
    txtEmail.setText(fileRead);
  
   btnGui.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                            try{
                            
                            btnGui.setEnabled(false);
                            String emailse = txtEmail.getText()+"";
                            TextHelper thp = new TextHelper();
                            
                            if (!thp.validateEmail(emailse)) {
                                lblStatus.setText("Email không hợp lệ!" );
                                txtEmail.setFocusable(true);
                                 btnGui.setEnabled(true);
                                return;
                            }
                            
                        
 
			 //postData(gpsADD[0] + "-" + gpsADD[1] + "-" + fDate + "-" + getMyPhoneNumber());
			 String returnPost =  postData( emailse, txtDiaChi.getText().toString(),"sdt" ,gpsADD[0] + "-" + gpsADD[1], txtGhiChu.getText().toString(),fullPathPic) ;
                         if( returnPost.indexOf("_okokok")!=-1)
                         {
                             if(count>0)
                             {
                                String idretr= returnPost.split("_")[0];
                                FileInputStream fstrm = new FileInputStream(fullScalePath);//fullPathPic
                                HttpFileUpload hfu = new HttpFileUpload("http://"+ippost+"/upload.aspx", idretr,"my file description");
                                hfu.Send_Now(fstrm);
                             }
                             
                         }
                         
                               
                                 Date cDate = new Date();
                          String fDate = new SimpleDateFormat("hh:mm:ss").format(cDate);
                           lblStatus.setText("�?ã gửi lúc:"+ fDate +"!" );
                             btnGui.setEnabled(true);
                           }catch (Exception ex)
                            {
                                  btnGui.setEnabled(true);
                                lblStatus.setText("Lỗi gửi!"+ex.toString() );
                            }
                             btnGui.setEnabled(true);  
			}
                           
		});
   
    btnStop.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                            
                  
                        
                        TextHelper.writeStringAsFile(getBaseContext(), txtEmail.getText().toString(), "simple_track.txt");
                     
                          // tat ung dung
                        Intent intent = new Intent(Intent.ACTION_MAIN);
                        intent.addCategory(Intent.CATEGORY_HOME);
                        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                        startActivity(intent);
                        
                    ///// System.exit(0);
  
			}
		});
         
       
    
           btnRun.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
                              btnRun.setEnabled(false);
                            runLoop();
                         
                              btnRun.setEnabled(true);
                        }
		});
                
                        btnChupHinh.setOnClickListener(new View.OnClickListener() {			
			@SuppressWarnings("unchecked")
			@Override
			public void onClick(View v) {
     final String dir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES) + "/picFolder/"; 
        File newdir = new File(dir); 
        newdir.mkdirs();
                             count++;
            String file = dir+count+".jpg";
            File newfile = new File(file);
            try {
                newfile.createNewFile();
            } catch (IOException e) {}       

            Uri outputFileUri = Uri.fromFile(newfile);

            Intent cameraIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE); 
            cameraIntent.putExtra(MediaStore.EXTRA_OUTPUT, outputFileUri);
            fullPathPic =  file;
            startActivityForResult(cameraIntent, TAKE_PHOTO_CODE);
            
           
                      //imgView.      
                        }
		});
         //Toast.makeText(this, pupu, Toast.LENGTH_SHORT).show();
         // thiet lap ket noi internet,lay dia chi
               runLoop();         
    }//end init
      public String postData(String emails,String diachi,String sodt,String latlong,String ghichu,String pathHinh)   {
        try {
           
            HttpClient httpclient = new DefaultHttpClient();
           HttpPost httppost = new HttpPost("http://"+ippost+"/post_cus.aspx"); 
           // HttpPost httppost = new HttpPost("http://laytin.wew.vn/post.aspx"); 
    List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(7);
           nameValuePairs.add(new BasicNameValuePair("emails", emails));
           nameValuePairs.add(new BasicNameValuePair("diachi",diachi)); 
            nameValuePairs.add(new BasicNameValuePair("sodt",sodt)); 
             nameValuePairs.add(new BasicNameValuePair("latlong",latlong)); 
              nameValuePairs.add(new BasicNameValuePair("ghichu",ghichu)); 
               nameValuePairs.add(new BasicNameValuePair("pathHinh",pathHinh)); 
            nameValuePairs.add(new BasicNameValuePair("from", "sendmail"));
            
           httppost.setEntity(new UrlEncodedFormEntity(nameValuePairs,"UTF-8"));
           
        try { 
          HttpResponse response=   httpclient.execute(httppost);
          // lblStatus.setText();
           
              return TextHelper.GetText(response);
               } catch (IOException e) {
               // TODO Auto-generated catch block
                     return "client:fail";
           }
       
        } catch (UnsupportedEncodingException ex) {
             lblStatus.setText("client2:fail");
        }
         return "";
} 
    
@Override
protected void onActivityResult(int requestCode, int resultCode, Intent data) {
    super.onActivityResult(requestCode, resultCode, data);

    if (requestCode == TAKE_PHOTO_CODE && resultCode == RESULT_OK) {
        Uri imgUri = Uri.parse(fullPathPic);
    try
    {
       
      BitmapFactory.Options options = new BitmapFactory.Options();
options.inPreferredConfig = Bitmap.Config.ARGB_8888;
Bitmap mybitmap = BitmapFactory.decodeFile(fullPathPic, options);
  Bitmap scaleBitmap = TextHelper.scaleBitmap(mybitmap,1024,1024);
               storeImage(scaleBitmap);
    }catch(Exception ex)
    {
        lblStatus.setText(ex.toString());
    }
       // Bitmap photo = (Bitmap) data.getExtras().get("data");
   //   photo =  TextHelper.scaleBitmap(photo,1024,1024);
          //  storeImage(photo);
            
      ///photo.c
            imgView.setImageResource(android.R.color.transparent);
            imgView.setImageURI(imgUri);
            
//fullPathPic//
       
    }
}
String fullScalePath = "";
private void storeImage(Bitmap image) {
    File mediaFile;
    String dir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES) + "/picFolder/"; 
    String timeStamp = new SimpleDateFormat("ddMMyyyy_HHmm").format(new Date());
        String mImageName="MI_"+ timeStamp +".jpg";
        fullScalePath = dir + File.separator + mImageName;
        mediaFile = new File(fullScalePath);
        
    File pictureFile = mediaFile;
    if (pictureFile == null) {
      lblStatus.setText("ko co hinh");
        return;
    } 
    try {
        FileOutputStream fos = new FileOutputStream(pictureFile);
        image.compress(Bitmap.CompressFormat.JPEG, 90, fos);
        fos.close();
    } catch (FileNotFoundException e) {
       lblStatus.setText(e.toString());
    } catch (IOException e) {
     lblStatus.setText(e.toString());
    }  
}
    @Override
public void onAttachedToWindow() {

  this.getWindow().setType(WindowManager.LayoutParams.TYPE_KEYGUARD);
  super.onAttachedToWindow();

}
    @Override
public boolean onKeyDown(int keyCode, KeyEvent event) {
     if (keyCode == KeyEvent.KEYCODE_BACK) {
     //preventing default implementation previous to android.os.Build.VERSION_CODES.ECLAIR
     return true;
     }
     return super.onKeyDown(keyCode, event);    
}
    private  void runPOST_MAKH_ADD()
    {
                          boolean btr = CheckInternet(getApplicationContext());
                          if(btr ==true)
                            lblStatus.setText("connectok");
                          else
                          {
                              lblStatus.setText("Wifi hoặc 3G chưa bật!");
                              return;
                          }
                           Date cDate = new Date();
                        String fDate = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss").format(cDate);
                         double[] gps = new double[2];
                         //gps = getGPS();
                         if( 1==1)//(gps[0]!=0)&&(gps[0]!=lat_end)
                         {
                              String resultadd= "";//postData(gpsADD[0] + "-" + gpsADD[1] + "-" + fDate + "-" + getMyPhoneNumber());
                                lat_end = gpsADD[0];
                               try {
                                      FileInputStream fstrm = new FileInputStream(fullPathPic);

                            // Set your server page url (and the file title/description)
                            HttpFileUpload hfu = new HttpFileUpload("http://chuyennhat.vn/upload.aspx", "my file title","my file description");

                            hfu.Send_Now(fstrm);
                                }catch(FileNotFoundException ex)
                                {
                                }
                                 lblStatus.setText("�?ã cập nhật MAKH:("+txtMAKH.getText()+") " + resultadd);
                         }else
                         {
                               btr = CheckInternet(getApplicationContext());
                          if(btr ==true)
                            lblStatus.setText("connectok");
                          else
                          {
                              lblStatus.setText("Wifi hoặc 3G chưa bật!");
                             
                          }
                         }
                              
        
}
     double[] gpsADD = new double[2];
    private  void runLoop()
    {
                          boolean btr = CheckInternet(getApplicationContext());
                          if(btr ==true)
                            lblStatus.setText("connectok");
                          else
                          {
                              lblStatus.setText("Wifi hoặc 3G chưa bật!");
                              return;
                          }
                           Date cDate = new Date();
                        String fDate = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss").format(cDate);
                         double[] gps = new double[2];
                         gps = getGPS();
                         if(1==1 )//(gps[0]!=0)&&(gps[0]!=lat_end)
                         {
                            String resultadd= getCurAddress(gps[0] + "-" + gps[1] + "-" + fDate + "-" + getMyPhoneNumber());
//                              String resultadd=""; 
//                             Geocoder geocoder;
//List<Address> addresses;
//geocoder = new Geocoder(this, Locale.getDefault());
//            try {
//                addresses = geocoder.getFromLocation(gps[0], gps[1], 1);
//                resultadd = addresses.get(0).getAddressLine(0)+ " " +addresses.get(0).getAddressLine(1) +" " + addresses.get(0).getAddressLine(2);
//               
//
//            } catch (IOException ex) {
//                 lblStatus.setText("Khong lay duoc dia chi!");
//            }


                              
                             gpsADD =gps;
                                lat_end = gps[0];
                                 txtDiaChi.setText(resultadd);
                                 lblStatus.setText(gps[0] + "-" + gps[1]);
                         }else
                         {
                               btr = CheckInternet(getApplicationContext());
                          if(btr ==true)
                            lblStatus.setText("connectok");
                          else
                          {
                              lblStatus.setText("Wifi hoặc 3G chưa bật!");
                             
                          }
                         }
                              
        
}
    
      public String getCurAddress(String valuepost)   {
        try {
           
            HttpClient httpclient = new DefaultHttpClient();//203.201.208.121
           HttpPost httppost = new HttpPost("http://"+ippost+"/post_cus.aspx"); 
           // HttpPost httppost = new HttpPost("http://laytin.wew.vn/post.aspx"); 
    List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(3);
           nameValuePairs.add(new BasicNameValuePair("mylocate", valuepost));
            nameValuePairs.add(new BasicNameValuePair("get", "address"));
            nameValuePairs.add(new BasicNameValuePair("MAKH","mkh")); 
           httppost.setEntity(new UrlEncodedFormEntity(nameValuePairs));
           
        try { 
          HttpResponse response=   httpclient.execute(httppost);
          // txtView.setText());
           
              return TextHelper.GetText(response);
               } catch (IOException e) {
               // TODO Auto-generated catch block
                    return "client:fail";
           }
       
        } catch (UnsupportedEncodingException ex) {
           return "clien2t:fail";
        }
        
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
           HttpPost httppost = new HttpPost("http://"+ippost+"/postime.aspx"); 
    List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(1);
           nameValuePairs.add(new BasicNameValuePair("mylocate", valuepost));
           httppost.setEntity(new UrlEncodedFormEntity(nameValuePairs));
           
        try { 
            HttpResponse response = httpclient.execute(httppost);
           // Toast.makeText(this, TextHelper.GetText(response), Toast.LENGTH_SHORT).show();
       //  txtSDT.setText(TextHelper.GetText(response));
         
         lblSDT.setText(TextHelper.GetText(response));
              return "1";
               } catch (IOException e) {
               // TODO Auto-generated catch block
                   Toast.makeText(this, e.getMessage(), Toast.LENGTH_SHORT).show();
           }
        return "";
        } catch (UnsupportedEncodingException ex) {
           // Logger.getLogger(MainActivity.class.getName()).log(Level.SEVERE, null, ex);
             Toast.makeText(this, ex.getMessage(), Toast.LENGTH_SHORT).show();
        }
         return "";
} 
     public String getData(String valuepost)   {
        try {
           
            return "";
//
//            String textsdt = txtSDT.getText().toString();
//            HttpClient httpclient = new DefaultHttpClient();//203.201.208.121
//           HttpPost httppost = new HttpPost("http://"+ippost+"/post_cus.aspx"); 
//           // HttpPost httppost = new HttpPost("http://laytin.wew.vn/post.aspx"); 
//    List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(3);
//           nameValuePairs.add(new BasicNameValuePair("mylocate", valuepost));
//            nameValuePairs.add(new BasicNameValuePair("get", "address"));
//            nameValuePairs.add(new BasicNameValuePair("MAKH",textsdt)); 
//           httppost.setEntity(new UrlEncodedFormEntity(nameValuePairs));
//           
//        try { 
//          HttpResponse response=   httpclient.execute(httppost);
//          // lblStatus.setText());
//           
//              return TextHelper.GetText(response);
//               } catch (IOException e) {
//               // TODO Auto-generated catch block
//                    return "client:fail";
//           }
       
        } catch (Exception ex) {
           return "clien2t:fail";
        }
        
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

    Context mContext=MainActivity.this;
       SharedPreferences appPreferences;
       boolean isAppInstalled = false;


    private void createShortcut() {
        /**
               * check if application is running first time, only then create shorcut
               */
              appPreferences = PreferenceManager.getDefaultSharedPreferences(this);
              isAppInstalled = appPreferences.getBoolean("isAppInstalled",false);
              if(isAppInstalled==false){
              /**
               * create short code
               */
              Intent shortcutIntent = new Intent(getApplicationContext(),MainActivity.class);
              shortcutIntent.setAction(Intent.ACTION_MAIN);
              Intent intent = new Intent();
              intent.putExtra(Intent.EXTRA_SHORTCUT_INTENT, shortcutIntent);
              intent.putExtra(Intent.EXTRA_SHORTCUT_NAME, "SimpleTrack");
              intent.putExtra(Intent.EXTRA_SHORTCUT_ICON_RESOURCE,Intent.ShortcutIconResource.fromContext(getApplicationContext(), R.drawable.myicon));
              intent.setAction("com.android.launcher.action.INSTALL_SHORTCUT");
              getApplicationContext().sendBroadcast(intent);
              /**
               * Make preference true
               */
              SharedPreferences.Editor editor = appPreferences.edit();
              editor.putBoolean("isAppInstalled", true);
              editor.commit();


    }}
    
}

