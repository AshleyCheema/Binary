<?php

	session_start();
	
	//Destroying all sessions
	if( session_destroy() )
	{
		
		//Redirecting to home page
		header( "Location: index.php" ); 
		
	}
	
?>