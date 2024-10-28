/*
 * IPWorks S/MIME 2024 Java Edition - Sample Project
 *
 * This sample project demonstrates the usage of IPWorks S/MIME in a 
 * simple, straightforward way. It is not intended to be a complete 
 * application. Error handling and other checks are simplified for clarity.
 *
 * www.nsoftware.com/ipworkssmime
 *
 * This code is subject to the terms and conditions specified in the 
 * corresponding product license agreement which outlines the authorized 
 * usage and restrictions.
 */

import java.io.*;

import java.io.*;

import java.io.*;

import ipworkssmime.*;

public class certmgr extends ConsoleDemo {
  CertMgr certmgr1;
  int i = 0;
  public certmgr() {
    try {
      certmgr1 = new CertMgr();
      certmgr1.addCertMgrEventListener(new CertMgrEvents(this));
      certmgr1.setCertStoreType(Certificate.cstPFXFile);
      certmgr1.setCertStore(prompt("Please enter key store path",":","test.pfx"));
      certmgr1.setCertStorePassword(prompt("Please enter store password",":","test"));
      certmgr1.listStoreCertificates();
    }
    catch (IPWorksSMIMEException ex) {
      System.out.println("IPWorksSMIMEException thrown: " + ex.getCode() + " [" +
          ex.getMessage() + "].");
    }
    catch (Exception ex) {
      System.out.println(ex.getMessage());
    }
  }

  public static void main(String[] args) {
    new certmgr();
  }

  public void certList(CertMgrCertListEvent args) {
    i++;
    System.out.println(i + ". " + args.certSubject);
  }
}

class CertMgrEvents
    implements CertMgrEventListener{
  certmgr instance;
  public CertMgrEvents(certmgr instance) {
    this.instance = instance;
  }

  public void certChain(CertMgrCertChainEvent args) {

  }

  public void certList(CertMgrCertListEvent args) {
    instance.certList(args);
  }

  public void error(CertMgrErrorEvent args) {

  }

  public void keyList(CertMgrKeyListEvent args) {

  }

  public void storeList(CertMgrStoreListEvent args) {

  }
  
  public void log(CertMgrLogEvent args){}
}

class ConsoleDemo {
  private static BufferedReader bf = new BufferedReader(new InputStreamReader(System.in));

  static String input() {
    try {
      return bf.readLine();
    } catch (IOException ioe) {
      return "";
    }
  }
  static char read() {
    return input().charAt(0);
  }

  static String prompt(String label) {
    return prompt(label, ":");
  }
  static String prompt(String label, String punctuation) {
    System.out.print(label + punctuation + " ");
    return input();
  }
  static String prompt(String label, String punctuation, String defaultVal) {
      System.out.print(label + " [" + defaultVal + "]" + punctuation + " ");
      String response = input();
      if (response.equals(""))
        return defaultVal;
      else
        return response;
  }

  static char ask(String label) {
    return ask(label, "?");
  }
  static char ask(String label, String punctuation) {
    return ask(label, punctuation, "(y/n)");
  }
  static char ask(String label, String punctuation, String answers) {
    System.out.print(label + punctuation + " " + answers + " ");
    return Character.toLowerCase(read());
  }

  static void displayError(Exception e) {
    System.out.print("Error");
    if (e instanceof IPWorksSMIMEException) {
      System.out.print(" (" + ((IPWorksSMIMEException) e).getCode() + ")");
    }
    System.out.println(": " + e.getMessage());
    e.printStackTrace();
  }

  /**
   * Takes a list of switch arguments or name-value arguments and turns it into a map.
   */
  static java.util.Map<String, String> parseArgs(String[] args) {
    java.util.Map<String, String> map = new java.util.HashMap<String, String>();
    
    for (int i = 0; i < args.length; i++) {
      // Add a key to the map for each argument.
      if (args[i].startsWith("-")) {
        // If the next argument does NOT start with a "-" then it is a value.
        if (i + 1 < args.length && !args[i + 1].startsWith("-")) {
          // Save the value and skip the next entry in the list of arguments.
          map.put(args[i].toLowerCase().replaceFirst("^-+", ""), args[i + 1]);
          i++;
        } else {
          // If the next argument starts with a "-", then we assume the current one is a switch.
          map.put(args[i].toLowerCase().replaceFirst("^-+", ""), "");
        }
      } else {
        // If the argument does not start with a "-", store the argument based on the index.
        map.put(Integer.toString(i), args[i].toLowerCase());
      }
    }
    return map;
  }
}



