

$(document).ready(function () {
    console.log("Ruleaza Java Script");

    //// Variabile comenzi BackGroundService
    // Buton start BackGroundService
    let btnStartBackgroundService = document.getElementById("btnStartBackgroundService");
    // Buton stop BackGroundService
    let btnStopBackgroundService = document.getElementById("btnStopBackgroundService");
    // Text stare Background service
    let textIsBackgroundServiceStarted = document.getElementById("textIsBackgroundServiceStarted");

    //// Variabile comenzi Plc Cuptor
    // Buton Create Plc Cuptor
    let btnCreatePlcCuptor = document.getElementById("btnCreatePlcCuptor");
    // Buton Delete Plc Cuptor
    let btnDeletePlcCuptor = document.getElementById("btnDeletePlcCuptor");
    // Buton Connect Plc Cuptor
    let btnConnectPlcCuptor = document.getElementById("btnConnectPlcCuptor");
    // Buton Deconnect Plc Cuptor
    let btnDeconnectPlcCuptor = document.getElementById("btnDeconnectPlcCuptor");
    // Buton Check connection Plc Cuptor
    let btnIsConnectedPlcCuptor = document.getElementById("btnIsConnectedPlcCuptor");

    //// Variabile comenzi Plc Gadda F4        
    // Buton Create Plc Gadda F4
    let btnCreatePlcGaddaF4 = document.getElementById("btnCreatePlcGaddaF4");
    // Buton Delete Plc Gadda F4
    let btnDeletePlcGaddaF4 = document.getElementById("btnDeletePlcGaddaF4");
    // Buton Connect Plc Gadda F4
    let btnConnectPlcGaddaF4 = document.getElementById("btnConnectPlcGaddaF4");
    // Buton Deconnect Plc Gadda F4
    let btnDeconnectPlcGaddaF4 = document.getElementById("btnDeconnectPlcGaddaF4");
    // Buton Check connection Plc Gadda F4
    let btnIsConnectedPlcGaddaF4 = document.getElementById("btnIsConnectedPlcGaddaF4");

    //// Variabile comenzi Setare lista mail si ora raportare Cuptor    
    // Buton comanda setare
    let btnSetareMailCuptor = document.getElementById("btnSetareMailCuptor");
    // TextBox Lista mail
    let textBoxListaMailCuptor = document.getElementById("textBoxListaMailCuptor");
    // TextBox Ora raportare
    let textBoxOraRaportCuptor = document.getElementById("textBoxOraRaportCuptor");

    //// Variabile comenzi Setare lista mail si ora raportare Gadda    
    // Buton comanda setare
    let btnSetareMailGadda = document.getElementById("btnSetareMailGadda");
    // TextBox Lista mail
    let textBoxListaMailGadda = document.getElementById("textBoxListaMailGadda");
    // TextBox Ora raportare
    let textBoxOraRaportGadda = document.getElementById("textBoxOraRaportGadda");

    //// Variabile Index si consum cuptor & Gadda
    let textIndexCuptorUltim = document.getElementById("textIndexCuptorUltim");
    let textConsumCuptorUltim = document.getElementById("textConsumCuptorUltim");
    let textIndexGaddaF2Ultim = document.getElementById("textIndexGaddaF2Ultim");
    let textConsumGaddaF2Ultim = document.getElementById("textConsumGaddaF2Ultim");
    let textIndexGaddaF4Ultim = document.getElementById("textIndexGaddaF4Ultim");
    let textConsumGaddaF4Ultim = document.getElementById("textConsumGaddaF4Ultim");




    // Add a class btnStartBackgroundService.classList.add("btn-danger")
    // btnStartBackgroundService.classList.toggle("btn-success");
    // Clase buton start si stop btn-danger btn-success
    var btnStop = document.getElementById("btnStop");
    var btnStart = document.getElementById("btnStart");
    // Buton Stop Click event
    btnStop.addEventListener('click', function () {
        alert("s-a apasat buton stop");
        $.ajax({
            url: " /Home/StopBackGroundServiceAsync",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat Stop Async din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Stop Async");
            }

        });
    });
    // Buton Start Click event
    btnStart.addEventListener('click', function () {
        alert("s-a apasat buton start");
        $.ajax({
            url: " /Home/StartBackGroundServiceAsync",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat Start Async din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Start Async");
            }

        });
    });
});
