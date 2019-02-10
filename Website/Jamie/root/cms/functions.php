<? php 

    /**
     * Send debug code to the Javascript console
     * https://paulund.co.uk/output-php-data-in-browser-console
     */ 
    function startDebug( $data ) {

        if( is_array( $data ) || is_object( $data ) )
        {

            echo( "<script>console.log('PHP: " . json_encode( $data ) . "');</script>" );

        } else {

            echo( "<script>console.log('PHP: " . $data . "');</script>" );

        }

    }

    //Initialise Login Procedure.
    function startLogin () 
    {
            //starting session
            session_start();

            //variable to store error message
            $error=''; 
            //If the post is submitted do the following
            if ( isset( $_POST[ 'submit' ] ) ) 
            {
                // if either input it empty do the following
                if ( empty( $_POST[ 'username' ] ) || empty( $_POST[ 'password' ] ) ) 
                {
                    //set error variable
                    $_SESSION['Error'] = "Username or Password is invalid";

                //if the name and password is set then
                } else {   
						  
                    // Define $username and $password 
                    $username = $_POST[ 'username' ]; 
                    $password = $_POST[ 'password' ];

                    // To protect MySQL injection for Security purpose 
                    $username = stripslashes( $username );
                    $password = stripslashes( $password );

                    include "config.php";

                    //added security for PHP
                    $username = mysqli_escape_string( $connection, $username );
                    $password = mysqli_escape_string( $connection, $password );

                    //Password Encryption
                    $password = md5 ( $password );

                    //SQL query to fetch information of registerd users and finds user match.
                    $query = mysqli_query( $connection, "SELECT * FROM users WHERE password='$password' AND username='$username'" );

                    //fetches data
                    $rows = mysqli_num_rows( $query );

                    if ( $rows == 1 ) {

                        //Initializing Session
                        $_SESSION[ 'username' ] = $username;

                        //Redirecting to other page
                        header( "location: commands.php" );

                    } else {

                        //set error message
                      
                        $_SESSION['Error'] = "Username or Password is invalid"; 

                    }
				
				    //Closing Connection
				    mysqli_close( $connection );  
										 
			     }

            }
        }

            
    //initialise log in checks
    function checkLogin( $isLogin = false ) 
    {	
        
        //Starting Session		                
		session_start();
		
		//Storing session
		$username = $_SESSION[ 'username' ];        
        
		include "config.php";
		
		//SQLI query to fetch complete information of user   
		$query = mysqli_query( $connection, "SELECT * FROM users WHERE username='$username'" );

		//perform query
		$row = mysqli_fetch_assoc( $query );
        
		//Store username into a variable
        $userid = $row['user_id'];
		$login_session = $row[ 'username' ];
        $firstname_session = $row['firstname'];
        $lastname_session = $row['lastname'];
        $email_session = $row['email'];
        $creation_date = $row['creationdate'];
        
        //Store Session variables.
        $_SESSION[ 'username' ] = $login_session;
        
       // the following is then used to direct the user to the correct location
        if( $isLogin ){
            //if the login information is set then:
            if( isset( $login_session ) )
            {
                //Closing Connection
                mysqli_close( $connection );

                //Redirecting to CMS dashboard 
                header( 'Location: commands.php' );
            }
            
        } else {
            //if the login information is not set then:
            if( !isset( $login_session ) )
            {
                //Closing Connection
                mysqli_close( $connection );

                //Redirecting to log in page
                header( 'Location: index.php' ); 
            
            }
		
	   }
        
    }

    ?>
