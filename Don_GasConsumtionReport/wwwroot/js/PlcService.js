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

    //// Variabile comenzi Plc Gadda F2        
    // Buton Create Plc Gadda F2
    let btnCreatePlcGaddaF2 = document.getElementById("btnCreatePlcGaddaF2");
    // Buton Delete Plc Gadda F2
    let btnDeletePlcGaddaF2 = document.getElementById("btnDeletePlcGaddaF2");
    // Buton Connect Plc Gadda F2
    let btnConnectPlcGaddaF2 = document.getElementById("btnConnectPlcGaddaF2");
    // Buton Deconnect Plc Gadda F2
    let btnDeconnectPlcGaddaF2 = document.getElementById("btnDeconnectPlcGaddaF2");
    // Buton Check connection Plc Gadda F2
    let btnCheckIpPlcGaddaF2 = document.getElementById("btnCheckIpPlcGaddaF2");

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
    let btnCheckIpPlcGaddaF4 = document.getElementById("btnCheckIpPlcGaddaF4");

    //// Variabile comenzi Setare lista mail si ora raportare Cuptor    
    // Buton comanda setare
    let btnSetareMailCuptor = document.getElementById("btnSetareMailCuptor");
    // Buton comanda afisare
    let btnAfisareMailCuptor = document.getElementById("btnAfisareMailCuptor");
    // TextBox Lista mail
    let textBoxListaMailCuptor = document.getElementById("textBoxListaMailCuptor");
    // TextBox Ora raportare
    let textBoxOraRaportCuptor = document.getElementById("textBoxOraRaportCuptor");
    // Text Block Data Ora Raport Efectuat
    let textOraDataRaportFacutCuptor = document.getElementById("textOraDataRaportFacutCuptor");

    //// Variabile comenzi Setare lista mail si ora raportare Gadda    
    // Buton comanda setare
    let btnSetareMailGadda = document.getElementById("btnSetareMailGadda");
    // TextBox Lista mail
    let textBoxListaMailGadda = document.getElementById("textBoxListaMailGadda");
    // TextBox Ora raportare
    let textBoxOraRaportGadda = document.getElementById("textBoxOraRaportGadda");
    // Text Block Data Ora Raport Efectuat
    let textOraDataRaportFacutGaddaF2 = document.getElementById("textOraDataRaportFacutGaddaF2");
    // Text Block Data Ora Raport Efectuat
    let textOraDataRaportFacutGaddaF4 = document.getElementById("textOraDataRaportFacutGaddaF4");
    //// Variabile Index si consum cuptor & Gadda
    let textIndexCuptorUltim = document.getElementById("textIndexCuptorUltim");
    let textConsumCuptorUltim = document.getElementById("textConsumCuptorUltim");
    let textIndexGaddaF2Ultim = document.getElementById("textIndexGaddaF2Ultim");
    let textConsumGaddaF2Ultim = document.getElementById("textConsumGaddaF2Ultim");
    let textIndexGaddaF4Ultim = document.getElementById("textIndexGaddaF4Ultim");
    let textConsumGaddaF4Ultim = document.getElementById("textConsumGaddaF4Ultim");
    let textBlockDataOraRaportFacut

    // Refresh parameters
    setInterval(function () {
        // Functie actualizare parametri
        $.ajax({
            url: " /Home/UpdateParameters",
            type: 'GET',
            success: function (response) {
                //console.log('proba proba');
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
                // Functie Actualizare Culoare butoane Creare/ Stergere Plc GaddaF2
                BtnsPlcCreationStatusGaddaF2(response.isCreatedPlcGaddaF2);
                // Functie Actualizare Culoare butoane Connect/ Deconnect Plc GaddaF2
                BtnsPlcConnectionStatusGaddaF2(response.isConnectedPlcGaddaF2);
                // Functie Actualizare Culoare butoane Creare/ Stergere Plc GaddaF4
                BtnsPlcCreationStatusGaddaF4(response.isCreatedPlcGaddaF4);
                // Functie Actualizare Culoare butoane Connect/ Deconnect Plc GaddaF4
                BtnsPlcConnectionStatusGaddaF4(response.isConnectedPlcGaddaF4);
                // Actualizare ultimele valori Index citite
                console.log("Index cuptor: " + response.textBlockIndexCuptor.toString());
                //console.log("Raspuns: " + "Ce vreau eu");
                //console.log("cveva ceva ceva xeva");
                textIndexCuptorUltim.innerHTML = response.textBlockIndexCuptor.toString();
                textIndexGaddaF2Ultim.innerHTML = response.textBlockIndexGaddaF2.toString();
                textIndexGaddaF4Ultim.innerHTML = response.textBlockIndexGaddaF4.toString();
                textConsumCuptorUltim.innerHTML = response.textBlockConsumCuptor.toString();
                textConsumGaddaF2Ultim.innerHTML = response.textBlockConsumGaddaF2.toString();
                textConsumGaddaF4Ultim.innerHTML = response.textBlockConsumGaddaF4.toString();
                textOraDataRaportFacutCuptor.innerHTML = response.textBlockDataOraRaportFacut.toString();
                textOraDataRaportFacutGaddaF2.innerHTML = response.textBlockDataOraRaportFacut.toString();
                textOraDataRaportFacutGaddaF4.innerHTML = response.textBlockDataOraRaportFacut.toString();

                //response.textBlockDataOraRaportFacut.toString();

                // Refresh pagina site pentru a nu pierde valori
                var currentdate = new Date();
                if (currentdate.getMinutes().toString().substring(1, 2) === "5" && currentdate.getSeconds() === 0)
                    location.reload(true); // Refresh pagina (de pe server) la fiecare 10 minute 
            },
            error: function (response) {
                console.log("Nu a mers call functie UpdateParameters");
            }

        });
        
        // refresh pagina
        //location.reload();
    }, 1000);


});

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

