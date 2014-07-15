/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package HereNowVN.Track;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import org.apache.http.HttpResponse;

/**
 *
 * @author Rong Viet
 */
class TextHelper {
    
public boolean sendEmail(string emailto,string subject1,string message1)
{
String to = emailto;
			  String subject = subject1;
			  String message = message1;
 
			  Intent email = new Intent(Intent.ACTION_SEND);
			  email.putExtra(Intent.EXTRA_EMAIL, new String[]{ to});
			  //email.putExtra(Intent.EXTRA_CC, new String[]{ to});
			  //email.putExtra(Intent.EXTRA_BCC, new String[]{to});
			  email.putExtra(Intent.EXTRA_SUBJECT, subject);
			  email.putExtra(Intent.EXTRA_TEXT, message);
 
			  //need this to prompts email client only
			  email.setType("message/rfc822");
 
			  startActivity(Intent.createChooser(email, "Choose an Email client :"));
 
}
public boolean validateEmail(String email) {

Pattern pattern;
Matcher matcher;
String EMAIL_PATTERN = "^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
pattern = Pattern.compile(EMAIL_PATTERN);
matcher = pattern.matcher(email);
return matcher.matches();

}



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

  public static String GetText(HttpResponse response) {
    String text = "";
    try {
      text = GetText(response.getEntity().getContent());
    } catch (Exception ex) {
    }
    return text;
  }
}
