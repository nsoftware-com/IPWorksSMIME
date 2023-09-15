<?php $sendBuffer = TRUE; ob_start(); ?>
<html>
<head>
<title>IPWorks S/MIME 2022 Demos - Encrypt Email</title>
<link rel="stylesheet" type="text/css" href="stylesheet.css">
<meta name="description" content="IPWorks S/MIME 2022 Demos - Encrypt Email">
</head>

<body>

<div id="content">
<h1>IPWorks S/MIME - Demo Pages</h1>
<h2>Encrypt Email</h2>
<p>Demonstrates how to use the SSMTP object to send encrypted emails.</p>
<a href="default.php">[Other Demos]</a>
<hr/>

<?php
require_once('../include/ipworkssmime_ssmtp.php');
require_once('../include/ipworkssmime_certmgr.php');
require_once('../include/ipworkssmime_const.php');

?>

<?php
$smtp = new IPWorksSMIME_SSMTP();
$certmgr = new IPWorksSMIME_CertMgr();
if ($_SERVER['REQUEST_METHOD'] == "POST") {

  $smtp->setMailServer($_POST["mailserver"]);
  $smtp->setFrom($_POST["from"]);
  $smtp->setSendTo($_POST["sendto"]);
  $smtp->setSubject($_POST["subject"]);
  $smtp->setMessageText($_POST["message"]);

  try{
  $smtp->doAddRecipientCert($_POST["cert"]);
  $smtp->doEncrypt();
  $smtp->doSend();
  echo "<h2>Message Sent!</h2>";
  } catch (Exception $e) {
    echo 'Error: ',  $e->getMessage(), "\n";
  }

  $certmgr->setCertEncoded($_POST["cert"]);
  echo "<p><b>Certificate Information:</b><P>";
  echo "<table>";
  echo "<tr><td><i>Issuer:              <td>" . $certmgr->getCertIssuer();
  echo "<tr><td><i>Subject:             <td>" . $certmgr->getCertSubject();
  echo "<tr><td><i>Version:             <td>" . $certmgr->getCertVersion();
  echo "<tr><td><i>SerialNumber:        <td>" . $certmgr->getCertSerialNumber();
  echo "<tr><td><i>SignatureAlgorithm:  <td>" . $certmgr->getCertSignatureAlgorithm();
  echo "<tr><td><i>EffectiveDate:       <td>" . $certmgr->getCertEffectiveDate();
  echo "<tr><td><i>ExpirationDate:      <td>" . $certmgr->getCertExpirationDate();
  echo "<tr><td><i>PublicKeyAlgorithm:  <td>" . $certmgr->getCertPublicKeyAlgorithm();
  echo "<tr><td><i>PublicKeyLength:     <td>" . $certmgr->getCertPublicKeyLength();
  echo "</table><hr>";
  }
?>

<form method=POST>
<table width="90%">

 <tr><td>Mail Server: <td><input type=text name=mailserver value="mail"  size=50>
 <tr><td>Sender:      <td><input type=text name=from value="me@myaddress.com" size=50>
 <tr><td>Recipient:   <td><input type=text name=sendto value="myfriend@server.com" size=50>
 <tr><td>Subject:     <td><input type=text name=subject value="Subject" size=50>
</table>

<table width="90%">
 <tr><td><td><textarea name=message cols=55 rows=5>
This message was encrypted and sent with the IP*Works S/MIME PHP Edition SSMTP component!

 </textarea>