// Event Listeners butoane GaddaF2
// Buton Create Plc GaddaF2 Click event
btnCreatePlcGaddaF2.addEventListener('click', function () {
    //alert("s-a apasat buton start");
    $.ajax({
        url: " /Home/CreatePlcGaddaF2",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Create Plc GaddaF2 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Create Plc GaddaF2");
        }
    });
});
// Buton Delete Plc GaddaF2 Click event
btnDeletePlcGaddaF2.addEventListener('click', function () {
    $.ajax({
        url: " /Home/DeletePlcGaddaF2",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Delete plc GaddaF2 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Delete Plc GaddaF2");
        }
    });
});

// Buton Connect Plc GaddaF2 Click event
btnConnectPlcGaddaF2.addEventListener('click', function () {
    //alert("s-a apasat buton start");
    $.ajax({
        url: " /Home/ConnectPlcGaddaF2",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Connect Plc GaddaF2 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Connect Plc GaddaF2");
        }
    });
});
// Buton Deconnect Plc GaddaF2 Click event
btnDeconnectPlcGaddaF2.addEventListener('click', function () {
    $.ajax({
        url: " /Home/DeconnectPlcGaddaF2",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Deconnect plc GaddaF2 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Deconnect Plc GaddaF2");
        }
    });
});
// Buton Check Ip Plc GaddaF2 Click event
btnCheckIpPlcGaddaF2.addEventListener('click', function () {
    $.ajax({
        url: " /Home/CheckIpPlcGaddaF2",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat CheckIP plc GaddaF2 din buton");
            //console.log(response);
            // Verificam daca Plc-ul este conectat
            if (response) {
                // Verificam daca deja butonul indica ca serviciul e pornit
                // Daca nu indica, ii adaugam clasa ca sa indice
                if (!btnCheckIpPlcGaddaF2.classList.contains("btn-success")) {
                    if (btnCheckIpPlcGaddaF2.classList.contains("btn-info"))
                        btnCheckIpPlcGaddaF2.classList.remove("btn-info");
                    btnCheckIpPlcGaddaF2.classList.add("btn-success");
                }// Verificam daca butonul stop indica ca este oprit
                // Daca este il facem sa nu mai indice
                if (btnCheckIpPlcGaddaF2.classList.contains("btn-danger"))
                    btnCheckIpPlcGaddaF2.classList.remove("btn-danger");
            } else {
                if (btnCheckIpPlcGaddaF2.classList.contains("btn-success"))
                    btnCheckIpPlcGaddaF2.classList.remove("btn-succes");
                if (btnCheckIpPlcGaddaF2.classList.contains("btn-info"))
                    btnCheckIpPlcGaddaF2.classList.remove("btn-info");
                if (!btnCheckIpPlcGaddaF2.classList.contains("btn-danger"))
                    btnCheckIpPlcGaddaF2.classList.add("btn-danger");
            }
        },
        error: function (response) {
            console.log("Nu a mers btn CheckIP Plc GaddaF2");
        }
    });
});

