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
﻿using System;
using System.Threading.Tasks;
using nsoftware.async.IPWorksSMIME;

class smimeDemo
{
  private static Smime smime = new nsoftware.async.IPWorksSMIME.Smime();
  private const string PRIVATE_CERT_PATH = "..\\..\\..\\testcert.pfx";
  private const string PUBLIC_CERT_PATH = "..\\..\\..\\testcert.cer";

  static async Task Main(string[] args)
  {
    try
    {
      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("smime> ");
      string command;
      string[] arguments;

      while (true)
      {
        command = Console.ReadLine();
        arguments = command.Split();

        if (arguments[0] == "?" || arguments[0] == "help")
        {
          Console.WriteLine("Commands: ");
          Console.WriteLine("  ?                        display the list of valid commands");
          Console.WriteLine("  help                     display the list of valid commands");
          Console.WriteLine("  sign <message>           sign a message");
          Console.WriteLine("  encrypt <message>        encrypt a message");
          Console.WriteLine("  signencrypt <message>    sign and encrypt a message");
          Console.WriteLine("  decrypt <message>        decrypted an encrypted message");
          Console.WriteLine("  verify <message>         verify a signed message");
          Console.WriteLine("  decryptverify <message>  decrypt and verify a signed and encrypted message");
          Console.WriteLine("  quit                     exit the application");
        }
        else if (arguments[0] == "sign")
        {
          smime.InputMessage = arguments[1];
          smime.Certificate = new Certificate(CertStoreTypes.cstPFXFile, PRIVATE_CERT_PATH, "password", "*");
          smime.DetachedSignature = true;
          smime.IncludeCertificate = true;

          await smime.Sign();

          Console.WriteLine("Encoded message:\r\n" + smime.OutputMessageHeadersString + "\r\n" + smime.OutputMessage);
        }
        else if (arguments[0] == "encrypt")
        {
          smime.InputMessage = arguments[1];
          Certificate encryptionCert = new Certificate(CertStoreTypes.cstPublicKeyFile, PUBLIC_CERT_PATH, "", "*");
          await smime.AddRecipientCert(encryptionCert.EncodedB);

          await smime.Encrypt();

          Console.WriteLine("Encoded message:\r\n" + smime.OutputMessageHeadersString + "\r\n" + smime.OutputMessage);
        }
        else if (arguments[0] == "signencrypt")
        {
          smime.InputMessage = arguments[1];
          smime.Certificate = new Certificate(CertStoreTypes.cstPFXFile, PRIVATE_CERT_PATH, "password", "*");
          Certificate encryptionCert = new Certificate(CertStoreTypes.cstPublicKeyFile, PUBLIC_CERT_PATH, "", "*");
          await smime.AddRecipientCert(encryptionCert.EncodedB);

          await smime.SignAndEncrypt();

          Console.WriteLine("Encoded message:\r\n" + smime.OutputMessageHeadersString + "\r\n" + smime.OutputMessage);
        }
        else if (arguments[0] == "decrypt")
        {
          smime.InputMessage = arguments[1];
          smime.Certificate = new Certificate(CertStoreTypes.cstPFXFile, PRIVATE_CERT_PATH, "password", "*");

          await smime.Decrypt();

          Console.WriteLine("Encoded message:\r\n" + smime.OutputMessageHeadersString + "\r\n" + smime.OutputMessage);
        }
        else if (arguments[0] == "verify")
        {
          smime.InputMessage = arguments[1];

          // For the purpose of this demo, we assume the public key was included in the signed message.
          await smime.VerifySignature();

          Console.WriteLine("Encoded message:\r\n" + smime.OutputMessageHeadersString + "\r\n" + smime.OutputMessage);
        }
        else if (arguments[0] == "decryptverify")
        {
          smime.InputMessage = arguments[1];
          smime.Certificate = new Certificate(CertStoreTypes.cstPFXFile, PRIVATE_CERT_PATH, "password", "*");

          // For the purpose of this demo, we assume the public key was included in the signed message.
          await smime.DecryptAndVerifySignature();

          Console.WriteLine("Encoded message:\r\n" + smime.OutputMessageHeadersString + "\r\n" + smime.OutputMessage);
        }
        else if (arguments[0] == "quit")
        {
          break;
        }
        else if (arguments[0] == "")
        {
          // Do nothing.
        }
        else
        {
          Console.WriteLine("Invalid command.");
        } // End of command checking.

        Console.Write("smime> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
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