function updateStockTable() {
    $(document).ready(function () {
        $("#ticker").autocomplete({
            source: '@Url.Action("Autocomplete")'
        });
        $("#get-stock-data").click(function () {
            var sym = $("#ticker").val();
            console.log(sym);
            var baseApiUri = "https://query.yahooapis.com/v1/public/yql";
            var queryUri = encodeURIComponent("select * from yahoo.finance.quotes where symbol in ('" + sym + "')");
            var formatUri = "&format=json&diagnostics=true&env=http://datatables.org/alltables.env"

            $.getJSON(baseApiUri, "q=" + queryUri + formatUri).done(function (data) {
                console.log(data);
                $("#table-ticker").text(data.query.results.quote.Symbol);
                $("#table-price").text(data.query.results.quote.LastTradePriceOnly);
                $("#table-change").text(data.query.results.quote.PercentChange);
                $("#table-stock-name").text(data.query.results.quote.Name);
            }).fail(function (jqxhr, textStatus, error) {
                var err = textStatus + ", " + error;
                console.log(error);
            });
        });
    });
}
    