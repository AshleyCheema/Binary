<?php

	//Connection details: (host, user, password, database-name)
	$con = mysqli_connect('localhost', 'root', 'root', 'unityaccess');

  //check that connection happened

    if (mysqli_connect_errno())
    {
    	echo "1: Connection failed"; //error code #1 = connection failed
    	exit();
    }

    $username = $_POST["name"];
    $password = $_POST["password"];

    //Check to see if the name already exists.
    $namecheckquery = "SELECT username, salt, hash, score from players WHERE username ='$username'";
    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Username check query failed"); //error code #2 = username check query failed.

    if (mysqli_num_rows($namecheck) != 1 )
    {
    	echo "5: Username does not exist"; //error code #3 = Either no username in database or there is more than one.
    	exit();
    }

    //get login info from query
    $existinginfo = mysqli_fetch_assoc($namecheck);
    $salt = $existinginfo["salt"];
    $hash = $existinginfo["hash"];
  

    $loginhash = crypt($password, $salt);
    if($hash != $loginhash)
    {
    	echo "6: Incorrect Password"; //error code #6 password does not has to match table
    	exit();
    }

    echo "0\t" . $existinginfo["score"];
?>