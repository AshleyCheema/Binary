<div id="title-panel" class="panel p-bot">
    <h5 class="light-text pl-3 rwnorm pt-2">Profile</h5>
</div>

<div class="container">

    <div class="row conmg">
        <div><img src="https://via.placeholder.com/200"></div>
        <div class="white float-left" style="margin-left:20px;">
            <h2>Full name</h2>
            <p>email@connect.glos.ac.uk - Administrator</p>
        </div>
    </div>

    <h3 class="conmg white">Account</h3>
    <hr>
    <form enctype="multipart/form-data" action="" method="POST">
        <div class="row mgt">
            <p class="col-12 col-md-2">Full name:</p>
            <input class="form-control col-12 col-md-8" type="text" name="fullname" value="First name">
        </div>

        <div class="row mgt">
            <p class="col-12 col-md-2">Email:</p>
            <input class="form-control col-12 col-md-8" type="text" name="fullname" value="email@email.com">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Password:</p>
            <input class="form-control col-12 col-md-8" type="password" name="fullname" value="password">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Rank:</p>
            <select class="form-control col-12 col-md-8" list="rank" name="myRank" value="Admin">
                <option value="Guest">Guest</option>
                <option value="Admin" selected>Admin</option>
            </select>
        </div>

        <div class="d-flex justify-content-center mt-5 login_container">
            <button type="button" name="button" value="submit" class="button login_btn">Update</button>
        </div>
    </form>

</div>