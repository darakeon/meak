$(document).ready(function () {
	SaveKey();
	SaveButton();

	$('form')[0].reset();

	$('#main-info .toggle').click(function () {
		const form = $('#main-info form');

		if (form.is(":visible")) {
			$('#main-info form').hide()
			$(this).html('&#129090;')
		} else {
			$('#main-info form').show()
			$(this).html('&#129088;')
		}
	})
});