// Event Listeners butoane GaddaF4
// Buton Create Plc GaddaF4 Click event
btnCreatePlcGaddaF4.addEventListener('click', function () {
    //alert("s-a apasat buton start");
    $.ajax({
        url: " /Home/CreatePlcGaddaF4",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Create Plc GaddaF4 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Create Plc GaddaF4");
        }
    });
});
// Buton Delete Plc GaddaF4 Click event
btnDeletePlcGaddaF4.addEventListener('click', function () {
    $.ajax({
        url: " /Home/DeletePlcGaddaF4",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Delete plc GaddaF4 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Delete Plc GaddaF4");
        }
    });
});

// Buton Connect Plc GaddaF4 Click event
btnConnectPlcGaddaF4.addEventListener('click', function () {
    //alert("s-a apasat buton start");
    $.ajax({
        url: " /Home/ConnectPlcGaddaF4",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Connect Plc GaddaF4 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Connect Plc GaddaF4");
        }
    });
});
// Buton Deconnect Plc GaddaF4 Click event
btnDeconnectPlcGaddaF4.addEventListener('click', function () {
    $.ajax({
        url: " /Home/DeconnectPlcGaddaF4",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat Deconnect plc GaddaF4 din buton");
        },
        error: function (response) {
            console.log("Nu a mers btn Deconnect Plc GaddaF4");
        }
    });
});
// Buton Check Ip Plc GaddaF2 Click event
btnCheckIpPlcGaddaF4.addEventListener('click', function () {
    $.ajax({
        url: " /Home/CheckIpPlcGaddaF4",
        type: 'GET',
        success: function (response) {
            console.log("S-a realizat CheckIP plc GaddaF4 din buton");
            //console.log(response);
            // Verificam daca Plc-ul este conectat
            if (response) {
                // Verificam daca deja butonul indica ca serviciul e pornit
                // Daca nu indica, ii adaugam clasa ca sa indice
                if (!btnCheckIpPlcGaddaF4.classList.contains("btn-success")) {
                    if (btnCheckIpPlcGaddaF4.classList.contains("btn-info"))
                        btnCheckIpPlcGaddaF4.classList.remove("btn-info");
                    btnCheckIpPlcGaddaF4.classList.add("btn-success");
                }// Verificam daca butonul stop indica ca este oprit
                // Daca este il facem sa nu mai indice
                if (btnCheckIpPlcGaddaF4.classList.contains("btn-danger"))
                    btnCheckIpPlcGaddaF4.classList.remove("btn-danger");
            } else {
                if (btnCheckIpPlcGaddaF4.classList.contains("btn-success"))
                    btnCheckIpPlcGaddaF4.classList.remove("btn-succes");
                if (btnCheckIpPlcGaddaF4.classList.contains("btn-info"))
                    btnCheckIpPlcGaddaF4.classList.remove("btn-info");
                if (!btnCheckIpPlcGaddaF4.classList.contains("btn-danger"))
                    btnCheckIpPlcGaddaF4.classList.add("btn-danger");
            }
        },
        error: function (response) {
            console.log("Nu a mers btn CheckIP Plc GaddaF4");
        }
    });
});

