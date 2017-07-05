$(document).ready(LayoutAndWork);

function LayoutAndWork() {
    AjustMainDivSize();
    ChooseBackground();

    AnimateLegend();

    AjustLogin();

    SceneChanger();

    $(window).resize(AjustMainDivSize);
    $(window).resize(AjustLogin);
}