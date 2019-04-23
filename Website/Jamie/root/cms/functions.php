<?php 

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
                if ( empty( $_POST[ 'email' ] ) || empty( $_POST[ 'password' ] ) ) 
                {
                    //set error variable
                    $_SESSION['Error'] = "Email or Password is invalid";

                //if the name and password is set then
                } else {   
						  
                    // Define $username and $password 
                    $email = $_POST[ 'email' ]; 
                    $password = $_POST[ 'password' ];

                    // To protect MySQL injection for Security purpose 
                    $email = stripslashes( $email );
                    $password = stripslashes( $password );

                    include "config.php";

                    //added security for PHP
                    $email = mysqli_escape_string( $connection, $email );
                    $password = mysqli_escape_string( $connection, $password );

                    //Password Encryption
                    $password = md5 ( $password );

                    //SQL query to fetch information of registerd users and finds user match.
                    $query = mysqli_query( $connection, "SELECT * FROM users WHERE user_password='$password' AND user_email='$email'" );

                    //fetches data
                    $rows = mysqli_num_rows( $query );
                   

                    if ( $rows == 1 ) {

                        $row = mysqli_fetch_assoc( $query );
                        //Initializing Session
                        $_SESSION[ 'id' ] = $row[ 'user_id' ];

                        //Redirecting to other page
                        header( "location: dashboard.php" );

                    } else {

                        //set error message
                      
                        $_SESSION['Error'] = "Email or Password is invalid"; 

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
		$id = $_SESSION[ 'id' ];        
        
		include "config.php";
		
		//SQLI query to fetch complete information of user   
		$query = mysqli_query( $connection, "SELECT * FROM users WHERE user_id='$id'" );

		//perform query
		$row = mysqli_fetch_assoc( $query );
        
		//Store username into a variable
        $user_id = $row[ 'user_id' ];
        $user_pic = $row[ 'user_pic' ];
		$login_session = $row[ 'user_email' ];
        $name_session = $row[ 'user_name' ];
        $pw = $row[ 'user_password' ];
        $right = $row[ 'user_right' ];
        $creation_date = $row[ 'user_creation' ];
        
        //Store Session variables.
        $_SESSION[ 'id' ] = $user_id;
        $_SESSION[ 'login_email' ] = $login_session;
        $_SESSION[ 'name' ] = $name_session;
        $_SESSION[ 'pic' ] = $user_pic;

        if($right == 0){
            $_SESSION[ 'rank' ] = 'Unverified';
        } elseif($right == 1) {
            $_SESSION[ 'rank' ] = 'Guest';
        } elseif($right == 2) {
            $_SESSION[ 'rank' ] = 'Admin';
        }
        
       // the following is then used to direct the user to the correct location
        if( $isLogin ){
            //if the login information is set then:
            if( isset( $user_id ) )
            {
                //Closing Connection
                mysqli_close( $connection );

                //Redirecting to CMS dashboard 
                header( 'Location: dashboard.php' );
            }
            
        } else {
            //if the login information is not set then:
            if( !isset( $user_id ) )
            {
                //Closing Connection
                mysqli_close( $connection );

                //Redirecting to log in page
                header( 'Location: index.php' ); 
            
            }
		
	   }
        
    }

    function updateProfile(){

        //If the post is submitted do the following
        if ( isset( $_POST[ 'update-information' ] ) ) 
        {
            include "config.php";

            //Save existing id in a variable
            $id = $_SESSION[ 'id' ];

            //retrieve new variables
            $newName = $_POST[ 'name' ];
            $newEmail= $_POST[ 'email' ];
            $newPassword = $_POST[ 'password' ];
            $newRank = $_POST[ 'rank' ];

            $query = mysqli_query( $connection, "SELECT * FROM users WHERE user_id ='$id' ");
            $row = mysqli_fetch_assoc($query);

            $existName = $row[ 'user_name' ];
            $existEmail = $row[ 'user_email' ];
            $existPassword = $row[ 'user_password' ];
            $existRight = $row[ 'user_right' ];

            //Update user's name
            if(($newName != $existName)&&(!empty($newName))) {
                $updateUser = mysqli_query( $connection, "UPDATE users SET user_name = '$newName' WHERE user_id = '$id' " );
                
            }

            //Update user's email address
            if(($newEmail != $existEmail)&&(!empty($newEmail))) {
                $updateEmail = mysqli_query( $connection, "UPDATE users SET user_email = '$newEmail' WHERE user_id = '$id' " );
                
            }

            //Update user's password
            if(($newPassword != $existPassword)&&($newPassword != '****')&&(!empty($newPassword))) {
                $mdPw = md5 ($newPassword);
                $updatePW = mysqli_query( $connection, "UPDATE users SET user_password = '$mdPw' WHERE user_id = '$id' " );
                
            }

            //Update user's email address
            if($newRank == "Guest"){
                $newRight = '1';
            } elseif ($newRank == "Admin") {
                $newRight = '2';
            }

            if($newRight != $existRight){
                $updateRight = mysqli_query( $connection, "UPDATE users SET user_right = '$newRight' WHERE user_id = '$id' " );
                
            }

        }

    }

    function addSlide(){

        if ( isset( $_POST[ 'add-slide' ] ) ) {

            $target_dir = "../images/slideuploads/";
            $target_file = $target_dir . basename($_FILES["fileToUpload"]["name"]);
            $uploadOk = 1;
            $imageFileType = strtolower(pathinfo($target_file,PATHINFO_EXTENSION));
            // Check if image file is a actual image or fake image
            if(isset($_POST["submit"])) {
                $check = getimagesize($_FILES["fileToUpload"]["tmp_name"]);
                if($check !== false) {
                    //echo "File is an image - " . $check["mime"] . ".";
                    $uploadOk = 1;
                } else {
                   // echo "File is not an image.";
                    $uploadOk = 0;
                }
            }
            // Check if file already exists
            if (file_exists($target_file)) {
                //echo "Sorry, file already exists.";
                $uploadOk = 0;
            }
            
            /* // Check file size
            if ($_FILES["fileToUpload"]["size"] > 500000) {
                echo "Sorry, your file is too large.";
                $uploadOk = 0;
            } */
            
            // Allow certain file formats
            if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
            && $imageFileType != "gif" ) {
                //echo "Sorry, only JPG, JPEG, PNG & GIF files are allowed.";
                $uploadOk = 0;
            }
            // Check if $uploadOk is set to 0 by an error
            if ($uploadOk == 0) {
                //echo "Sorry, your file was not uploaded.";
            // if everything is ok, try to upload file
            } else {
                if (move_uploaded_file($_FILES["fileToUpload"]["tmp_name"], $target_file)) {
                    //echo "The file ". basename( $_FILES["fileToUpload"]["name"]). " has been uploaded.";
                } else {
                    //echo "Sorry, there was an error uploading your file.";
                }
            }

            $slideImg = $target_file;
            $slideTitle = $_POST[ 'slide-title' ];
            $slideSub = $_POST[ 'slide-subtitle' ];

            //Added security to information being placed in database
            $slideTitle = stripslashes( $slideTitle );
            $slideSub = stripslashes( $slideSub );

            include "config.php";

            $result = mysqli_query( $connection, "INSERT INTO slideshow (slide_img, slide_title, slide_sub ) VALUES ('$slideImg', '$slideTitle', '$slideSub' )" );
            
            if ($connection->query($result) === TRUE) {
                //echo "New record created successfully";
            } else {
               // echo "Error: " . $result . "<br>" . $connection->error;
            }

            /* free result set */
            mysqli_free_result($result);
            mysqli_close( $connection );

        }
    }

    function addBlog(){

        if ( isset( $_POST[ 'add-blog' ] ) ) {

            $target_dir = "../images/bloguploads/";
            $target_file = $target_dir . basename($_FILES["fileToUpload"]["name"]);
            $uploadOk = 1;
            $imageFileType = strtolower(pathinfo($target_file,PATHINFO_EXTENSION));
            // Check if image file is a actual image or fake image
            if(isset($_POST["submit"])) {
                $check = getimagesize($_FILES["fileToUpload"]["tmp_name"]);
                if($check !== false) {
                    echo "File is an image - " . $check["mime"] . ".";
                    $uploadOk = 1;
                } else {
                    echo "File is not an image.";
                    $uploadOk = 0;
                }
            }
            // Check if file already exists
            if (file_exists($target_file)) {
                echo "Sorry, file already exists.";
                $uploadOk = 0;
            }
            
            /* // Check file size
            if ($_FILES["fileToUpload"]["size"] > 500000) {
                echo "Sorry, your file is too large.";
                $uploadOk = 0;
            } */
            
            // Allow certain file formats
            if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
            && $imageFileType != "gif" ) {
                echo "Sorry, only JPG, JPEG, PNG & GIF files are allowed.";
                $uploadOk = 0;
            }
            // Check if $uploadOk is set to 0 by an error
            if ($uploadOk == 0) {
                echo "Sorry, your file was not uploaded.";
            // if everything is ok, try to upload file
            } else {
                if (move_uploaded_file($_FILES["fileToUpload"]["tmp_name"], $target_file)) {
                    //echo "The file ". basename( $_FILES["fileToUpload"]["name"]). " has been uploaded.";
                } else {
                    echo "Sorry, there was an error uploading your file.";
                }
            }

            $blogTitle = $_POST[ 'blog-title' ];
            $blogContent = $_POST[ 'blog-content' ];
            
            include "config.php";
            
            //Added security to information being placed in database
            $blogTitle = mysqli_real_escape_string($connection, stripslashes( $blogTitle ));
            $blogContent = mysqli_real_escape_string($connection, stripslashes( $blogContent ));
            
            $blogImage = $target_file;
            $blogAuthor = $_SESSION[ 'name' ];

            $newsql = "INSERT INTO blog (blog_author, blog_title, blog_image, blog_content )
                VALUES ('$blogAuthor', '$blogTitle', '$blogImage', '$blogContent' )";
            $result = mysqli_query( $connection, $newsql );
            
            /* if ($connection->query($result) === TRUE) {
                //echo "New record created successfully";
            } else {
                // echo "Error: " . $result . "<br>" . $connection->error;
            } */
            
            /* free result set */
            //mysqli_free_result($result);
            mysqli_close( $connection );
        }
    }

     function showBlogCms(){
        include "config.php";

        //Query the database to find all information in the blog and to arrange it in descending order.
        $queryblogs = "SELECT * FROM blog ORDER BY blog_id DESC" ;
        
        if ($result = mysqli_query($connection, $queryblogs) or die("Bad Query: $queryblogs")) {
        
            while ($row = mysqli_fetch_assoc($result))
            {
                //make variables equal to the data from taken from the database.
                $id = $row['blog_id'];
                $image = $row['blog_image'];
                $title = $row['blog_title'];
    
                ?>

                <form enctype="multipart/form-data" action="" method="POST"> 
                    <input type="hidden" name="blog-id" value="<?php echo $id; ?>">   
                    <div class="slide-item m-4"><div style="background-image: url('<?php echo $image ?>'); background-size:cover; height:200px; width:200px;'"></div><br>
                        <p class="text-center"><?php echo $title ?></p>  
                        <div class="hover-item cms-hover">
                            <button type="submit" name="edit-blog" class="button cms-button btn">Edit</button><button type="submit" name="delete-blog" class="button cms-button btn"> Delete</button>
                        </div>
                    </div>
                </form>
                <?php
    
            }

             /* free result set */
            mysqli_free_result($result);

        }

        mysqli_close( $connection );
    }

    function deleteSlide(){

        if(isset($_POST["delete-slide"])) {

            $id = $_POST[ 'slide-id' ];
            include "config.php";

            $result = mysqli_query( $connection, "DELETE FROM slideshow WHERE slide_id = $id " );

            //Close the mysqli connection
            mysqli_close( $connection );
        }
    }

    function editSlide(){
        if(isset($_POST["edit-slide"])) {

            $id = $_POST[ 'slide-id' ];
            include "config.php";

            $result = mysqli_query( $connection, "SELECT * FROM slideshow WHERE slide_id = $id " );
            $row = mysqli_fetch_assoc($result);

            $image = $row['slide_image'];
            $title = $row['slide_title'];
            $sub = $row['slide_sub'];

            $_SESSION['this_slide_id'] = $id;
            $_SESSION['this_slide_image'] = $image;
            $_SESSION['this_slide_title'] = $title;
            $_SESSION['this_slide_sub'] = $sub;

            //Close the mysqli connection
            mysqli_close( $connection );

            header('Location: dashboard.php?page=editslide');

        }
    }

    function updateSlide(){
        if(isset($_POST["update-slide"])){

            $id = $_SESSION['this_slide_id'];
            $title = $_POST[ 'slide-title' ];
            $image = $_POST[ 'slide-image' ];
            $sub = $_POST[ 'slide-sub' ];

            include "config.php";

            $update = mysqli_query( $connection, "UPDATE slideshow SET slide_title='$title', slide_sub='$sub' WHERE slide_id = $id" );

            //Close the mysqli connection
            mysqli_close( $connection );

            header('Location: dashboard.php?page=slideshow');

        }
    }

    function deleteBlog(){
        
        if(isset($_POST["delete-blog"])) {

            $id = $_POST[ 'blog-id' ];
            include "config.php";

            $result = mysqli_query( $connection, "DELETE FROM blog WHERE blog_id = $id " );

            //Close the mysqli connection
            mysqli_close( $connection );

        }
    }

    function editBlog(){
        
        if(isset($_POST["edit-blog"])) {

            $id = $_POST[ 'blog-id' ];
            include "config.php";

            $result = mysqli_query( $connection, "SELECT * FROM blog WHERE blog_id = $id " );
            $row = mysqli_fetch_assoc($result);

            $author = $row['blog_author'];
            $title = $row['blog_title'];
            $image = $row['blog_image'];
            $content = $row['blog_content'];
            $date = $row['blog_date'];

            $_SESSION['this_id'] = $id;
            $_SESSION['this_author'] = $author;
            $_SESSION['this_title'] = $title;
            $_SESSION['this_image'] = $image;
            $_SESSION['this_date'] = $date;

            $_SESSION['this_content'] = htmlspecialchars($row['blog_content']); 

            //Close the mysqli connection
            mysqli_close( $connection );

            header('Location: dashboard.php?page=editblog');

        }
    }

    function updateBlog(){
        if(isset($_POST["update-post"])){

            $id = $_SESSION['this_id'];
            $title = $_POST[ 'blog-title' ];
            $image = $_POST[ 'blog-image' ];
            $content = $_POST[ 'blog-content' ];

            include "config.php";

            $update = mysqli_query( $connection, "UPDATE blog SET blog_title='$title', blog_content='$content' WHERE blog_id = $id" );

            //Close the mysqli connection
            mysqli_close( $connection );

            header('Location: dashboard.php?page=blog');

        }
    }

    function displaySlides(){

        include "config.php";

        //Query the database to find all information in the slideshow and to arrange it in descending order.
        $queryslide = "SELECT * FROM slideshow ORDER BY slide_id DESC" ;

        if ($result = mysqli_query($connection, $queryslide) or die("Bad Query: $queryslide")) {
        
            while ($row = mysqli_fetch_assoc($result))
            {
                //make variables equal to the data from taken from the database.
                $id = $row['slide_id'];
                $image = $row['slide_img'];
                $title = $row['slide_title'];
                $sub = $row['slide_sub'];
    
                ?>
                 <div class="slide-img" style="background-image: url('<?php echo $image?>');">
                    <div class="img-overlay">
                        <h3 class="img-title"><?php echo $title?></h3>
                        <p class="img-author"><?php echo $sub?></p>
                    </div>
                </div>
                <?php
    
            }

             /* free result set */
            mysqli_free_result($result);

         }
    }

    function showSlideCms(){
        include "config.php";

        //Query the database to find all information in the slides and to arrange it in descending order.
        $queryslides = "SELECT * FROM slideshow ORDER BY slide_id DESC" ;
        
        if ($result = mysqli_query($connection, $queryslides) or die("Bad Query: $queryslides")) {
        
            while ($row = mysqli_fetch_assoc($result))
            {
                //make variables equal to the data from taken from the database.
                $id = $row['slide_id'];
                $image = $row['slide_img'];
                $title = $row['slide_title'];
    
                ?>

                <form enctype="multipart/form-data" action="" method="POST"> 
                    <input type="hidden" name="slide-id" value="<?php echo $id; ?>">   
                    <div class="slide-item m-4"><div style="background-image: url('<?php echo $image ?>'); background-size:cover; height:200px; width:200px;'"></div><br>
                        <p class="text-center"><?php echo $title ?></p>  
                        <div class="hover-item cms-hover">
                            <button type="submit" name="edit-slide" class="button cms-button btn">Edit</button><button type="submit" name="delete-slide" class="button cms-button btn"> Delete</button>
                        </div>
                    </div>
                </form>

                <?php
    
            }

             /* free result set */
            mysqli_free_result($result);

        }

        mysqli_close( $connection );
    }

    function threeBlogs(){

        include "config.php";

        //sql to find all information in the blog and to arrange it in descending order.
        $blogsqlone = "SELECT * FROM blog ORDER BY blog_date DESC LIMIT 0,1" ;
        $blogsqltwo = "SELECT * FROM blog ORDER BY blog_date DESC LIMIT 1,1" ;
        $blogsqlthree = "SELECT * FROM blog ORDER BY blog_date DESC LIMIT 2,1" ;


        //Result of querying the database
        $result = mysqli_query($connection, $blogsqlone) or die("Bad Query: $blogsqlone");
        $row = mysqli_fetch_assoc($result);

        $firstId = $row['blog_id'];
        $firstAuthor = $row['blog_author'];
        $firstTitle = $row['blog_title'];
        $firstImg = $row['blog_image'];
        $firstCont = $row['blog_content'];
        $firstDate = $row['blog_date'];
        $dt1 = date('jS M Y', strtotime($firstDate));

            
        /* free result set */
        mysqli_free_result($result);

        //Result of querying the database
        $result = mysqli_query($connection, $blogsqltwo) or die("Bad Query: $blogsqltwo");
        $row = mysqli_fetch_assoc($result);

        $secondId = $row['blog_id'];
        $secondAuthor = $row['blog_author'];
        $secondTitle = $row['blog_title'];
        $secondImg = $row['blog_image'];
        $secondCont = $row['blog_content'];
        $secondDate = $row['blog_date'];
        $dt2 = date('jS M Y', strtotime($secondDate));


        /* free result set */
        mysqli_free_result($result);

        //Result of querying the database
        $result = mysqli_query($connection, $blogsqlthree) or die("Bad Query: $blogsqlthree");
        $row = mysqli_fetch_assoc($result);

        $thirdId = $row['blog_id'];
        $thirdAuthor = $row['blog_author'];
        $thirdTitle = $row['blog_title'];
        $thirdImg = $row['blog_image'];
        $thirdCont = $row['blog_content'];
        $thirdDate = $row['blog_date'];
        $dt3 = date('jS M Y', strtotime($thirdDate));

        /* free result set */
        mysqli_free_result($result);

        ?>

        <div class="col-12 col-lg-7 blog-post" style="background-image:url('<? echo $firstImg ?>');">
            <div class="blog-title">
                <h5 class="blog-title-text"><? echo $firstTitle ?></h5>
                <p class="blog-date"><? echo $dt1 ?></p>
                <br>
                <a class="blog-link" href="blog.php?post_id=<? echo $firstId ?>">Read More</a>
            </div>
        </div>
        <div class="col-sm-12 col-lg-5 blog-row-2">
            <div class="blog-post sm-blog-post" style="background-image:url('<? echo $secondImg ?>');">
                <div class="blog-title">
                    <h5 class="blog-title-text"><? echo $secondTitle ?></h5>
                    <p class="blog-date"><? echo $dt2 ?></p>
                    <a class="blog-link" href="blog.php?post_id=<? echo $secondId ?>">Read More</a>
                </div>
            </div>
            <div class="blog-post sm-blog-post" style="background-image:url('<? echo $thirdImg ?>');">
                <div class="blog-title">
                    <h5 class="blog-title-text"><? echo $thirdTitle ?></h5>
                    <p class="blog-date"><? echo $dt3 ?></p>
                    <a class="blog-link" href="blog.php?post_id=<? echo $thirdId ?>">Read More</a>
                </div>
            </div>
        <?php
    }

    function allBlogs(){
        include "config.php";

        //sql to find all information in the blog and to arrange it in descending order.
        $blogsql = "SELECT * FROM blog ORDER BY blog_date DESC" ;
        
        //Result of querying the database
        $result = mysqli_query($connection, $blogsql) or die("Bad Query: $blogsql");
        while ($row = mysqli_fetch_assoc($result)){

            $rowdate = $row['blog_date'];
            $dt = date('jS M Y', strtotime($rowdate));

            ?>
                <div class="row row-post">
                    <div class="col-7 row-title">
                        <h3 class="title"><?php echo $row['blog_title'] ?></h3>
                        <p class="date"><span class="highlight"><?php echo $row['blog_author'] ?></span>, <?php echo $dt ?></p>
                        <br>
                        <a class="link" href="?post_id=<?php echo $row['blog_id'] ?>">Read More</a>
                    </div>
                    <div class="col-5 parent">
                         <div class="image" style="background-image:url('<?php echo $row['blog_image'] ?>')" ;></div>
                    </div>
                </div>

            <?php

        }
         /* free result set */
         mysqli_free_result($result);
    }

    function specificBlog(){

        include "config.php";

        $postid = $_GET['post_id'];

        //sql to find all information in the blog and to arrange it in descending order.
        $thisBlog = "SELECT * FROM blog WHERE blog_id = '$postid'";
        //Result of querying the database
        $blogresult = mysqli_query($connection, $thisBlog) or die("Bad Query: $thisBlog");

        while($row = mysqli_fetch_assoc($blogresult)){

            $rowdate = $row['blog_date'];
            $dt = date('jS M Y', strtotime($rowdate));
            $image = $row[ 'blog_image'];
            
            ?>

            <div class="post-wrap">
                <div id="myImg" class="post-image" style="background-image:url('<?php echo $image ?>');">

                </div>
                <!-- The Modal -->
                <div id="myModal" class="modal">
                <span class="close">&times;</span>
                <!-- Modal Content (The Image) -->
                <div class="modal-content" id="img01" style="background-image:url('<?php echo $image ?>');"></div>
                </div>

                <div class="container text-white">
                    <div class="col-12 post">
                        <h1 class=""><?php echo $row[ 'blog_title' ]; ?></h1>
                        <hr>
                    <!--  <p>Sub title about some Lorem ipsum dolor sit amet consectetur, adipisicing elit. Aliquam
                            corrupti
                            perferendis ipsam.</p> -->
                        <p class="highlight"><?php echo $row[ 'blog_author' ]; ?></p>
                        <p class="grey-text"><?php echo $dt ?></p>
                        <br>
                        <br>
                        <?php echo $row[ 'blog_content' ]; ?>

                    </div>
                    <a class="button" href="blog.php">Back to blogs</a>
                </div>
            </div>
            <script>
                // Get the modal
                var modal = document.getElementById('myModal');
                // Get the image and insert it inside the modal - use its "alt" text as a caption
                var img = document.getElementById('myImg');
                
                img.onclick = function(){
                    modal.style.display = "block";
                }
                // Get the <span> element that closes the modal
                var span = document.getElementsByClassName("close")[0];
                // When the user clicks on <span> (x), close the modal
                span.onclick = function() { 
                    modal.style.display = "none";
                }
                modal.onclick = function(){
                    modal.style.display = "none";
                }
            </script>

            <?php
            
        }
        /* free result set */
        mysqli_free_result($blogresult);
    }

    ?>
