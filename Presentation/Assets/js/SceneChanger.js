function SceneChanger() {
    $(".SceneChanger li").click(function () {
        var scene = $(this).attr("scene");

        $(".SceneChanger li.chosen").removeClass("chosen");
        $(".sceneChanger" + scene).addClass("chosen");

        $(".scenes .reading").removeClass("reading");
        $("#Scene" + scene).addClass("reading");

        $(".main").scrollTop(0);
    });
}