<br>
<b>In order to encrypt this message, you'll need to do so with the recipients<br>
public key which is specified through a base64 encoded certificate.  To do this, <br>
you can export the certificate to a base64 encoded CER file and paste it below.</b><p>
<tr><td>Recipient Cert:    </td><td>
<textarea  name=cert cols=55 rows=5 wrap="Off">
MIID3zCCA4mgAwIBAgIKCIegsQAAAAAAAjANBgkqhkiG9w0BAQUFADB6MRwwGgYJ
KoZIhvcNAQkBFg10ZXN0QHRlc3QuY29tMQswCQYDVQQGEwJVUzEQMA4GA1UECBMH
Tm9TdGF0ZTEPMA0GA1UEBxMGTm9DaXR5MRswGQYDVQQKExJCb2d1cyBPcmdhbml6
YXRpb24xDTALBgNVBAMTBFRlc3QwHhcNMDExMTA4MTczNjQxWhcNMDIxMTA4MTc0
NjQxWjCBhjEcMBoGCSqGSIb3DQEJARYNdGVzdEB0ZXN0LmNvbTELMAkGA1UEBhMC
VVMxEDAOBgNVBAgTB05vU3RhdGUxDzANBgNVBAcTBk5vQ2l0eTEbMBkGA1UEChMS
Qm9ndXMgT3JnYW5pemF0aW9uMRkwFwYDVQQDExBUZXN0IENlcnRpZmljYXRlMFww
DQYJKoZIhvcNAQEBBQADSwAwSAJBAL1chdQsYPfWfa6iP5Qkr/CKJTCtPO7XWMDk
SIEneDPeXti3x6pGJT1zC8ConJL0gFQNBcfKPppnNRxwcWIlgF0CAwEAAaOCAeIw
ggHeMA4GA1UdDwEB/wQEAwIE8DATBgNVHSUEDDAKBggrBgEFBQcDATAdBgNVHQ4E
FgQUfVBbOGN/uDvG8GWFSimWdkkgYRowgbMGA1UdIwSBqzCBqIAU77UFWFB9j7Dj
+0JByWbNtugOBA2hfqR8MHoxHDAaBgkqhkiG9w0BCQEWDXRlc3RAdGVzdC5jb20x
CzAJBgNVBAYTAlVTMRAwDgYDVQQIEwdOb1N0YXRlMQ8wDQYDVQQHEwZOb0NpdHkx
GzAZBgNVBAoTEkJvZ3VzIE9yZ2FuaXphdGlvbjENMAsGA1UEAxMEVGVzdIIQeuqx
zMcpMYRHfgdm50+/MzBfBgNVHR8EWDBWMCigJqAkhiJodHRwOi8vdGVzdGJveS9D
ZXJ0RW5yb2xsL1Rlc3QuY3JsMCqgKKAmhiRmaWxlOi8vXFx0ZXN0Ym95XENlcnRF
bnJvbGxcVGVzdC5jcmwwgYAGCCsGAQUFBwEBBHQwcjA2BggrBgEFBQcwAoYqaHR0
cDovL3Rlc3Rib3kvQ2VydEVucm9sbC90ZXN0Ym95X1Rlc3QuY3J0MDgGCCsGAQUF
BzAChixmaWxlOi8vXFx0ZXN0Ym95XENlcnRFbnJvbGxcdGVzdGJveV9UZXN0LmNy
dDANBgkqhkiG9w0BAQUFAANBAJR1kFUg6LM5bRvL1n6Y3W5LIK4vBAMu58FaJxY7
9enJtOCPH1/UyL8nwVrG1pxGFhsyOlAOFbfoJKLIOrmSlSs=
</textarea>
</td></tr>
<tr><td><td><br><input type=submit value="Send Message!">
</table>
</form>


<br/>
<br/>
<br/>
<hr/>
NOTE: These pages are simple demos, and by no means complete applications.  They
are intended to illustrate the usage of the IPWorks S/MIME objects in a simple,
straightforward way.  What we are hoping to demonstrate is how simple it is to
program with our components.  If you want to know more about them, or if you have
questions, please visit <a href="http://www.nsoftware.com/?demopg-IMPHA" target="_blank">www.nsoftware.com</a> or
contact our technical <a href="http://www.nsoftware.com/support/">support</a>.
<br/>
<br/>
Copyright (c) 2023 /n software inc.
<br/>
<br/>
</div>

<div id="footer">
<center>
IPWorks S/MIME 2022 - Copyright (c) 2023 /n software inc. - For more information, please visit our website at <a href="http://www.nsoftware.com/?demopg-IMPHA" target="_blank">www.nsoftware.com</a>.
</center>
</div>

</body>
</html>

<?php if ($sendBuffer) ob_end_flush(); else ob_end_clean(); ?>
