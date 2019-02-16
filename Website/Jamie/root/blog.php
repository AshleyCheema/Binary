<?php

//includes login script
include( 'cms/functions.php' );

?>
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Retro Gecko | Blog</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS"
        crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/Swiper/4.3.5/css/swiper.min.css">
    <link href="https://fonts.googleapis.com/css?family=Raleway:200,900" rel="stylesheet">
    <link rel="stylesheet" href="owl/owl.carousel.min.css">
    <link rel="stylesheet" href="owl/owl.theme.default.min.css">
    <link rel="stylesheet" href="font/customfonts.css">
    <link rel="stylesheet" href="css/main.css">
</head>

<body>
    <div id="wrapper">
        <!--Navigation-->
        <div id="newBody">
            <nav id="navbar" class="navbar navbar-expand-lg navbar-dark bg-dark ">
                <a id="brandtitle" class="navbar-brand" href="#"><span class="rwthin pl-3">RETRO</span><span class="sym">GECKO</span></a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
                    aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse justify-content-end pr-3" id="navbarNav">
                    <ul class="navbar-nav">
                        <li class="nav-item">
                            <a class="nav-link" href="index.php">Home </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="about.html">About</a>
                        </li>
                        <li class="nav-item active">
                            <a class="nav-link" href="blog.php">Blog<span class="sr-only">(current)</span></a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="leaderboard.php">Leaderboard</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="/cms">Sign in</a>
                        </li>
                    </ul>
                </div>
            </nav>
            <!--End Navigation-->
            <div class="container pt-5">
                <h2 class="text-white">News</h2>
                <hr>

                <!-- <div class="row row-post">
                    <div class="col-7 row-title">
                        <h3 class="title">Random ass, slightly long blog title.</h3>
                        <p class="date"><span class="highlight">Jamie Laurence</span>, February 07th 2019</p>
                        <br>
                        <a class="link" href="#">Read More</a>
                    </div>
                    <div class="col-5 image" style="background-image:url('')" ;></div>
                </div> -->
                <?php
                    allBlogs();
                ?>

            </div>

        </div>
        <footer class="footer">
            <p>&copy Retro Gecko 2019, all rights reserved</p>
        </footer>
    </div>

    <!-- threejs.org canvas lines example -->
    <script src="
                https://cdnjs.cloudflare.com/ajax/libs/three.js/r67/three.min.js">
    </script>
    <script src="https://code.jquery.com/jquery-3.3.1.js" integrity="sha256-2Kok7MbOyxpgUVvAk/HJ2jigOSYS2auK4Pfzbm7uH60="
        crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Swiper/4.3.5/js/swiper.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut"
        crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k"
        crossorigin="anonymous"></script>
    <script src="owl/owl.carousel.min.js"></script>
    <script src="js/particle-background.js"></script>
    <script src="js/main.js"></script>
</body>

</html>