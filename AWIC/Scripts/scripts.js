"use strict";

/* DROP MENU */
$('#mobile-menu > ul > li').click(function(){
    $(this).children('ul').toggle({display: "toggle"});
});

$('#mobile-button').click(function(){
    $('#mobile-menu').toggle({display: "toggle"});
});

function InitHome() {

    $('#slider').flexslider({
        animation: "slide",
        animationLoop: true,
        prevText: '',
        nextText: ''
    });

    $('#photo-slider').flexslider({
        animation: "slide",
        animationLoop: true,
        prevText: '',
        nextText: '',
        itemMargin: 10
    });

    $('section.news .photos h3 .control .left').click(function() {
        var slider = $('#photo-slider').data('flexslider');
        slider.flexslider("prev");
    });

    $('section.news .photos h3 .control .right').click(function() {
        var slider = $('#photo-slider').data('flexslider');
        slider.flexslider("next");
    });

}


function InitBlog() {

    $('#photostream').flexslider({
        animation: "slide",
        animationLoop: true,
        prevText: '',
        nextText: ''
    });

    $('aside.blog #photostream h3 .control .left').click(function() {
        var slider = $('#photostream').data('flexslider');
        slider.flexslider("prev");
    });

    $('aside.blog #photostream h3 .control .right').click(function() {
        var slider = $('#photostream').data('flexslider');
        slider.flexslider("next");
    });


    $('#aside-dynamic .menu > div').click(function() {
        var attr = $(this).attr('data-attr');

        $('#aside-dynamic .menu > div').removeClass('active');
        $(this).addClass('active');


        $('#aside-dynamic .content').css('display','none');
        $('#aside-dynamic .content.' + attr).css('display','block');
    });

}