// Setare si afisare ListaMail OraRaport Plc Cuptor
// Buton Set ListaMail si OraRaport Plc Cuptor Click event
btnSetareMailCuptor.addEventListener('click', function () {
    //let myTransferData = "listaMail=" + textBoxListaMailCuptor.innerHTML + "&oraRaport=" +
    //    textBoxOraRaportCuptor.innerHTML;
    let myTransferData = {
        listaMail: textBoxListaMailCuptor.value,
        oraRAport: textBoxOraRaportCuptor.value
    };
    console.log(myTransferData);
    $.ajax({
        url: " /Home/SetListaMailOraRaportPlcCuptor",
        type: 'POST',
        data: myTransferData,
        success: function (response) {
            console.log("S-a realizat setare mail plc Cuptor din buton");
            // Setare Text box mail and ora raport cutpr and gadda
            textBoxListaMailCuptor.value = response.lista;
            textBoxOraRaportCuptor.value = response.ora;
            //console.log(response.lista.toString()) + "    " + response.ora.toString();
        },
        error: function (response) {
            console.log("Nu a mers btn setare mail Plc Cuptor");
        }
    });
});

// Buton Afisare ListaMail si OraRaport Plc Cuptor Click event
btnAfisareMailCuptor.addEventListener('click', function () {
    //let myTransferData = "listaMail=" + textBoxListaMailCuptor.innerHTML + "&oraRaport=" +
    //    textBoxOraRaportCuptor.innerHTML;    
    //console.log(myTransferData);
    $.ajax({
        url: " /Home/ShowListaMailOraRaportPlcCuptor",
        type: 'GET',
        //data: myTransferData,
        success: function (response) {
            console.log("S-a realizat afisare mail plc Cuptor din buton");
            // Setare Text box mail and ora raport cutpr and gadda
            textBoxListaMailCuptor.value = response.lista;
            textBoxOraRaportCuptor.value = response.ora;
            //console.log(response.lista.toString()) + "    " + response.ora.toString();
        },
        error: function (response) {
            console.log("Nu a mers btn afisare mail Plc Cuptor");
        }
    });
});

// Setare si afisare ListaMail OraRaport Plc Gadda
// Buton Set ListaMail si OraRaport Plc Gadda Click event
btnSetareMailGadda.addEventListener('click', function () {
    //let myTransferData = "listaMail=" + textBoxListaMailCuptor.innerHTML + "&oraRaport=" +
    //    textBoxOraRaportCuptor.innerHTML;
    let myTransferData = {
        listaMail: textBoxListaMailGadda.value,
        oraRAport: textBoxOraRaportGadda.value
    };
    console.log(myTransferData);
    $.ajax({
        url: " /Home/SetListaMailOraRaportPlcGadda",
        type: 'POST',
        data: myTransferData,
        success: function (response) {
            console.log("S-a realizat setare mail plc Gadda din buton");
            // Setare Text box mail and ora raport cutpr and gadda
            textBoxListaMailGadda.value = response.lista;
            textBoxOraRaportGadda.value = response.ora;
            //console.log(response.lista.toString()) + "    " + response.ora.toString();
        },
        error: function (response) {
            console.log("Nu a mers btn setare mail Plc Gadda");
        }
    });
});

// Buton Afisare ListaMail si OraRaport Plc Gadda Click event
btnAfisareMailGadda.addEventListener('click', function () {
    //let myTransferData = "listaMail=" + textBoxListaMailCuptor.innerHTML + "&oraRaport=" +
    //    textBoxOraRaportCuptor.innerHTML;    
    //console.log(myTransferData);
    $.ajax({
        url: " /Home/ShowListaMailOraRaportPlcGadda",
        type: 'GET',
        //data: myTransferData,
        success: function (response) {
            console.log("S-a realizat afisare mail plc Gadda din buton");
            // Setare Text box mail and ora raport cutpr and gadda
            textBoxListaMailGadda.value = response.lista;
            textBoxOraRaportGadda.value = response.ora;
            //console.log(response.lista.toString()) + "    " + response.ora.toString();
        },
        error: function (response) {
            console.log("Nu a mers btn afisare mail Plc Gadda");
        }
    });
});

