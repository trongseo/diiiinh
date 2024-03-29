/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Simple.Tracker;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
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

  public static String GetText(HttpResponse response) {
    String text = "";
    try {
      text = GetText(response.getEntity().getContent());
    } catch (Exception ex) {
    }
    return text;
  }
}
