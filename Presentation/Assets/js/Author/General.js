$(document).ready(function () {
    AjustHeight();
    
    AjustEditTextSize();

    $(window).resize(AjustEditTextSizeNoParam);
});

//To not pass any parameters
function AjustEditTextSizeNoParam() {
    AjustEditTextSize();
}