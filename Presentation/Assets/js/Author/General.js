$(document).ready(function () {
    AjustHeight();

    AjustEditTextSize();

    SaveKey();
    SaveButton();

    $("form")[0].reset();

    $(window).resize(function () {
        AjustEditTextSize();
    });
});