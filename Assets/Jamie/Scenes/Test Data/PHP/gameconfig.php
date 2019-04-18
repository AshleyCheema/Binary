<?php
    
    //Database details saved as variables
    $dbserver = "localhost";
    
    $dbusername = "retrogec_binary";
        
    $dbpassword = "Uni#Y%AM*A^$";

    $database = "retrogec_game_db";

	//Establishing Connection with Server by passing server_name, user_id and password as a parameter 
	$con = mysqli_connect( $dbserver, $dbusername, $dbpassword, $database );
	

?>