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

    <h3 class="conmg white">Edit Blog</h3>
    <hr>

    <form enctype="multipart/form-data" action="" method="POST">

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Title:</p>
            <input class="form-control col-12 col-md-10" type="text" name="blog-title" value="<? echo $_SESSION['this_title']; ?>">
        </div>

        <!-- <div class="row mgt">
            <p class="col-12 col-md-2">Blog Main Image:</p>
            <input class="form-control col-12 col-md-10" type="file" name="fileToUpload" accept="image/*" placeholder="<? echo $_SESSION['this_image']; ?>">
        </div> -->

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Content:</p>
            <div class="col-12 col-md-10" style="padding-left:0px; padding-right:0px">
                <textarea type="text" name="blog-content" style="min-height:300px;" value="">
                    <? echo $_SESSION['this_content']; ?>
                </textarea>
            </div>
        </div>
        <div class="d-flex justify-content-center mt-5 login_container">
            <button type="submit" name="update-post" class="button login_btn">Submit</button>
            <a href="dashboard.php?page=blog" class="button login_btn ml-3">Cancel</a>
        </div>
    </form>
</div>
