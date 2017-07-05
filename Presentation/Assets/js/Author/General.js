$(document).ready(function () {
    AjustHeight();

    AjustEditTextSize();

    SaveKey();
    SaveButton();

    $(window).resize(function () {
        AjustEditTextSize();
    });
});