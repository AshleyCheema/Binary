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
    <title>Retro Gecko</title>
    <link rel="apple-touch-icon" sizes="180x180" href="/images/fav/apple-touch-icon.png">
    <link rel="icon" type="image/png" sizes="32x32" href="/images/fav/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="16x16" href="/images/fav/favicon-16x16.png">
    <link rel="manifest" href="/images/fav/site.webmanifest">
    <link rel="mask-icon" href="/images/fav/safari-pinned-tab.svg" color="#5bbad5">
    <meta name="msapplication-TileColor" content="#00a300">
    <meta name="theme-color" content="#ffffff">
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
        <!--Landing Space-->
        <div id="space" class="home-space">
            <h1 id="title-text" class="text-center text-white rwthin">RETRO<span class="sym">GECKO</span></h1>
            <a id="start" class="hidden exo2">Let's Begin</a>
        </div>
        <!--End-->
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
                        <li class="nav-item active">
                            <a class="nav-link" href="index.php">Home <span class="sr-only">(current)</span></a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="blog.php">Blog</a>
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
            <!--Slideshow-->
            <div class="owl-carousel owl-theme slide-show">
                <?php
                    displaySlides();
                ?>
            </div>
            <!--End SlideShow-->
            <div class="">
                <!--BLOG-->
                <div id="blog" class="text-white mt-5 page">
                    <h2>Latest News</h2>
                    <hr>
                    <div class="row middle container">
                            <?php threeBlogs() ?>
                        </div>
                    </div>
                    <div class="row pt-4">
                        <a class="button middle" href="blog.php">See more news</a>
                    </div>
                </div>
                <!--EndBlog-->

                <!--Meet The Team-->
                <div class="page mt-5 text-white">
                    <h2 class="mb-2">Meet the Team</h2>
                    <hr>
                    <div class="owl-carousel meet-carousel">
                        <div class="team-mem chewy">
                            <div class="overlay">
                                <div>
                                    <h4>Ashley Cheema</h4>
                                    <p>Project Manager & Programmer</p>
                                    <a class="team-link" href="https://ashleycheema.github.io" target="_blank">Visit Ashley's website</a>
                                </div>
                            </div>
                        </div>
                        <div class="team-mem ian">
                            <div class="overlay">
                                <div>
                                    <h4>Ian Hudson</h4>
                                    <p>Programmer</p>
                                    <a class="team-link" href="https://i-hudson.github.io/" target="_blank">Visit Ian's website</a>
                                </div>
                            </div>
                        </div>
                        <div class="team-mem sandles">
                            <div class="overlay">
                                <div>
                                    <h4>Lysander Foster</h4>
                                    <p>3D art & Design</p>
                                    <a class="team-link" href="https://lysanderfoster.com" target="_blank">Visit Lysander's website</a>
                                </div>
                            </div>
                        </div>
                        <div class="team-mem jess">
                            <div class="overlay">
                                <div>
                                    <h4>Jess Barrett</h4>
                                    <p>Concept art & Design</p>
                                    <a class="team-link" href="https://jessbarrett96.wixsite.com/website" target="_blank">Visit Jess' website</a>
                                </div>
                            </div>
                        </div>
                        <div class="team-mem james">
                            <div class="overlay">
                                <div>
                                    <h4>James Barrett</h4>
                                    <p>Level Design</p>
                                    <a class="team-link" href="https://jamesgbarrett.portfoliobox.net/" target="_blank">Visit James' website</a>
                                </div>
                            </div>
                        </div>
                        <div class="team-mem jamie">
                            <div class="overlay">
                                <div>
                                    <h4>Jamie Laurence</h4>
                                    <p>Media Developer</p>
                                    <a class="team-link" href="http://www.jamielaurence.co.uk" target="_blank">Visit Jamie's website</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row pt-4">
                        
                    </div>
                </div>
                <!--END Meet the team-->
            </div>
            </div>
            <footer class="footer">
                <p class="highlight">&copy Retro Gecko 2019, all rights reserved</p>
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