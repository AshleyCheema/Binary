
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
            <input class="form-control col-12 col-md-10" type="text" name="blog_title" placeholder="Slide Title">
        </div>

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Main:</p>
            <input class="form-control col-12 col-md-10" type="file" name="blog_image" accept="image/*">
        </div>

        <div class="row mgt">
            <p class="col-12 col-md-2">Blog Content:</p>
            <div class="col-12 col-md-10" style="padding-left:0px; padding-right:0px">
                <textarea type="text" name="blog_content" style="min-height:300px;">
                </textarea>
            </div>
        </div>
        <button type="submit" class="btn btn-primary mgt">Submit</button>
    </form>

    <h3 class="conmg white mt-5">Manage Existing Blogs</h3>
    <hr>

    <div id="slide-container" class="mgt light-text p-4">

        <div class="slide-item m-4"><img height="200px" width="200px" src="https://via.placeholder.com/200"><br>
            <p class="text-center">Blog title</p>
            <div class="hover-item">
                <i class="fas fa-times-circle"></i>
            </div>
        </div>
    </div>
</div>
