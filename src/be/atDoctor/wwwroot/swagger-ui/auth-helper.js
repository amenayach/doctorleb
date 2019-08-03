(function () {
	$(function () {
	    var basicAuthUI =
	        '<div class="input">Login &nbsp; <input placeholder="username" id="input_username" name="username" type="text">' +
	            '&nbsp;<input placeholder="password" id="input_password" name="password" type="password">' +
	            '&nbsp;<input id="btnlogin" name="btnlogin" type="button" value="Login"></div>' +
	            '<div class="input" id="dvimg"><img src="/swagger/images/throbber.gif"></div>' +
	            '<div class="input" id="dvtoken"><input style="display:inline-block;width:100%;" disabled="disabled" placeholder="token" id="tbtoken" name="tbtoken" type="text"></div>' +
                '<br><input type="button" title="Ctrl + /" style="cursor: pointer; padding: 12px; border-radius: 22px; background-color: #89bf04; color: #fff; font-weight: bold; font-size: 14px; text-shadow: 0px 1px 0px #2f6627; border: #89bf04 1px solid;" value="Collapse all" data-exp="1" id="btnexpander">';
		$(basicAuthUI).insertAfter('.info_title'); // '#api_selector div.input:last-child');
		$("#input_apiKey").hide();
		$("#dvtoken").hide();
		$("#dvimg").hide();

		var logdataJson = getLs();
		$("#input_username").val(logdataJson.username);
		$("#input_password").val(logdataJson.password);
        $('#btnlogin').click(addAuthorization);
        $('#btnexpander').click(expandIt);

	    window.addEventListener('keydown',
	        function(ev) {
	            if (ev.ctrlKey && ev.keyCode === 191) {
	                expandIt();
	            }
	        });
	});

    function expandIt() {
        var exp = $('#btnexpander');
        var doExp = exp.attr('data-exp') === '1';
        exp.val(doExp ? 'Expand all' : 'Collapse all');
        jQuery('ul.endpoints').css("display", doExp ? "none" : 'block');
        exp.attr('data-exp', doExp ? '0' : '1');
    }

	function addAuthorization() {
		var username = $('#input_username').val();
		var password = $('#input_password').val();
		if (username && username.trim() != "" && password && password.trim() != "") {

			$("#dvimg").show();
			$.ajax({
				url: '/api/account/login',
				type: 'POST',
                data: JSON.stringify({ "email": username, "password": password, "rememberMe": true }),
				contentType: "application/json;charset=utf-8",
				dataType: "json",
				success: function (data) {
					$("#dvimg").hide();
					$("#dvtoken").show();
					$("#tbtoken").val(data.token);
					saveLs({ username: username, password: password });
					//var authHeader = new SwaggerClient.ApiKeyAuthorization("Authorization", "bearer " + data.token, "header");
					//window.swaggerUi.api.clientAuthorizations.add("jwt", authHeader);
					console.log(data.token);
				},
				error: function (x, y, z) {
					console.log(x);
					$("#dvimg").hide();
					$("#tbtoken").val('');
					window.swaggerUi.api.clientAuthorizations.remove("jwt");
				}
			});
			
		}
	}

	function saveLs(obj) {
		window.localStorage.setItem('logdata',JSON.stringify(obj));
	}

	function getLs() {
		var logdataString = window.localStorage.getItem('logdata');
		return logdataString && logdataString.length > 1 ? JSON.parse(logdataString) : {username: '', password: ''};
	}
})();