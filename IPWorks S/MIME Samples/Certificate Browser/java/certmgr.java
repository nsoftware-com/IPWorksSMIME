/*
 * IPWorks S/MIME 2022 Java Edition - Sample Project
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
  Certmgr certmgr1;
  int i = 0;
  public certmgr() {
    try {
      certmgr1 = new Certmgr();
      certmgr1.addCertmgrEventListener(new CertmgrEvents(this));
      certmgr1.setCertStoreType(Certificate.cstPFXFile);
      certmgr1.setCertStore(prompt("Please enter key store path",":","test.pfx"));
      certmgr1.setCertStorePassword(prompt("Please enter store password",":","test"));
      certmgr1.listStoreCertificates();
    }
    catch (IPWorksSMIMEException ex) {
      System.out.println("IPWorks exception thrown: " + ex.getCode() + " [" +
          ex.getMessage() + "].");
    }
    catch (Exception ex) {
      System.out.println(ex.getMessage());
    }
  }

  public static void main(String[] args) {
    new certmgr();
  }

  public void certList(CertmgrCertListEvent args) {
    i++;
    System.out.println(i + ". " + args.certSubject);
  }
}

class CertmgrEvents
    implements CertmgrEventListener{
  certmgr instance;
  public CertmgrEvents(certmgr instance) {
    this.instance = instance;
  }

  public void certChain(CertmgrCertChainEvent args) {

  }

  public void certList(CertmgrCertListEvent args) {
    instance.certList(args);
  }

  public void error(CertmgrErrorEvent args) {

  }

  public void keyList(CertmgrKeyListEvent args) {

  }

  public void storeList(CertmgrStoreListEvent args) {

  }
  
  public void log(CertmgrLogEvent args){}
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

  static String prompt(String label, String punctuation, String defaultVal)
  {
	System.out.print(label + " [" + defaultVal + "] " + punctuation + " ");
	String response = input();
	if(response.equals(""))
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
}



