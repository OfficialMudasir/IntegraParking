$(document).ready(function() { 

	var cookieName = 'skin-select',
		cookieOptions = {expires: 7, path: '/'};

    if($.cookie(cookieName) == null) {
        $.cookie(cookieName, '0', cookieOptions);
    }

        
    function SkinSelect() {
        
        if($.cookie(cookieName) == "0") {
            /*
            $('#skin-select').animate({ left: 0 }, 100);	
            $('.wrap-fluid').css({"width":"auto","margin-left":"320px"});
            $('.navbar').css({"margin-left":"240px"});
            $('#skin-select li').css({"text-align":"left"});
            //$('#skin-select li span, ul.topnav h4, .side-dash, .noft-blue, .noft-purple-number, .noft-blue-number, .title-menu-left').css({"display":"inline-block", "float":"none"});
            //$('body').css({"padding-left":"250px"});
    
    
            $('.ul.topnav li a:hover').css({" background-color":"green!important"});
            $('.ul.topnav h4').css({"display":"none"});
            
            $('.datepicker-wrap').css({"position":"absolute", "right":"300px"});
            $('.skin-part').css({"visibility":"visible"});
            //$('#menu-showhide, .menu-left-nest').css({"margin":"10px"});
            //$('#search-box').css({"display":"block"});
            $('#search-collapse').css({"display":"none"});
            
            $('.dropdown-wrap').css({"position":"absolute", "left":"0px", "top":"53px"});
            */
            $('#skin-select').removeClass('collapsed');
            $('#page-wrap').removeClass('collapsed');
            
            $('.tooltip-tip2').addClass('tooltipster-disable');
            $('.tooltip-tip').addClass('tooltipster-disable');


            
        } else {
            
            /*
            //$('#skin-select').animate({ left: -270 }, 100);

            //$('.wrap-fluid').css({"width":"auto", "margin-left":"50px"});
            
            $('.navbar').css({"margin-left":"50px"});
            
            $('#skin-select li').css({"text-align":"right"});
            
            //$('#skin-select li span, ul.topnav h4, .side-dash, .noft-blue, .noft-purple-number, .noft-blue-number, .title-menu-left').css({"display":"none"});
            
            //$('body').css({"padding-left":"50px"});
            
            $('.datepicker-wrap').css({"position":"absolute", "right":"84px"});
            
            $('.skin-part').css({"visibility":"visible", "top":"3px"});
            //$('#search-box').css({"display":"none"});
            //$('#menu-showhide, .menu-left-nest').css({"margin":"0"});
            
            //$('#search-collapse').css({"display":"block", "position":"relative", "left":"270px"});
            
            $('.dropdown-wrap').css({"position":"absolute", "left":"-10px", "top":"53px"});
            */
            $('#skin-select').addClass('collapsed');
            $('#page-wrap').addClass('collapsed');
            
            $('.tooltip-tip2').removeClass('tooltipster-disable');
            $('.tooltip-tip').removeClass('tooltipster-disable');

            

        }
        return false;
    }
    
    SkinSelect();


    // toggle skin select	
    $("#skin-select #toggle").click(function(e) {
        e.preventDefault();
        if($.cookie(cookieName) == "0") {
            $(this).removeClass('active');
            $.cookie(cookieName, '-270', cookieOptions);
            SkinSelect();
        } else {
            $(this).addClass('active');
            $.cookie(cookieName, '0', cookieOptions);
            SkinSelect();
        }
    });
    
    // toggle skin select PHONE	SMALL!
    $("nav #aside-sm-toggle").click(function(e) {
        e.preventDefault();
        $(this).toggleClass('active');
        $("#skin-select").toggleClass('active');
            
    });
        



    function queryListener(x) {
        if (x("(min-width: 700px)").matches) { // If media query matches
            document.body.style.backgroundColor = "yellow";
            console.warn('The viewport is less than, or equal to, 700 pixels wide');

        } else {
            document.body.style.backgroundColor = "pink";
            console.info('The viewport is greater than 700 pixels wide');
            
        }
    }
        


	
}); // end doc.ready



var xs = window.matchMedia("(min-width: 425px)");
xs.addListener(mqlogger);
var sm = window.matchMedia("(min-width: 768px)");
sm.addListener(mqlogger);
var md = window.matchMedia("(min-width: 992px)");
md.addListener(mqlogger);
var lg = window.matchMedia("(min-width: 1200px)");
lg.addListener(mqlogger);

var mqlogger = function(e)
{

    if(lg.matches) {
        console.log(lg.media);
    } else
    if(md.matches) {
        console.log(md.media);
    } else
    if(sm.matches) {
        console.log(sm.media);
    } else
    if(xs.matches) {
        console.log(xs.media);
    } 

};

if (document.body.classList.contains('querymedia'))
{
    window.addEventListener("resize",mqlogger);
}


