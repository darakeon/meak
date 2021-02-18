function BlockChanger() {
	$(".block-changer li").click(function () {
		var block = $(this).attr("block");

		$(".block-changer li.chosen").removeClass("chosen");
		$(".block-changer" + block).addClass("chosen");

		$(".blocks .reading").removeClass("reading");
		$("#Block" + block).addClass("reading");

		$(document).scrollTop(0);
	});
}