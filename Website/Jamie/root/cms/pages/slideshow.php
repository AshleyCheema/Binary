<div id="title-panel" class="panel p-bot">
    <h5 class="light-text pl-3 rwnorm pt-2">Slideshow</h5>
</div>

<div class="container">
    <h3 class="conmg white">Upload new Slide</h3>
    <hr>
    <form enctype="multipart/form-data" action="" method="POST">

        <div class="row mgt">
            <p class="col-12 col-md-2">Upload Image:</p>
            <input class="form-control col-12 col-md-10" type="file" name="slide_image" accept="image/*">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Slide Title:</p>
            <input class="form-control col-12 col-md-10" type="text" name="title" placeholder="Slide Title">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Slide Subtitle:</p>
            <input class="form-control col-12 col-md-10" type="text" name="subtitle" placeholder="Slide Subtitle">
        </div>
    </form>
    <div class="space"></div>
    <h3 class="conmg white">Existing Slides</h3>
    <hr>
    <div id="slide-container" class="mgt light-text p-4">

        <div class="slide-item m-4"><img height="200px" width="200px" src="https://via.placeholder.com/200"><br>
            <p class="text-center">Slide title</p>
            <div class="hover-item">
                <i class="fas fa-times-circle"></i>
            </div>
        </div>
    </div>
</div>