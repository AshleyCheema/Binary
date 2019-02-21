$(document).ready(function () {

    resizeOnStart();

    ajaxRequstOnLoad();   

});

$('#open-btn').click(function () {

    $('#content-panel').removeClass('mg-sm');
    $('#title-panel').removeClass('newWidth');

    $('#smallnav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#sidenav').show("slide", {
        direction: "left"
    }, 500);

});

$('#close-btn').click(function () {

    $('#content-panel').addClass('mg-sm');
    $('#title-panel').addClass('newWidth');

    $('#sidenav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#smallnav').show("slide", {
        direction: "left"
    }, 500);

});


function resizeOnStart(){
    //If window is smaller than 768 show the small navigation
    if ($(window).width() < 768) {
        $('#sidenav').hide();
        $('#smallnav').show();
        $('#content-panel').addClass('mg-sm');
        $('#title-panel').addClass('newWidth');
    } else { //else show the large navigation
        $('#smallnav').hide();
    }
}

function ajaxRequstOnLoad(){

    var url_string = window.location.href;
    var url = new URL(url_string);
    var page = url.searchParams.get("page");
    //console.log(page);

    if (page != null){
        //Ajax load the necessary page
        $("#content-panel").load('pages/'+ page + '.php');
    } else {
        //default load
        $("#content-panel").load('pages/profile.php');
    }

}

$('.cms-button').click(function (){
    event.preventDefault();
});




//Ajax function to load the required content
/* $('.a-link').click(function () {

    event.preventDefault();

    //Remove existing class from all elements
    $(".nav-btn-active").removeClass('nav-btn-active');

    //find the additional class that is on the button
    $class = "." + $(this).attr('class').split(' ')[1];

    //find link based on classes href;
    $href = $($class).attr('href');

    //give active class to all buttons with the same additional class
    $($class).find('.nav-btn').addClass('nav-btn-active');

    //console.log($href);

    //Ajax load the page
    $("#content-panel").load($href);

}); */