<?php


    //includes login script
    include( '../functions.php' );

    checkLogin();
    
?>

<div id="title-panel" class="panel p-bot">
    <h5 class="light-text pl-3 rwnorm pt-2">Profile</h5>
</div>

<div class="container">

    <div class="row conmg">
        <div style="margin-right:20px; background-image:url('<?php echo $_SESSION[ 'pic' ]?>');background-size:cover;height:200px;width:200px;"></div>
        <div class="white float-left" >
            <h2><?php echo $_SESSION[ 'name' ]?></h2>
            <p><?php echo $_SESSION[ 'login_email' ]?> - <?php echo $_SESSION[ 'rank' ]?></p>
        </div>
    </div>

    <h3 class="conmg white">Account</h3>
    <hr>
    <form enctype="multipart/form-data" action="" method="POST">
        <div class="row mgt">
            <p class="col-12 col-md-2">Full name:</p>
            <input class="form-control col-12 col-md-10" type="text" name="name" value="<?php echo $_SESSION[ 'name' ]?>"autocomplete="off">
        </div>

        <div class="row mgt">
            <p class="col-12 col-md-2">Email:</p>
            <input class="form-control col-12 col-md-10" type="text" name="email" value="<?php echo $_SESSION[ 'email' ]?>"autocomplete="off">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Password:</p>
            <input class="form-control col-12 col-md-10" type="password" name="password" value="****"autocomplete="off">
        </div>
        <div class="row mgt">
            <p class="col-12 col-md-2">Rank:</p>
            <select class="form-control col-12 col-md-10" list="rank" name="rank"autocomplete="off">
                <?php

                    if ($_SESSION[ 'rank' ] == "Admin"){
                        ?>
                        <option value="Guest">Guest</option>
                        <option value="Admin" selected>Admin</option>
                        <?php
                    } else{
                        ?>
                        <option value="Guest" selected>Guest</option>
                        <option value="Admin">Admin</option>
                        <?php
                    }?>
            
            </select>
        </div>

        <div class="d-flex justify-content-center mt-5 login_container">
            <button type="submit" name="update-information" value="update-information" class="button login_btn">Update</button>
        </div>
    </form>

</div>
					     