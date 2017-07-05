$(document).ready(function () {
    AjustHeight();

    AjustEditTextSize();

    SaveKey();
    SaveButton();

    $("form").reset();

    $(window).resize(function () {
        AjustEditTextSize();
    });
});