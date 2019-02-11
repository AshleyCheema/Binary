$(document).ready(function () {
    $('#smallnav').hide();

    //default load
    $("#content-panel").load('pages/profile.php');


});

$('#open-btn').click(function () {

    $('#content-panel').removeClass('mg-sm');
    $('#content-panel').addClass('mg-lg');
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
    $('#content-panel').removeClass('mg-lg');
    $('#title-panel').addClass('newWidth');

    $('#sidenav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#smallnav').show("slide", {
        direction: "left"
    }, 500);

});


//Ajax function to load the required content
$('.a-link').click(function () {

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

});