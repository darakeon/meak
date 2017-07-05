$(document).ready(LayoutAndWork);

function LayoutAndWork() {
    AjustMainDivSize();
    ChooseBackground();

    AnimateLegend();

    AjustLogin();

    $(window).resize(AjustMainDivSize);
    $(window).resize(AjustLogin);
}