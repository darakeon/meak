$(document).ready(function () {
    AjustHeight();

    AjustEditTextSize();

    $(window).resize(function () {
        AjustEditTextSize();
    });
});