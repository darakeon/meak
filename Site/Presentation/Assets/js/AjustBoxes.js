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