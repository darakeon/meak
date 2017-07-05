$(document).ready(LayoutAndWork);

function LayoutAndWork() {
    AjustMainDivSize();
    ChooseBackground();

    AnimateLegend();

    AjustLogin();

    SceneChanger();
    FontResizer();

    $(window).resize(AjustMainDivSize);
    $(window).resize(AjustLogin);
}