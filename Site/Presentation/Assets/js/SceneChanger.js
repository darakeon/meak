function SceneChanger() {
	$(".scene-changer li").click(function () {
		var scene = $(this).attr("scene");

		$(".scene-changer li.chosen").removeClass("chosen");
		$(".scene-changer" + scene).addClass("chosen");

		$(".scenes .reading").removeClass("reading");
		$("#Scene" + scene).addClass("reading");

		$(document).scrollTop(0);
	});
}