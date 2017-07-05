function AjustMainDivSize() {
    AjustHeight();

    $(".main").show();

    AjustFinalMargin();
}

function AjustHeight() {
    var windowHeight = $(window).height();


    var general = ScaleVerticalEdge("body");
    var main = ScaleVerticalEdge(".main");

    var head = ScaleHeight(".head");

    var mainHeight = windowHeight - general - main;

    $(".main").height(mainHeight);
}


var secureSpace = 10;

function AjustFinalMargin() {
    var main = NumericValueCss(".main", "height");

    var paddingBottom = main / 4;

    $(".main .sign img").css("margin-top", paddingBottom);
}



function AjustLogin() {
    if ($(".login .inner")[0]) {
        AjustOuterBox();
        AjustInnerBox();
    }
}


function AjustOuterBox() {
    $(".login").addClass("darkback");
    $(".login").width($(window).width());
    $(".login").height($(window).height());
}


function AjustInnerBox() {

    $(".login").css("padding", 0);

    var vertical = LoginVertical();
    var horizontal = LoginHorizontal();

    var padding =
            vertical + "px " +
            horizontal + "px ";

    $(".login").css("padding", padding);
}

function LoginVertical() {
    var login = ScaleHeight(".login");
    var inner = $(".login .inner").height();

    return (login - inner) / 2;
}

function LoginHorizontal() {
    var login = ScaleWidth(".login");
    var inner = $(".login .inner").width();

    return (login - inner) / 2;
}