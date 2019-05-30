<?php


    //includes login script
    include( '../functions.php' );

    checkLogin();
    
?>

<div id="title-panel" class="panel p-bot">
    <h5 class="light-text pl-3 rwnorm pt-2">Slideshow</h5>
</div>

<div class="container">
    <h3 class="conmg white">Upload new Slide</h3>
    <hr>
    <form enctype="multipart/form-data" action="" method="POST">

        <div class="row mgt">
            <p class="col-12 col-md-2">Upload Image:</p>
            <input class="form-control col-12 col-md-10" type="file" name="fileToUpload" accept="image/*">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Slide Title:</p>
            <input class="form-control col-12 col-md-10" type="text" name="slide-title" placeholder="Slide Title">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Slide Subtitle:</p>
            <input class="form-control col-12 col-md-10" type="text" name="slide-subtitle" placeholder="Slide Subtitle">
        </div>
        <div class="d-flex justify-content-center mt-5 login_container">
            <button type="submit" name="add-slide" value="add-slide" class="button login_btn">Add Slide</button>
        </div>
    </form>
    <div class="space"></div>
    <h3 class="conmg white">Existing Slides</h3>
    <hr>
    <div id="slide-container" class="mgt light-text p-4">
        <?php showSlideCms() ?>
    </div>
</div>