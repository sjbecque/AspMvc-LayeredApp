
function showModal(options) {
		
	options = $.extend( {
		id: 'OVERRIDE_THIS',
		textHeader: 'Melding',
		textBody: '',
		textClose: 'Sluiten'
	}, options );
		
	var div = $('<div id="'+options.id+'" class="modal hide" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" />');
	div.append('<div class="modal-header"> <h3>'+options.textHeader+'<h3> </div>');
	div.append('<div class="modal-body"> <p>'+options.textBody+'</p> </div>');
		
	var footer = $('<div class="modal-footer"/>');
	for(var i=0; i<options.buttons.length; i++){
		footer.append('<button class="btn btn-primary button'+i+'">'+options.buttons[i][0]+'</button>');
		$('body').on('click', '.button'+i, options.buttons[i][1] );
	}		
	footer.append('<button class="btn" data-dismiss="modal">'+options.textClose+'</button>');
	footer.appendTo(div);
	
	$('#' + options.id).length == 0 ? $('body').append(div) : $('body').find('#'+options.id).replaceWith(div);	
		
	$(div).modal('show');
}