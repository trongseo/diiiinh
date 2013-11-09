/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Firstdroid.Tutorial.Gps;

import android.util.Log;

/**
 *
 * @author User
 */
public class SomeBackgroundProcess implements Runnable {

    Thread backgroundThread;

    public void start() {
       if( backgroundThread == null ) {
          backgroundThread = new Thread( this );
          backgroundThread.start();
       }
    }

    public void stop() {
       if( backgroundThread != null ) {
          backgroundThread.interrupt();
       }
    }

    public void run() {
        
           while( !backgroundThread.interrupted() ) {
              doSomething();
           }
          // Log.i("Thread stopping.");
        
           backgroundThread = null;
        
    }

    private void doSomething() {
        int s=0;
        
    }
}
