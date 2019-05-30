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
    $result = $_POST["result"];
    $class = $_POST["class"];
    $steps = $_POST["steps"];
    $shots = $_POST["shots"];
    $abilities = $_POST["abilities"];
    $points = $_POST["points"];

    $win = 0;
    $lose = 0;
    $playedSpy = 0;
    $playedMerc = 0;
    $winSpy = 0;
    $winMerc = 0;
    $loseSpy = 0;
    $loseMerc = 0;

    // Method to calculate which collumns need to be updated in the DB
    if ($result == "win"){
      $win = 1;

      if ($class == "spy"){

        $playedSpy = 1;
        $winSpy = 1;

      } else {
        $playedMerc = 1;
        $winMerc = 1;
      }

    } else {
      $lose = 1;
       
      if ($class == "spy"){
        $playedSpy = 1;
        $loseSpy = 1;

      } else {
        $playedMerc = 1;
        $loseMerc = 1;
      }
    }

    //Check to see if the name already exists.
    $namecheckquery = "SELECT name FROM playerstats WHERE name ='$name'";
    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Username check query failed"); //error code #2 = username check query failed.

    if (mysqli_num_rows($namecheck) != 1 )
    {
      $insertuserquery = "INSERT INTO playerstats (name) VALUES ('$name')";
      $result = mysqli_query($con, $insertuserquery) or die("7: Insert player query failed"); //error code #7 = User doesn't exist and Insert query failed.
    }

    $updateplayer = "UPDATE playerstats SET gamesPlayed=gamesPlayed+1, latestResult='$result', wins=wins+'$win', loses=loses+'$lose', playedSpy=playedSpy+'$playedSpy', playedMerc=playedMerc+'$playedMerc', winSpy=winSpy+'$winSpy', loseSpy=loseSpy+'$loseSpy', winMerc=winMerc+'$winMerc', loseMerc=loseMerc+'$loseMerc', stepsTaken=stepsTaken+'$steps', shotsFired=shotsFired+'$shots', abilitiesUsed=abilitiesUsed+'$abilities', pointsCaptured=pointsCaptured+'$points' WHERE name = '$name'";
    $result = mysqli_query($con, $updateplayer) or die("8: Save Query Failed"); //error code #8 - UPDATE query failed

    echo "0";
    
?>