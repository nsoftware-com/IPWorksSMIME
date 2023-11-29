/*
 * IPWorks S/MIME 2022 .NET Edition - Sample Project
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
 * 
 */

using System.Collections.Generic;
﻿using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using nsoftware.async.IPWorksSMIME;

class shtmlmailerDemo
{
    private static Shtmlmailer shtmlmailer;

    static void shtmlmailer_OnSSLServerAuthentication(object sender, ShtmlmailerSSLServerAuthenticationEventArgs e)
    {
        if (e.Accept) return;
        Console.Write("Server provided the following certificate:\nIssuer: " + e.CertIssuer + "\nSubject: " + e.CertSubject + "\n");
        Console.Write("The following problems have been determined for this certificate: " + e.Status + "\n");
        Console.Write("Would you like to continue anyways? [y/n] ");
        if (Console.Read() == 'y') e.Accept = true;
    }

    static async Task Main(string[] args)
    {
        shtmlmailer = new Shtmlmailer();
        bool sign = false;
        bool encrypt = false;

        if (args.Length < 3)
        {
            Console.WriteLine("Usage: shtmlmailer server from to [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  server  the name or address of a mail server (mail relay)");
            Console.WriteLine("  from    the email address of the sender");
            Console.WriteLine("  to      a comma separated list of addresses for destinations");
            Console.WriteLine("  -s      the subject of the mail message");
            Console.WriteLine("  -m      the HTML version of the message content");
            Console.WriteLine("  -a      the path of file to attach to the message");
            Console.WriteLine("  -pub    the optional path to the recipient's public key file for encryption");
            Console.WriteLine("  -priv   the optional path to the private key file for signing");
            Console.WriteLine("\r\nExample: shtmlmailer mail.local sender@mail.com recipient@mail.local -s \"Subject text\" -m \"<b>Hello</b>, my name is <i>Tom</i>\" -a FileToAttach -pub publickey.cer -priv privatekey.pfx privateKeyPass");
            Console.WriteLine("Note: If using the provided test private key certificate (test.pfx), set the password to \"password\"");
        }
        else
        {
            try
            {
                shtmlmailer.OnSSLServerAuthentication += shtmlmailer_OnSSLServerAuthentication;

                for (int i = 0; i < args.Length; i++)
                {
                    shtmlmailer.MailServer = args[0];
                    shtmlmailer.From = args[1];
                    shtmlmailer.SendTo = args[2];

                    if (args[i].StartsWith("-")) // args[i + 1] = actual argument value
                    {
                        if (args[i].Equals("-s")) shtmlmailer.Subject = args[i + 1];
                        if (args[i].Equals("-m")) shtmlmailer.MessageHTML = args[i + 1];
                        if (args[i].Equals("-a")) await shtmlmailer.AddAttachment(args[i + 1]);
                        if (args[i].Equals("-pub"))
                        {
                            Certificate recipientCert = new Certificate(args[i + 1]);
                            await shtmlmailer.AddRecipientCert(recipientCert.EncodedB);
                            encrypt = true;
                        }
                        if (args[i].Equals("-priv"))
                        {
                            shtmlmailer.Certificate = new Certificate(
                                CertStoreTypes.cstAuto,
                                args[i + 1], // path
                                args[i + 2], // password
                                "*"
                                );
                            sign = true;
                        }
                    }
                }

                // must call before encrypting & signing
                await shtmlmailer.CalcMessageText();

                if (encrypt && !sign)
                {
                    await shtmlmailer.Encrypt();
                }
                else if (sign && !encrypt)
                {
                    await shtmlmailer.Sign();
                }
                else if (sign && encrypt)
                {
                    await shtmlmailer.SignAndEncrypt();
                }

                Console.WriteLine("Sending message ...");

                await shtmlmailer.Send();

                Console.WriteLine("Message sent successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}


class ConsoleDemo
{
  public static Dictionary<string, string> ParseArgs(string[] args)
  {
    Dictionary<string, string> dict = new Dictionary<string, string>();

    for (int i = 0; i < args.Length; i++)
    {
      // If it starts with a "/" check the next argument.
      // If the next argument does NOT start with a "/" then this is paired, and the next argument is the value.
      // Otherwise, the next argument starts with a "/" and the current argument is a switch.

      // If it doesn't start with a "/" then it's not paired and we assume it's a standalone argument.

      if (args[i].StartsWith("/"))
      {
        // Either a paired argument or a switch.
        if (i + 1 < args.Length && !args[i + 1].StartsWith("/"))
        {
          // Paired argument.
          dict.Add(args[i].TrimStart('/'), args[i + 1]);
          // Skip the value in the next iteration.
          i++;
        }
        else
        {
          // Switch, no value.
          dict.Add(args[i].TrimStart('/'), "");
        }
      }
      else
      {
        // Standalone argument. The argument is the value, use the index as a key.
        dict.Add(i.ToString(), args[i]);
      }
    }
    return dict;
  }

  public static string Prompt(string prompt, string defaultVal)
  {
    Console.Write(prompt + (defaultVal.Length > 0 ? " [" + defaultVal + "]": "") + ": ");
    string val = Console.ReadLine();
    if (val.Length == 0) val = defaultVal;
    return val;
  }
}