
var Table = function () {
	return {

		//Fields:

		Description: 'Table object',
		Url: null,
		Parent: null,
		MaxItemsOnPage: 10,
		PageNumber: 1,
		ColumnInfo: [
			{ PropertyName: 'A1', Header: 'A-1' },
			{ PropertyName: 'B1', Header: 'B-1' }
		],
		Data: [
			['a1', 'a2'],
			['b1', 'b2']
		]

		//Methods:

		, Init: function (Configuration) {
			//TODO defaults extenden ipv overschrijven?
			this.Url = Configuration.Url;
			this.Parent = Configuration.Parent;
			this.ColumnInfo = Configuration.ColumnInfo;
			this.MaxItemsOnPage = Configuration.MaxItemsOnPage;
			this.PageNumber = Configuration.PageNumber;
			this.RenderTable();
			this.Refresh();
		}
		, Refresh: function (callbackSuccess) {
			var that = this;
			$.Json(this.Url, this.GetObjForServer(),
				function (objResponse) {
					that.Data = objResponse.Items;
					that.RenderTableBody();
				});
		}
		, RenderTable: function () {

			this.ElmTable = $('<table class="table"></table>').appendTo(this.Parent);
			this.ElmHead = $('<thead/>')
				.append($('<tr/>').append($.map(this.ColumnInfo, function (elm, index) { return $('<th>').text(elm.Header); })))
				.appendTo(this.ElmTable);
			this.ElmBody = $('<tbody/>').appendTo(this.ElmTable);
		}
		, RenderTableBody: function () {
			this.ElmBody.empty();

			var that = this;
			$.each(this.Data, function (indexRow, arrayRow) {
				var newRow = $('<tr/>').appendTo(that.ElmBody);
				$.each(arrayRow, function (indexColumn, columnValue) {
					newRow.append('<td>' + that.Data[indexRow][indexColumn] + '</td>');
				});
			});

			//..Refresh extra controls;
		}
		//-----------------------
		, InitDemo: function () {
			this.Parent = $('body');
			this.RenderTable();
			this.RenderTableBody();
		}
		, GetObjForServer: function () {
			return {
				MaxItemsOnPage: this.MaxItemsOnPage,
				PageNumber: this.PageNumber,
				TableColumns: this.ColumnInfo
			};
		}

	}

} ();

var CustomTableExt = function () {

	

	return $.extend(Table,
	{
		GetObjForServer: function () {
			return {
				MaxItemsOnPage: this.MaxItemsOnPage,
				PageNumber: this.PageNumber,
				TableColumns: this.ColumnInfo
			};
		}
	});


} ();

//NB: no jQuery plug-in was created because of easier inheritance and flexibility;

//hoeveel nut heeft de function(){}() hier?
//TODO remove from global namespace?
//TODO is het gebruik van directe properties zoals Parent handig&veilig genoeg?
