<?php


    //includes login script
    include( '../functions.php' );

    checkLogin();
    
?>

<div id="title-panel" class="panel p-bot">
    <h5 class="light-text pl-3 rwnorm pt-2">Slideshow</h5>
</div>

<div class="container">
    <h3 class="conmg white">Edit Slide</h3>
    <hr>
    <form enctype="multipart/form-data" action="" method="POST">
<!-- 
        <div class="row mgt">
            <p class="col-12 col-md-2">Upload Image:</p>
            <input class="form-control col-12 col-md-10" type="file" name="fileToUpload" accept="image/*">
        </div> -->
        <div class="row mgt">
            <p class="col-12 col-md-2">Slide Title:</p>
            <input class="form-control col-12 col-md-10" type="text" name="slide-title" value="<?php echo $_SESSION['this_slide_title'] ?>">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Slide Subtitle:</p>
            <input class="form-control col-12 col-md-10" type="text" name="slide-sub" value="<?php echo $_SESSION['this_slide_sub'] ?>">
        </div>
        <div class="d-flex justify-content-center mt-5 login_container">
            <button type="submit" name="update-slide" value="update-slide" class="button login_btn">Update Slide</button>
            <a href="dashboard.php?page=slideshow" class="button login_btn ml-3">Cancel</a>
        </div>
    </form>
</div>