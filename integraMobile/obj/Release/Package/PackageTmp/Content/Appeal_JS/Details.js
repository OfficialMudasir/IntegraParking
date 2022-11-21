

var idNumber = "";
var arrowleft = "";
let AllDetailsIds = [];
$(document).ready(function (e) {

    AllDetailsIds = event.target.querySelectorAll("[id^=detailedTicket_]");
    if (AllDetailsIds.length > 0) {
        AllDetailsIds.forEach((ticketDetailId) => {
            $("#" + ticketDetailId.id).hide();
        });
    }
});
$(document).on('click', '[id^=detailedTicketIcon_]', function (e) {
    //debugger;
    idNumber = this.id.split('_').pop();
    var ticketId = 'Ticket_' + idNumber;
    var eyeBtnId = 'detailedTicketIcon_' + idNumber
    var detaildTicketId = 'detailedTicket_' + idNumber;
    arrowleft = 'arrowleft_' + idNumber;

    if (idNumber != null) {
        jQuery('#Ticket_' + idNumber).hide();
        jQuery('#detailedTicket_' + idNumber).addClass("TicketDetails_" + idNumber).removeClass("TicketDetails");
        jQuery('#arrowleft_' + idNumber).addClass('arrowleft_' + idNumber).removeClass("arrowleft");
        jQuery('#detailedTicket_' + idNumber).show();
    }

    // Ticket Preview
    let pdfid = 'pdfimage_' + idNumber;
    let pdfElement = $("#" + pdfid).val();
    if (pdfElement != undefined) {
        let linkSource = `data:application/octet-stream;base64,${pdfElement}`;
        fetch(linkSource)
            .then(res => res.blob())
            .then(blob => {
                let file = new File([blob], `BlinkayTicket_${idNumber}.pdf`, { type: "application/octet-stream" });

                var reader = new FileReader();
                reader.onload = function (e) {
                    showInCanvas(e.target.result);
                }
                reader.readAsDataURL(file);

                function convertDataURIToBinary(dataURI) {
                    var BASE64_MARKER = ';base64,';
                    var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
                    var base64 = dataURI.substring(base64Index);
                    var raw = window.atob(base64);
                    var rawLength = raw.length;
                    var array = new Uint8Array(new ArrayBuffer(rawLength));

                    for (i = 0; i < rawLength; i++) {
                        array[i] = raw.charCodeAt(i);
                    }
                    return array;
                }
                function showInCanvas(url) {
                    'use strict';
                    var pdfAsArray = convertDataURIToBinary(url);
                    pdfjsLib.getDocument(pdfAsArray).then(function (pdf) {
                        pdf.getPage(1).then(function (page) {
                            var scale = 1.5;
                            var viewport = page.getViewport(scale);
                            var canvas = document.getElementById('the-canvas' + idNumber);
                            var context = canvas.getContext('2d');
                            canvas.height = viewport.height;
                            canvas.width = viewport.width;
                            var renderContext = {
                                canvasContext: context,
                                viewport: viewport
                            };
                            page.render(renderContext);
                        });
                    });
                }
            });
    }
    else {
        var canvas = document.getElementById('ticketPreview' + idNumber);
        canvas.innerText = "Ticket Preview is not Available !!!";
        canvas.innerHTML.blink();
    }


});

$(document).on('click', '[id^=downloadTicketIcon_]', function () {
    debugger;
    let downloadId = this.id;
    idNumber = this.id.split('_').pop();
    let pdfid = 'pdfimage_' + idNumber;
    let pdfElement = $("#" + pdfid).val();
    let AllPdfImageIds = event.target.querySelectorAll("[id^=pdfimage_]");

    if (pdfElement != undefined) {
        let linkSource = `data:application/octet-stream;base64,${pdfElement}`;
        let downloadLink = document.createElement("a");
        downloadLink.href = linkSource;
        downloadLink.download = `BlinkayTicket_${idNumber}.pdf`;
        downloadLink.click();
       


        $('#exampleModalCenter').modal('show');
        $('.alert').removeClass("alert-danger");
        $('.alert').addClass("alert-success");
        $('#ErrorMessage').text("Ticket Copy is Downloading...");
        $('#ErrorMessage').addClass("success");
        $('#exampleModalLongTitle').hide();
        $('#btnCloseHead').hide();
        $('#btnclose').hide();
        setTimeout(function () {
            $('#exampleModalCenter').modal('hide');
        }, 5000);
        
    }
    else {
        $('#exampleModalCenter').modal('show');
        $('.alert').addClass("alert-danger");
        $('.alert').removeClass("alert-success");
        $('#ErrorMessage').text("Ticket copy is not available !");
        $('#ErrorMessage').addClass("danger");
        $('#btnCloseHead').show();

    }
});


