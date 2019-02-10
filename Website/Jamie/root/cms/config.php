<?php
    
    //Database details saved as variables
    $dbserver = "localhost";
    
    $dbusername = "retrogeck_admin";
        
    $dbpassword = "FQ4mq*EsMP_*";

    $database = "retrogec_website_db";

	//Establishing Connection with Server by passing server_name, user_id and password as a parameter 
	$connection = mysqli_connect( $dbserver, $dbusername, $dbpassword, $database );
	

?>