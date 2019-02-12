<?php

 //includes login script
 include( 'functions.php' );

 //checks to see if user is logged in
 checkLogin();

?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Retro Gecko Dashboard</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS"
        crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/Swiper/4.3.5/css/swiper.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.1/css/all.css" integrity="sha384-gfdkjb5BdAXd+lj+gudLWI+BXq4IuLW5IT+brZEZsLFm++aCMlF1V92rMkPaX4PP"
        crossorigin="anonymous">
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <link rel="stylesheet" href="/font/customfonts.css">
    <link rel="stylesheet" href="css/dashboard.css">
</head>

<body class="base">

    <div id="wrapper">

        <nav class="navbar navbar-default navbar-static-top panel p-bot text-white">
            <h3><span class="rwthin pl-3 ">RETRO</span><span class="sym">GECKO</span></h3>

            <a href="logout.php">Logout <i class="fas fa-sign-out-alt"></i> </a>
        </nav>

        <div id="sidenav" class="panel p-right">

            <div class="profile mb-5">

                <div class="row">
                    <div class="d-inline-flex ml-5 mt-5">
                        <img class="pro-img" src="<?php echo $_SESSION[ 'pic' ]?>" height="75px" />
                        <div class="div ml-3 mt-2">
                            <h5 class="light-text sym" style="max-width:170px;"><?php echo $_SESSION[ 'name' ] ?></h5>
                            <p class="hi-text"><?php echo $_SESSION[ 'rank' ] ?></p>
                        </div>
                    </div>
                </div>
            </div>

            <a class="a-link a-1" href="pages/profile.php">
                <div class="nav-btn nav-btn-active d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-home"></i></div>
                    <div class="item vr ml-3"></div>
                    <p class="ml-3 mt-3">Home</p>
                </div>
            </a>

            <a class="a-link a-2" href="pages/profile.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-user-alt"></i></div>
                    <div class="item vr ml-3"></div>
                    <p class="ml-3 mt-3">Profile</p>
                </div>
            </a>
          <!--   <a class="a-link a-3" href="pages/users.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-users"></i></div>
                    <div class="item vr ml-3"></div>
                    <p class="ml-3 mt-3">Users</p>
                </div>
            </a> -->
            <a class="a-link a-4" href="pages/slideshow.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-image"></i></div>
                    <div class="item vr ml-3"></div>
                    <p class="ml-3 mt-3">Slideshow</p>
                </div>
            </a>
            <a class="a-link a-5" href="pages/blog.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-font"></i></div>
                    <div class="item vr ml-3"></div>
                    <p class="ml-3 mt-3">Blog</p>
                </div>
            </a>

            <a href="#" style="font-size: 32pt;" id="close-btn" class="item light-text fixed-bottom">
                <i class=" fas fa-caret-left float-right pr-3"></i>
            </a>

        </div>

        <div id="smallnav" class="panel p-right">

            <div class="profile mb-5">
                <img class="pro-img-small ml-2 mt-5" src="<?php echo $_SESSION[ 'pic' ]?>" height="50px" />
            </div>
            <a class="a-link a-1" href="pages/profile.php">
                <div class="nav-btn nav-btn-active d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-home"></i></div>
                </div>
            </a>
            <a class="a-link a-2" href="pages/profile.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-user-alt"></i></div>

                </div>
            </a>
          <!--   <a class="a-link a-3" href="pages/users.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-users"></i></div>
                </div>
            </a> -->
            <a class="a-link a-4" href="pages/slideshow.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-image"></i></div>
                </div>
            </a>
            <a class="a-link a-5" href="pages/blog.php">
                <div class="nav-btn d-flex justify-content-left align-items-center">
                    <div class="item ml-3"><i class="fas fa-font"></i></div>
                </div>
            </a>

            <a href="#" style="font-size: 32pt;" id="open-btn" class="item light-text fixed-bottom">
                <i class=" fas fa-caret-right float-right pr-3"></i>
            </a>

        </div>
        <div id="content-panel">

  
         
            <!--< ?php 
                $allowed = array('home', 'profile', 'users', 'slideshow', 'blog'); // add the pagenames you need
                $page = ( isset($_GET['page']) ) ? $_GET['page'] : 'home';
                if( in_array( $page, $allowed ) ){
                    include("pages/$page.php");
                } else {
                    include("pages/404.php");
            } ?>-->
           
        </div>

    </div>


    <script src="https://code.jquery.com/jquery-3.3.1.js" integrity="sha256-2Kok7MbOyxpgUVvAk/HJ2jigOSYS2auK4Pfzbm7uH60="
        crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30="
        crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Swiper/4.3.5/js/swiper.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut"
        crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k"
        crossorigin="anonymous"></script>
    <script src="js/main.js"></script>

    <script>

    </script>
</body>

</html>