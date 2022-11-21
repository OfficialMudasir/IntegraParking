$(document).ready(function () {
    debugger;
    // Hide some info forms
    $(".ticketDetails").hide();
    $("#stateCase").hide();
    $("#invalid_email").hide();
    $("#invalid_confirmEmail").hide();
    $("#doesnot_Match").hide();


    
    // Detect Device

    var isMobile = {
        Android: function () {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function () {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function () {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function () {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Web: function () {
            return navigator.userAgent.match(/Chrome/i);
        },
        Windows: function () {
            return navigator.userAgent.match(/IEMobile/i);
        },
        None: function () {
            if (!(isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows() || isMobile.Web())) return "None"
        },
        any: function () {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows() || isMobile.Web() || isMobile.None());
        }
    };

    // Appeal information validations
    let firstName = "";
    let lastName = "";
    let Appeal_Address = "";
    let email = "";
    let confirmEmail = "";
    let appealReason = "";
    let appealDesc = "";
    let EmailEqual = false;

    let AppealData = {
        FirstName: "",
        LastName: "",
        Address: "",
        Email: "",
        AppealReason: "",
        AppealDesc: "",
        Device: isMobile.any()[0]
    };
    let InstallationDetails = {
        ticketnumber: $("#searchedTicketNumber").val(),
        plate: $("#platenumber").val(),
        InstallationId: $("#installationId").val(),
    }
    $("#firstname").change(function (e) {
        firstName = e.target.value;
        AppealData.FirstName = firstName;
        debugger;
        if (firstName == "") {
            var first = document.getElementById("firstNameErrorMsg")
            first.innerText = "First name can not be empty";
            first.style.color = "red";
            first.style.fontSize = "11px"
            first.blur();
        }
        else {
            $("#firstNameErrorMsg").text("");
        }

    });
    $("#lastname").change(function (e) {
        lastName = e.target.value;
        AppealData.LastName = lastName;
        if (lastName == "") {
            var first = document.getElementById("lastNameErrorMsg")
            first.innerText = "Last name can not be empty";
            first.style.color = "red";
            first.style.fontSize = "11px"
            first.blur();
        }
        else {
            $("#lastNameErrorMsg").text("");
        }
    });
    $("#Appeal_Address").change(function (e) {
        Appeal_Address = e.target.value;
        AppealData.Address = Appeal_Address;
        if (Appeal_Address == "") {
            var first = document.getElementById("addressErrorMsg")
            first.innerText = "Address can not be empty";
            first.style.color = "red";
            first.style.fontSize = "11px"
            first.blur();
        }
        else {
            $("#addressErrorMsg").text("");
        }
    });
    $("#email").change(function (e) {
        email = e.target.value;
        if (email == "") {

        }
        if (!IsEmail(email)) {
            $("#invalid_email").show();
        }
        else {
            $("#invalid_email").hide();
            AppealData.Email = email;
        }

    });
    $("#confirmEmail").change(function (e) {
        confirmEmail = e.target.value;
        if (!IsEmail(confirmEmail)) {
            $("#invalid_confirmEmail").show();
            $("#doesnot_Match").hide();
        }
        else if (confirmEmail != email) {
            $("#invalid_confirmEmail").hide();
            $("#doesnot_Match").show();
        }
        else {
            $("#invalid_confirmEmail").hide();
            $("#doesnot_Match").hide();
            EmailEqual = true;
        }

    });
    $("#reason").change(function () {
        appealReason = $(this).children("option:selected").val();
        AppealData.AppealReason = appealReason;


    });
    $("#desc").change(function (e) {
        appealDesc = e.target.value;
        AppealData.AppealDesc = appealDesc

    });
    $(".formbox").change(function (e) {
        if (firstName != "" && lastName != "" && Appeal_Address != "" && email != "" && confirmEmail != null && IsEmail(email) && IsEmail(confirmEmail) && EmailEqual) {
            $("#btnContinue").removeAttr("disabled");
        }
    });

    $("#btnContinue").click((e) => {
        $("#stateCase").show();
        $("#personalInformation").hide();
        $("#case").addClass("active");
        e.preventDefault();
    })

    //Load Google places addresses

    //google.maps.event.addDomListener(window, 'load', function () {
    //    var options = {
    //        types: ['(cities)'],
    //        componentRestrictions: { country: "in" }
    //    };

    //    var input = document.getElementById('Appeal_Address');
    //    var places = new google.maps.places.Autocomplete(input, options);


    //});

    google.maps.event.addDomListener(window, 'load', function () {
        var places = new google.maps.places.Autocomplete(document.getElementById('Appeal_Address'));
        google.maps.event.addListener(places, 'place_changed', function () {
            var place = places.getPlace();
            document.getElementById('txtAddress').value = place.formatted_address;
            document.getElementById('txtLatitude').value = place.geometry.location.lat();
            document.getElementById('txtLongitude').value = place.geometry.location.lng();
        });
    });

    // Validate Email Address
    function IsEmail(email) {
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!regex.test(email)) {
            return false;
        } else {
            return true;
        }
    }

    //Appeal Images drag and Drop as well as On change
    const fileInput = document.getElementById("pictureInput");
    let myFiles = {};
    // if you expect files by default, make this disabled
    // we will wait until the last file being processed
    let isFilesReady = true;
    let AppealImageList = [];


    fileInput.addEventListener("change", async (event) => {
        debugger;
        // clean up earliest items
        myFiles = {};
        // set state of files to false until each of them is processed
        isFilesReady = false;

        // this is to get the input name attribute, in our case it will yield as "picture"
        // I'm doing this because I want you to use this code dynamically
        // so if you change the input name, the result also going to effect
        const inputKey = fileInput.getAttribute("name");
        var files = event.srcElement.files;

        const filePromises = Object.entries(files).map((item) => {
            return new Promise((resolve, reject) => {
                const [index, file] = item;
                const reader = new FileReader();
                reader.readAsBinaryString(file);

                reader.onload = function (event) {
                    // if it's multiple upload field then set the object key as picture[0], picture[1]
                    // otherwise just use picture
                    const fileKey = `${inputKey}${files.length > 1 ? `[${index}]` : ""
                        }`;
                    // Convert Base64 to data URI
                    // Assign it to your object
                    myFiles[fileKey] = `data:${file.type};base64,${btoa(
                        event.target.result
                    )}`;
                    file.imagename = file.name;
                    file.date = file.lastModified;
                    file.fileSize = file.size;
                    file.ticketnumber = $("#searchedTicketNumber").val();
                    file.cityid = $("#installationId").val();
                    file.image = `data:${file.type};base64,${btoa(
                        event.target.result
                    )}`;
                    AppealImageList.push(file);
                    displayImages(file);
                    resolve();
                };
                reader.onerror = function () {
                    console.log("can't read the file");
                    reject();
                };
            });
        });

        Promise.all(filePromises)
            .then(() => {
                console.log("ready to submit");
                isFilesReady = true;

                // demo purposes only
                var json = JSON.stringify(myFiles, undefined, 2);
                // /demo purposes only
            })
            .catch((error) => {
                console.log(error);
                console.log("something wrong happened");
            });
    });

    // upoad image functionality

    if (typeof (window.FileReader) == 'undefined') {
        alert('Browser does not support HTML5 file uploads!');
    }
    dropOnMe.addEventListener("drop", dropHandler, false);

    dropOnMe.addEventListener("dragover", function (ev) {
        $("#dropOnMe").css("background-color", "lightgoldenrodyellow;");
        ev.preventDefault();
    }, false);

    function dropHandler(ev) {
        // Prevent default processing.
        ev.preventDefault();

        // Get the file(s) that are dropped.
        var filelist = ev.dataTransfer.files;
        if (!filelist) return;  // if null, exit now


        // get image base 64

        const filePromises = Object.entries(filelist).map((item) => {
            return new Promise((resolve, reject) => {
                const [index, file] = item;
                const reader = new FileReader();
                reader.readAsBinaryString(file);

                reader.onload = function (event) {
                    // if it's multiple upload field then set the object key as picture[0], picture[1]
                    // otherwise just use picture
                    const fileKey = `picture${filelist.length > 1 ? `[${index}]` : ""
                        }`;
                    // Convert Base64 to data URI
                    // Assign it to your object
                    myFiles[fileKey] = `data:${file.type};base64,${btoa(
                        event.target.result
                    )}`;
                    file.imagename = file.name;
                    file.date = file.lastModified;
                    file.fileSize = file.size;
                    file.ticketnumber = $("#searchedTicketNumber").val();
                    file.cityid = $("#installationId").val();
                    file.image = `data:${file.type};base64,${btoa(
                        event.target.result
                    )}`;
                    AppealImageList.push(file);
                    console.log("image File List : ", AppealImageList);
                    displayImages(file);

                    resolve();
                };
                reader.onerror = function () {
                    console.log("can't read the file");
                    reject();
                };
            });
        });

        Promise.all(filePromises)
            .then(() => {
                console.log("ready to submit");
                isFilesReady = true;

                // demo purposes only
                var json = JSON.stringify(myFiles, undefined, 2);

                // /demo purposes only
            })
            .catch((error) => {
                console.log(error);
                console.log("something wrong happened");
            });




    }

    

    $("#upload").click(function () {
        debugger;
        console.log("appead data: ", AppealData)
        let url = "";
        var data = new FormData();
        for (var i = 0; i < AppealImageList.length; i++) {
            data.append(AppealImageList[i].name, AppealImageList[i]);
        }
        $.ajax({
            type: "POST",
            url: "/Appeal/uploadData",
            data: {
                data: JSON.stringify(AppealImageList),
                appealData: JSON.stringify(AppealData),
                InstallationDetails: JSON.stringify(InstallationDetails)
            },
            dataType: "json",
            success: function (result) {
                if (result.success == true) {
                    window.location.replace("/Appeal/SaveAppealInformation?Org=Canmore&response=true");
                }
                else {
                    window.location.replace("/Appeal/SaveAppealInformation?Org=Canmore&response=false");
                }
            },
            error: function (e) {
                e.preventDefault();
                alert("There was error uploading files!");

            }
        });
    });

    //Display images

    function displayImages(image) {

        if (image != null) {
            var uploadImageOuter = document.createElement("div");
            uploadImageOuter.setAttribute("class", "upload-img-outer");
            uploadImageOuter.setAttribute("id", "imageouter" + image.lastModified)

            var uploadimg = document.createElement("div");
            uploadimg.setAttribute("class", "upload-img");
            uploadimg.setAttribute("id", "uploadimage" + image.lastModified);

            var imgoverlay = document.createElement("div");
            imgoverlay.setAttribute("class", "img-overlay");
            imgoverlay.setAttribute("id", "imageoverlay" + image.lastModified);
            imgoverlay.innerText = image.name;

            var img = document.createElement('img');
            img.setAttribute("id", "image" + image.lastModified);
            img.src = image.image;

            //var appealName = document.getElementById("appealName");
            //appealName.setAttribute("value", AppealImageList);
            //appealName.name = AppealImageList;

            //var appealBase64 = document.getElementById("appealBase64");
            //appealBase64.setAttribute("value", image.base64);
            //appealBase64.name = image.base64;

            $(".upload-row").append(uploadImageOuter);
            $("#imageouter" + image.lastModified).append(uploadimg);
            $("#uploadimage" + image.lastModified).append(img);
            $("#uploadimage" + image.lastModified).append(imgoverlay);
        }
    }
    dropOnMe.addEventListener("dragend", function (ev) {
        $("#dropOnMe").css("background-color", "lightgray;");
        $("#dropOnMe").text("");
        $("upload").click(function () { });
        ev.preventDefault();
    }, false);
});
$(document).on('click', '[id^=downloadTicketIcon_]', function () {
    debugger;
    let downloadId = this.id;
    let idNumber = this.id.split('_').pop();
    let pdfid = 'pdfimage_' + idNumber;
    let pdfElement = $("#" + pdfid).val();
    let AllPdfImageIds = event.target.querySelectorAll("[id^=pdfimage_]");

    if (pdfElement != undefined) {
        let linkSource = `data:application/octet-stream;base64,${pdfElement}`;
        let downloadLink = document.createElement("a");
        downloadLink.href = linkSource;
        downloadLink.download = `BlinkayTicket_${idNumber}.pdf`;
        downloadLink.click();
    }
    else {
        alert("Ticket copy is not available !!!");
        //$('#exampleModalCenter').modal('show');
        //$('#ErrorMessage').text("Ticket copy is not available !");
    }
});

//$("#ViewTicket").click((e) => {

//    $("#Ticket").hide();
//    $(".ticketDetails").show();

//})

$(document).on('click', '[id^=detailedTicketIcon_]', function (e) {
    debugger;
    idNumber = this.id.split('_').pop();
    var ticketId = 'Ticket_' + idNumber;
    var eyeBtnId = 'detailedTicketIcon_' + idNumber
    var detaildTicketId = 'detailedTicket_' + idNumber;
    arrowleft = 'arrowleft_' + idNumber;

    if (idNumber != null) {
        $("#Ticket").hide();
        $(".ticketDetails").show();
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

$(".arrowleft").click((e) => {
    $("#Ticket").show();
    $(".ticketDetails").hide();
})