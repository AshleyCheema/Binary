<?php


    //includes login script
    include( '../functions.php' );

    checkLogin();
    
?>
<script>tinymce.init({ selector:'textarea' });</script>

<div id="title-panel" class="panel p-bot">
    <h5 class="light-text pl-3 rwnorm pt-2">Blog</h5>
</div>

<div class="container">

    <h3 class="conmg white">Upload new Blog</h3>
    <hr>

    <form enctype="multipart/form-data" action="" method="POST">

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Title:</p>
            <input class="form-control col-12 col-md-10" type="text" name="blog-title" placeholder="Slide Title">
        </div>

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Main Image:</p>
            <input class="form-control col-12 col-md-10" type="file" name="fileToUpload" accept="image/*">
        </div>

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Content:</p>
            <div class="col-12 col-md-10" style="padding-left:0px; padding-right:0px">
                <textarea type="text" name="blog-content" style="min-height:300px;">
                </textarea>
            </div>
        </div>
        <div class="d-flex justify-content-center mt-5 login_container">
            <button type="submit" name="add-blog" class="button login_btn" value="add-blog" >Submit</button>
        </div> 
    </form>

    <h3 class="conmg white mt-5">Manage Existing Blogs</h3>
    <hr>

    <div id="slide-container" class="mgt light-text p-4">

    <?php showBlogCms() ?>

    </div>
</div>
