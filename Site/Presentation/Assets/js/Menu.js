function PutLink() {
	$(".head").click(function () {
		location = '/';
	});
}


function ShowStoryNames() {
	$(".linkWithTitle").hover(
		function () { showName(this); },
		function () { hideName(this); }
	);
}


function showName(obj) {
	var name = $(obj).attr("rel");
	replaceName(obj, name);
}

function hideName(obj) {
	replaceName(obj, "");
}

function replaceName(obj, name) {
	$(obj).closest(".season")
		  .find(".storyNameBox")
		  .html(name);
}
