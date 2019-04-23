<?php

    //Connection details: (host, user, password, database-name)
    // $con = mysqli_connect('localhost', 'root', 'root', 'unityaccess');
  
    //Includes connection details
    include "gameconfig.php";

    //check that connection happened

    if (mysqli_connect_errno())
    {
    	echo "1: Connection failed"; //error code #1 = connection failed
    	exit();
    }

    $username = $_POST["name"];
    $password = $_POST["password"];

    //Check to see if the name already exists.
    $namecheckquery = "SELECT username FROM players WHERE username = '$username' ";
    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Username check query failed"); //error code #2 = username check query failed.

    if (mysqli_num_rows($namecheck) > 0 )
    {
    	echo "3: Username already exists"; //error code #3 = Username already exists cannot register
    	exit();
    }

	//Encrypt the player information using SHA256 encryption
	$salt = "\$5\$rounds=5000\$" . "retrogeckos" . $username . "\$";
	$hash = crypt($password, $salt);

	//Insert the player into the database
	//$insertuserquery = "INSERT INTO players (username, hash, salt) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "');";
	$insertuserquery = "INSERT INTO players (`username`, `hash`, `salt`) VALUES ('$username', '$hash', '$salt');";
	$result = mysqli_query($con, $insertuserquery) or die("4: Insert player query failed"); //error code #4 = Insert query failed.

	echo("0");



?>