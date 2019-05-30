<?php
  
    //Includes connection details
    include "gameconfig.php";

    //check that connection happened
    if (mysqli_connect_errno())
    {
    	echo "1: Connection failed"; //error code #1 = connection failed
    	exit();
    }

    $gameDuration = $_POST["duration"];
    $gameWinner = $_POST["winner"];


	//Insert the match into the database
	$insermatchquery = "INSERT INTO matchrecord (duration, winner) VALUES ('$gameDuration', '$gameWinner')";
	$result = mysqli_query($con, $insermatchquery) or die("4: Insert Match query failed"); //error code #4 = Insert query failed.

	echo("0");



?>