<?php
  
    //Includes connection details
    include "gameconfig.php";

    //check that connection happened
    if (mysqli_connect_errno())
    {
    	echo "1: Connection failed"; //error code #1 = connection failed
    	exit();
    }

    $name = $_POST["name"];

    //Check to see if the name already exists.
    $namecheckquery = "SELECT name FROM playerstats WHERE name = '$name'";
    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Player check query failed"); //error code #2 = username check query failed.

    if (mysqli_num_rows($namecheck) > 0 )
    {
    	echo "3: Player already exists"; //error code #3 = User already exists in database
    	exit();
    }

	//Encrypt the player information using SHA256 encryption
	$salt = "\$5\$rounds=5000\$" . "retrogeckos" . $username . "\$";
	$hash = crypt($password, $salt);

	//Insert the player into the database
	//$insertuserquery = "INSERT INTO players (username, hash, salt) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "');";
	$insertuserquery = "INSERT INTO playerstats (name) VALUES ('$name')";
	$result = mysqli_query($con, $insertuserquery) or die("4: Insert player query failed"); //error code #4 = Insert query failed.

	echo("0");



?>