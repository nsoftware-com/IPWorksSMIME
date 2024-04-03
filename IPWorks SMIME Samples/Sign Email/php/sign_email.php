<?php $sendBuffer = TRUE; ob_start(); ?>
<html>
<head>
<title>IPWorks S/MIME 2022 Demos - Sign Email</title>
<link rel="stylesheet" type="text/css" href="stylesheet.css">
<meta name="description" content="IPWorks S/MIME 2022 Demos - Sign Email">
</head>

<body>

<div id="content">
<h1>IPWorks S/MIME - Demo Pages</h1>
<h2>Sign Email</h2>
<p>Demonstrates how to use the SSMTP object to send signed emails.</p>
<a href="default.php">[Other Demos]</a>
<hr/>

<?php
require_once('../include/ipworkssmime_ssmtp.php');
require_once('../include/ipworkssmime_const.php');

?>

<?php
$smtp = new IPWorksSMIME_SSMTP();

if ($_SERVER['REQUEST_METHOD'] == "POST") {

  $smtp->setMailServer($_POST["mailserver"]);
  $smtp->setFrom($_POST["from"]);
  $smtp->setSendTo($_POST["sendto"]);
  $smtp->setSubject($_POST["subject"]);
  $smtp->setMessageText($_POST["message"]);

  $smtp->setCertStoreType(2); //PFXFile
  $smtp->setCertStore($_POST["pfxfile"]);
  $smtp->setCertStorePassword($_POST["pfxpass"]);
  $smtp->setCertSubject("*");

  $smtp->setIncludeCertificate(true);

  try{
    $smtp->doSign();
    $smtp->doSend();
  echo "<h2>Message Sent!</h2>";
  } catch (Exception $e) {
    echo 'Error: ',  $e->getMessage(), "\n";
  }
}
?>

<form method=POST>
<table width="90%">

 <tr><td>Mail Server: <td><input type=text name=mailserver value="mail"  size=50>
 <tr><td>Sender:      <td><input type=text name=from value="me@myaddress.com" size=50>
 <tr><td>Recipient:   <td><input type=text name=sendto value="myfriend@server.com" size=50>
 <tr><td>Subject:     <td><input type=text name=subject value="Subject" size=50>
 <tr><td>PFX File:    <td><input type=text name=pfxfile value="Full Path to PFX File" size=50>
 <tr><td>PFX Password:<td><input type=password name=pfxpass value="" size=50>
</table>

<br>
<b>In order to sign this message, you'll need to do so with a digital certificate (PFX).<br></b>
<p>
<table width="90%">
 <tr><td><td><textarea name=message cols=55 rows=15>

This message was signed and sent with the IP*Works S/MIME PHP Edition SSMTP component!

 </textarea>

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
Copyright (c) 2024 /n software inc.
<br/>
<br/>
</div>

<div id="footer">
<center>
IPWorks S/MIME 2022 - Copyright (c) 2024 /n software inc. - For more information, please visit our website at <a href="http://www.nsoftware.com/?demopg-IMPHA" target="_blank">www.nsoftware.com</a>.
</center>
</div>

</body>
</html>

<?php if ($sendBuffer) ob_end_flush(); else ob_end_clean(); ?>