$(document).on('click', '[id^=btnContest_]', function () {
    debugger;
    let idNumber = this.id.split('_').pop();

    //$("#TicketID_" + idNumber).val(idNumber);
    //if (!$("#searchedTicket_" + idNumber).is(":checked")) {
    //    event.preventDefault();
    //    $('#exampleModalCenter').modal('show');
    //    $('#ErrorMessage').text("Please select this ticket for the contest !");
    //}
       
})

$("#btnContest").click((e) => {
    var array = [];
    $("input:checked").each(function () {
        debugger
        array.push($(this).val());
    });

    array.forEach((item) => {
        console.log("value", item)
    })

    if (array.length > 1) {
        event.preventDefault();
        $('#exampleModalCenter').modal('show');
        $('#ErrorMessage').text("Please select one ticket at a time for the contest !");
    }
    else {

    }

    if (array.length < 1) {
        event.preventDefault();
        $('#exampleModalCenter').modal('show');
        $('#ErrorMessage').text("Please select atleast one ticket for Contest...");
    }
})

// Under Progress


//$(document).on('click', '[id^=btnPay_]', function () {
//    debugger;
//    var array = [];
//    $("input:checked").each(function () {
//        debugger
//        array.push($(this).val());
//    });

//    array.forEach((item) => {
//        console.log("value", item)
//    })

//    if (array.length < 1) {
//        event.preventDefault();
//        $('#exampleModalCenter').modal('show');
//        $('#ErrorMessage').text("Please select atleast one ticket for Payment...");
//    }
//    else {

//    }

//    // This will be removed here
//    if (array.length >= 1) {
//        event.preventDefault();
//        $('#exampleModalCenter').modal('show');
//        $('#ErrorMessage').html(`<b style="color:blue;">Payment Module is Under Progress....</b>`);
//    }
//})

$("#btnPay").click((e) => {
    var array = [];
    $("input:checked").each(function () {
        debugger
        array.push($(this).val());
    });

    array.forEach((item) => {
        console.log("value", item)
    })

    if (array.length < 1) {
        event.preventDefault();
        $('#exampleModalCenter').modal('show');
        $('#ErrorMessage').text("Please select atleast one ticket for Payment...");
    }
    else {

    }

    // This will be removed here
    if (array.length >= 1) {
        event.preventDefault();
        $('#exampleModalCenter').modal('show');
        $('#ErrorMessage').html(`<b style="color:blue;">Payment Module is Under Progress....</b>`);
    }
})

$("#arrowleft_" + idNumber).click((e) => {
    debugger;
    jQuery('#Ticket_' + idNumber).show();
    jQuery('#detailedTicket_' + idNumber).addClass("TicketDetails").removeClass("TicketDetails_" + idNumber);
    jQuery('#detailedTicket_' + idNumber).hide();
})
$(".arrowleft").click((e) => {
    $("#Ticket").show();
    $(".ticketDetails").hide();
})

function Checkallcheckbox(currentClickedCheckbox) {

    var section1Chks = $('.ticket__inner').find(':checkbox');
    //var section2Chks = $('.ticket__inner').find(':checkbox');

    if ($(currentClickedCheckbox).is(':checked')) {
        section1Chks.prop('checked', true);
        $('.selectChecklbl').text("Un-Select all tickets");
        //section2Chks.prop('checked', true);
    } else {
        section1Chks.prop('checked', false);
        $('.selectChecklbl').text("Select all tickets");

        //section2Chks.prop('checked', false);
    }
}

