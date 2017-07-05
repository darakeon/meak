function SceneChanger() {
    $(".sceneChanger li").click(function () {
        var scene = $(this).attr("scene");

        $(".sceneChanger li.chosen").removeClass("chosen");
        $(".sceneChanger" + scene).addClass("chosen");

        $(".scenes .reading").removeClass("reading");
        $("#Scene" + scene).addClass("reading");

        $(".main").scrollTop(0);
    });
}