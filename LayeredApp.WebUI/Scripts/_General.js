function log(x) {
	if (window.console) console.log(x);
}


var H = {};  //Helpers;

H.extendBefore = function (context, funcBefore, func) {
	return function () {
		funcBefore();
		func.apply(context, arguments);
	}
}
H.extendAfter = function (context, funcAfter, func) {
	return function () {
		func.apply(context, arguments);
		funcAfter();
	}
}


function AddjQueryPlugin(name, func) {
	if ($.fn[name] === undefined) {
		$.fn[name] = func;
	} else {
		throw "jQuery plug-in already exists;";
	}
}
function AddjQueryMethod(name, func) {
	if ($[name] === undefined) {
		$[name] = func;
	} else {
		throw "jQuery method already exists;";
	}
}

AddjQueryMethod('Json', function (url, objToServer, onSuccess, onError, onComplete) {
	$.ajax({
		type: 'POST',
		contentType: 'application/json',
		url: url,
		data: JSON.stringify(objToServer),
		success: onSuccess,
		error: onError,
		complete: onComplete
	});
});

