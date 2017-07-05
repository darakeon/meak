$(document).ready(LayoutAndWork);

function LayoutAndWork() {
    AjustMainDivSize();
    ChooseBackground();

    //AnimateLegend();
    ShowStoryNames();

    AjustLogin();

    SceneChanger();
    FontResizer();

    $(window).resize(AjustMainDivSize);
    $(window).resize(AjustLogin);
}