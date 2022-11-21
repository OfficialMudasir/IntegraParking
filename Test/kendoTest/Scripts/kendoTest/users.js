
function users_DropDownListCountry_OnChange(e) {
    
    //e.sender.span[0].style.backgroundImage = "Url(../../Content/img/banderas/AD.gif)";
    e.sender.span[0].style.backgroundRepeat = "no-repeat";
    e.sender.span[0].style.backgroundPosition = "5px 7px";
    e.sender.span[0].style.paddingLeft = "20px";    

    $.ajax({
        type: 'POST',
        url: users_countryFlagUrl,
        data: { idCountry: e.sender.value() },
        success: function (data) {
            e.sender.span[0].style.backgroundImage = "Url(../../Content/img/banderas/" + data + ")";
        },
        error: function (xhr) {
            alert("error");
        }
    });

}

function users_ComboBoxCountry_OnChange(e) {
    
    e.sender.input[0].style.backgroundRepeat = "no-repeat";
    e.sender.input[0].style.backgroundPosition = "5px 7px";
    e.sender.input[0].style.paddingLeft = "20px";
    e.sender.input[0].style.paddingRight = "20px";

    $.ajax({
        type: 'POST',
        url: users_countryFlagUrl,
        data: { idCountry: e.sender.value() },
        success: function (data) {
            e.sender.input[0].style.backgroundImage = "Url(../../Content/img/banderas/" + data + ")";
        },
        error: function (xhr) {
            alert("error");
        }
    });

}

function users_PopupReady() {

    //alert($("#MainPhoneCountry"));

    var dropdownlist = $("#AlternativePhoneCountry").data("kendoComboBox");
    dropdownlist.list.width(500);
}