// BACKGROUNDSERVICE
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

// PLC CUPTOR
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

// PLC GADDAF2
// Functie setare culoare background btns Creare/ stergere Plc GaddaF2
function BtnsPlcCreationStatusGaddaF2(isCreatedPlcGaddaF2) {
    // Verificam daca serviciu este pornit
    if (isCreatedPlcGaddaF2) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnCreatePlcGaddaF2.classList.contains("btn-success"))
            btnCreatePlcGaddaF2.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnDeletePlcGaddaF2.classList.contains("btn-danger"))
            btnDeletePlcGaddaF2.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnCreatePlcGaddaF2.classList.contains("btn-success"))
            btnCreatePlcGaddaF2.classList.remove("btn-success");
        if (!btnDeletePlcGaddaF2.classList.contains("btn-danger"))
            btnDeletePlcGaddaF2.classList.add("btn-danger");
    }
}

// Functie setare culoare background btns Connect/ Deconnect Plc GaddaF2
function BtnsPlcConnectionStatusGaddaF2(isConnectedPlcGaddaF2) {
    // Verificam daca Plc-ul este conectat
    if (isConnectedPlcGaddaF2) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnConnectPlcGaddaF2.classList.contains("btn-success"))
            btnConnectPlcGaddaF2.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnDeconnectPlcGaddaF2.classList.contains("btn-danger"))
            btnDeconnectPlcGaddaF2.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnConnectPlcGaddaF2.classList.contains("btn-success"))
            btnConnectPlcGaddaF2.classList.remove("btn-success");
        if (!btnDeconnectPlcGaddaF2.classList.contains("btn-danger"))
            btnDeconnectPlcGaddaF2.classList.add("btn-danger");
    }
}

// PLC GADDAF4
// Functie setare culoare background btns Creare/ stergere Plc GaddaF4
function BtnsPlcCreationStatusGaddaF4(isCreatedPlcGaddaF4) {
    // Verificam daca serviciu este pornit
    if (isCreatedPlcGaddaF4) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnCreatePlcGaddaF4.classList.contains("btn-success"))
            btnCreatePlcGaddaF4.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnDeletePlcGaddaF4.classList.contains("btn-danger"))
            btnDeletePlcGaddaF4.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnCreatePlcGaddaF4.classList.contains("btn-success"))
            btnCreatePlcGaddaF4.classList.remove("btn-success");
        if (!btnDeletePlcGaddaF4.classList.contains("btn-danger"))
            btnDeletePlcGaddaF4.classList.add("btn-danger");
    }
}

// Functie setare culoare background btns Connect/ Deconnect Plc GaddaF4
function BtnsPlcConnectionStatusGaddaF4(isConnectedPlcGaddaF4) {
    // Verificam daca Plc-ul este conectat
    if (isConnectedPlcGaddaF4) {
        // Verificam daca deja butonul indica ca serviciul e pornit
        // Daca nu indica, ii adaugam clasa ca sa indice
        if (!btnConnectPlcGaddaF4.classList.contains("btn-success"))
            btnConnectPlcGaddaF4.classList.add("btn-success");
        // Verificam daca butonul stop indica ca este oprit
        // Daca este il facem sa nu mai indice
        if (btnDeconnectPlcGaddaF4.classList.contains("btn-danger"))
            btnDeconnectPlcGaddaF4.classList.remove("btn-danger");
    }
    else { // La fel si daca nu merge serviciul
        if (btnConnectPlcGaddaF4.classList.contains("btn-success"))
            btnConnectPlcGaddaF4.classList.remove("btn-success");
        if (!btnDeconnectPlcGaddaF4.classList.contains("btn-danger"))
            btnDeconnectPlcGaddaF4.classList.add("btn-danger");
    }
}


