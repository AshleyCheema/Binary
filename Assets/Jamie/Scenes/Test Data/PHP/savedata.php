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
    $newscore = $_POST["score"];

    //Check to see if the name already exists.
    $namecheckquery = "SELECT username FROM players WHERE username ='$username'";
    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Username check query failed"); //error code #2 = username check query failed.

    if (mysqli_num_rows($namecheck) != 1 )
    {
    	echo "5: Username does not exist"; //error code #3 = Either no username in database or there is more than one.
    	exit();
    }

    $updatequery = "UPDATE players SET score = '$newscore' WHERE username = '$username'";
    $result = mysqli_query($con, $updatequery) or die("7: Save Query Failed"); //error code #7 - UPDATE query failed

    echo "0";
    
?>