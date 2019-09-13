$(document).ready(function () {
    console.log("Ruleaza Java Script");

    //// Variabile comenzi BackGroundService
    // Buton start BackGroundService
    let btnStartBackgroundService = document.getElementById("btnStartBackgroundService");
    // Buton stop BackGroundService
    let btnStopBackgroundService = document.getElementById("btnStopBackgroundService");
    // Text stare Background service
    let textIsBackgroundServiceStarted = document.getElementById("textIsBackgroundServiceStarted");
    // Text Ceas
    let textCeas = document.getElementById("ceas");

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
    let btnCheckIpPlcCuptor = document.getElementById("btnCheckIpPlcCuptor");

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

    // Buton Stop BackgroundService Click event
    btnStopBackgroundService.addEventListener('click', function () {
        //alert("s-a apasat buton stop");
        $.ajax({
            url: " /Home/StopBackGroundServiceAsync",
            type: 'GET',
            success: function (response) {
                //console.log("S-a realizat Stop Async din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Stop background Service");
            }

        });
    });
    // Buton Start BackgroundService Click event
    btnStartBackgroundService.addEventListener('click', function () {
        //alert("s-a apasat buton start");
        $.ajax({
            url: " /Home/StartBackGroundServiceAsync",
            type: 'GET',
            success: function (response) {
                //console.log("S-a realizat Start Async din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Start background service Async");
            }
        });
    });
    // Buton Create Plc Cuptor Click event
    btnCreatePlcCuptor.addEventListener('click', function () {
        //alert("s-a apasat buton start");
        $.ajax({
            url: " /Home/CreatePlcCuptor",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat Create Plc Cuptor din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Create Plc Cuptor");
            }
        });
    });
    // Buton Delete Plc Cuptor Click event
    btnDeletePlcCuptor.addEventListener('click', function () {
        $.ajax({
            url: " /Home/DeletePlcCuptor",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat Delete plc Cuptor din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Delete Plc Cuptor");
            }
        });
    });
    // Buton Connect Plc Cuptor Click event
    btnConnectPlcCuptor.addEventListener('click', function () {
        //alert("s-a apasat buton start");
        $.ajax({
            url: " /Home/ConnectPlcCuptor",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat Connect Plc Cuptor din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Connect Plc Cuptor");
            }
        });
    });
    // Buton Deconnect Plc Cuptor Click event
    btnDeconnectPlcCuptor.addEventListener('click', function () {
        $.ajax({
            url: " /Home/DeconnectPlcCuptor",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat Deconnect plc Cuptor din buton");
            },
            error: function (response) {
                console.log("Nu a mers btn Deconnect Plc Cuptor");
            }
        });
    });
    // Buton Check Ip Plc Cuptor Click event
    btnCheckIpPlcCuptor.addEventListener('click', function () {
        $.ajax({
            url: " /Home/CheckIpPlcCuptor",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat CheckIP plc Cuptor din buton");
                //console.log(response);
                // Verificam daca Plc-ul este conectat
                if (response) {
                    // Verificam daca deja butonul indica ca serviciul e pornit
                    // Daca nu indica, ii adaugam clasa ca sa indice
                    if (!btnCheckIpPlcCuptor.classList.contains("btn-success")) {
                        if (btnCheckIpPlcCuptor.classList.contains("btn-info"))
                            btnCheckIpPlcCuptor.classList.remove("btn-info");
                        btnCheckIpPlcCuptor.classList.add("btn-success");
                    }// Verificam daca butonul stop indica ca este oprit
                    // Daca este il facem sa nu mai indice
                    if (btnCheckIpPlcCuptor.classList.contains("btn-danger"))
                        btnCheckIpPlcCuptor.classList.remove("btn-danger");
                } else {
                    if (btnCheckIpPlcCuptor.classList.contains("btn-success"))
                        btnCheckIpPlcCuptor.classList.remove("btn-succes");
                    if (btnCheckIpPlcCuptor.classList.contains("btn-info"))
                        btnCheckIpPlcCuptor.classList.remove("btn-info");
                    if (!btnCheckIpPlcCuptor.classList.contains("btn-danger"))
                        btnCheckIpPlcCuptor.classList.add("btn-danger");
                }
            },
            error: function (response) {
                console.log("Nu a mers btn CheckIP Plc Cuptor");
            }
        });
    });
    // Refresh parameters
    setInterval(function () {
        // Functie actualizare parametri
        $.ajax({
            url: " /Home/UpdateParameters",
            type: 'GET',
            success: function (response) {
                console.log("S-a realizat call functie UpdateParameters");
                // Actualizare text IsBackgroundServiceStarted
                textIsBackgroundServiceStarted.innerHTML = response.isStartedBackgroundService.toString();
                // Actualizare Text Ceas
                textCeas.innerHTML = "Ceas: " + response.clock;
                // Functie Actualizare Culoare butoane BackgroundService Run/ Stop
                BtnsBackgroundServiceStartStatus(response.isStartedBackgroundService);
                // Functie Actualizare Culoare butoane Creare/ Stergere Plc Cuptor
                BtnsPlcCreationStatusCuptor(response.isCreatedPlcCuptor);
                // Functie Actualizare Culoare butoane Connect/ Deconnect Plc Cuptor
                BtnsPlcConnectionStatusCuptor(response.isConnectedPlcCuptor);
            },
            error: function (response) {
                console.log("Nu a mers call functie UpdateParameters");
            }

        });

        // refresh pagina
        //location.reload();
    }, 1000);


});


// Functie setare culoare background btns BackgroundService 
function BtnsBackgroundServiceStartStatus(isBackgroundServiceStarted) {
    // Verificam daca serviciu este pornit
    if (isBackgroundServiceStarted) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnStartBackgroundService.classList.contains("btn-success"))
            btnStartBackgroundService.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnStopBackgroundService.classList.contains("btn-danger"))
            btnStopBackgroundService.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnStartBackgroundService.classList.contains("btn-success"))
            btnStartBackgroundService.classList.remove("btn-success");
        if (!btnStopBackgroundService.classList.contains("btn-danger"))
            btnStopBackgroundService.classList.add("btn-danger");
    }
}

// Functie setare culoare background btns Creare/ stergere Plc Cuptor
function BtnsPlcCreationStatusCuptor(isCreatedPlcCuptor) {
    // Verificam daca serviciu este pornit
    if (isCreatedPlcCuptor) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnCreatePlcCuptor.classList.contains("btn-success"))
            btnCreatePlcCuptor.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnDeletePlcCuptor.classList.contains("btn-danger"))
            btnDeletePlcCuptor.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnCreatePlcCuptor.classList.contains("btn-success"))
            btnCreatePlcCuptor.classList.remove("btn-success");
        if (!btnDeletePlcCuptor.classList.contains("btn-danger"))
            btnDeletePlcCuptor.classList.add("btn-danger");
    }
}

// Functie setare culoare background btns Connect/ Deconnect Plc Cuptor
function BtnsPlcConnectionStatusCuptor(isConnectedPlcCuptor) {
    // Verificam daca Plc-ul este conectat
    if (isConnectedPlcCuptor) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnConnectPlcCuptor.classList.contains("btn-success"))
            btnConnectPlcCuptor.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnDeconnectPlcCuptor.classList.contains("btn-danger"))
            btnDeconnectPlcCuptor.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnConnectPlcCuptor.classList.contains("btn-success"))
            btnConnectPlcCuptor.classList.remove("btn-success");
        if (!btnDeconnectPlcCuptor.classList.contains("btn-danger"))
            btnDeconnectPlcCuptor.classList.add("btn-danger");
    }
}
