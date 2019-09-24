
$(function () {



    var link = document.getElementById("downloadToExcel").getAttribute('href');
    var dataFrom;
    var dataTo;
    var hrefLink;
    var ancorElem = document.getElementById("downloadToExcel");
    // Prevent default event la click link raportare daca nu e introdusa data
    ancorElem.addEventListener("click", function (e) {
        if (dataFrom === undefined || dataTo === undefined) {
            alert("Te rog selecteaza perioada de raportare.");
            e.preventDefault();
        }

    });

    //Date time picker options
    var dateFormat = "dd.mm.yy",
        from = $("#from")
            .datepicker({
                defaultDate: "+0w",
                changeMonth: true,
                numberOfMonths: 2
            })
            .on("change", function () {
                to.datepicker("option", "minDate", getDate(this));
                //
                console.log("S-a schimbat data la from");
                dataFrom = document.getElementById("from").value;
                hrefLink = link + "?dataFrom=" + dataFrom + "&dataTo=" + dataTo;
                console.log(hrefLink);
                document.getElementById("downloadToExcel").setAttribute('href', hrefLink);
            }),
        to = $("#to").datepicker({
            defaultDate: "+0w",
            changeMonth: true,
            numberOfMonths: 2   
        })
            .on("change", function () {
                from.datepicker("option", "maxDate", getDate(this));
                //
                console.log("S-a schimbat data la to");
                dataTo = document.getElementById("to").value;
                hrefLink = link + "?dataFrom=" + dataFrom + "&dataTo=" + dataTo;
                console.log(hrefLink);
                document.getElementById("downloadToExcel").setAttribute('href', hrefLink);
            });

    function getDate(element) {
        var date;
        try {
            date = $.datepicker.parseDate(dateFormat, element.value);
        } catch (error) {
            date = null;
        }

        return date;
    }

    // Set format of data
    $("#from").datepicker("option", "dateFormat", "dd.mm.yy");
    $("#to").datepicker("option", "dateFormat", "dd.mm.yy");

});
