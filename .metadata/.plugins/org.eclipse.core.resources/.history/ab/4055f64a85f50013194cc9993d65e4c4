/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package simple.tracker.com.vn;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Bitmap.Config;
import android.graphics.Canvas;
import android.graphics.Matrix;
import android.graphics.Paint;
import android.util.Log;
import java.io.*;
import java.util.logging.Logger;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import org.apache.http.HttpResponse;

/**
 *
 * @author Rong Viet
 */
class TextHelper {
  public static String GetText(InputStream in) {
    String text = "";
    BufferedReader reader = new BufferedReader(new InputStreamReader(in));
    StringBuilder sb = new StringBuilder();
    String line = null;
    try {
      while ((line = reader.readLine()) != null) {
      sb=  sb.append(line + "\n");
      }
      text = sb.toString();
    } catch (Exception ex) {

    } finally {
      try {

        in.close();
      } catch (Exception ex) {
      }
    }
    return text;
  }
  

public static void writeStringAsFile(Context context,final String fileContents, String fileName) {
         
        try {
            FileWriter out = new FileWriter(new File(context.getFilesDir(), fileName));
            out.write(fileContents);
            out.close();
        } catch (IOException e) {
            
        }
    }

    public static String readFileAsString( Context context,String fileName) {
     
        StringBuilder stringBuilder = new StringBuilder();
        String line;
        BufferedReader in = null;

        try {
            in = new BufferedReader(new FileReader(new File(context.getFilesDir(), fileName)));
            while ((line = in.readLine()) != null) stringBuilder.append(line);

        } catch (FileNotFoundException e) {
            
        } catch (IOException e) {
            
        } 

        return stringBuilder.toString();
    }


  public static String GetText(HttpResponse response) {
    String text = "";
    try {
      text = GetText(response.getEntity().getContent());
    } catch (Exception ex) {
    }
    return text;
  }
public static Bitmap scaleBitmap(Bitmap bitmap, int wantedWidth, int wantedHeight) {
    
    float ratio = Math.min(
            (float) wantedWidth / bitmap.getWidth(),
            (float) wantedHeight / bitmap.getHeight());
    int width = Math.round((float) ratio * bitmap.getWidth());
    int height = Math.round((float) ratio * bitmap.getHeight());

    Bitmap newBitmap = Bitmap.createScaledBitmap(bitmap, width,
            height, true);
    return newBitmap;
    
//        Bitmap output = Bitmap.createBitmap(wantedWidth, wantedHeight, Config.ARGB_8888);
//        Canvas canvas = new Canvas(output);
//        Matrix m = new Matrix();
//        m.setScale((float) wantedWidth / bitmap.getWidth(), (float) wantedHeight / bitmap.getHeight());
//        canvas.drawBitmap(bitmap, m, new Paint());
//
//        return output;
    }
   public boolean validateEmail(String email) {

Pattern pattern;
Matcher matcher;
String EMAIL_PATTERN = "^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
pattern = Pattern.compile(EMAIL_PATTERN);
matcher = pattern.matcher(email);
return matcher.matches();

}

}
