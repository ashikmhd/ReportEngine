var modal;

function Confirmation() {
    if (confirm('Do you want to Delete..?') == true) {
        return true;
    } else {
        return false;
    }
}

function Closediv(id) {
    document.getElementById(id).style.display = 'none';
}

function Redirect(URL) {
    window.location.href = URL;
    return false;
}

function confirmsave() {
    if (confirm('Do you want to Save..?') == true) {
        return true;
    } else {
        return false;
    }
}

function confirmApproval() {
    if (confirm('Do you want to Approve..?') == true) {
        return true;
    } else {
        return false;
    }
}

function confirmupdate() {
    if (confirm('Do you want to Update?') == true) {
        return true;
    } else {
        return false;
    }
}


function Validatetxtlen(text, length) {
    //asp.net textarea maxlength doesnt work; do it by hand
    var object = document.getElementById(text.id) //get your object
    if (object.value.length > length) {
        object.focus(); //set focus to prevent jumping
        object.value = text.value.substring(0, length); //truncate the value
        object.scrollTop = object.scrollHeight; //scroll to the end to prevent jumping
        return false;
    }
    return true;
}

// Popup window code
function newPopup(url) {
    document.location.href = url;
}

function Popup(url) {
    popupWindow = window.open(
        url, 'popUpWindow', 'height=600px,width=1000px,left=0,top=0,statusbar=0,menubar=0,resizable=yes,scrollbars=yes,toolbar=no,menubar=0,location=0,directories=0,status=yes');
    return false;
}

function PopupFromAPopUp(url, WindowName, height, width) {
    popupWindow = window.open(
        url, WindowName, 'height=' + height + 'px,width=' + width + 'px,left=0,top=0,statusbar=0,menubar=0,resizable=0,scrollbars=yes,toolbar=no,menubar=0,location=0,directories=0,status=yes');
    return false;
}

function Mainpopup(url) {
    window.open(url, 'Audit Management System', 'height=' + screen.height + ',width=' + screen.width + ',statusbar=0,menubar=0,resizable=yes,scrollbars=yes,toolbar=no,menubar=0,location=no,directories=0,status=yes')
}

function PopupNewWindow(url, WindowName) {
    popupWindow = window.open(
        url, WindowName, 'height=600px,width=1000px,left=0,top=0,statusbar=0,menubar=0,resizable=yes,scrollbars=yes,toolbar=no,menubar=0,location=0,directories=0,status=yes');
    return false;
}

function PopupWindow(url, WindowName, height, width) {
    popupWindow = window.open(
        url, WindowName, 'height=' + height + 'px,width=' + width + 'px,left=0,top=0,statusbar=0,menubar=0,resizable=0,scrollbars=yes,toolbar=no,menubar=0,location=0,directories=0,status=yes');
    return false;
}
////  <script language='JavaScript' type='text/JavaScript'> 
//    // http://htmlgenerator.weebly.com 
//    var tenth = ''; 
// 
//    function ninth() { 
//        if (document.all) { 
//            (tenth);
//           alert("Right Click Disable"); 
//           return false; 
//        } 
//    } 
// 
//    function twelfth(e) { 
//        if (document.layers || (document.getElementById && !document.all)) { 
//            if (e.which == 2 || e.which == 3) {
//                (tenth);
//                alert("Right Click Disable"); 
//                return false; 
//            } 
//        } 
//    } 
//    if (document.layers) { 
//        document.captureEvents(Event.MOUSEDOWN); 
//        document.onmousedown = twelfth; 
//    } else { 
//        document.onmouseup = twelfth; 
//        document.oncontextmenu = ninth;
//    }

//    document.oncontextmenu = new Function('alert("Right Click Disable"); return false')
//</script> 

//    function openWin() {
//        var myWindow = window.open("", "myWindow", "width=200,height=100");
//        myWindow.document.write("<p>This is 'myWindow'</p>");
//        setTimeout(function () { myWindow.close() }, 1000);
//    }



//Disable right click script III- By Renigade (renigade@mediaone.net)
//For full source code, visit http://www.dynamicdrive.com

//var message="";
/////////////////////////////////////
//function clickIE() { if (document.all) { (message); alert('Right Click is Disabled!'); return false; } }
//function clickNS(e) {if 
//(document.layers||(document.getElementById&&!document.all)) {
//        if (e.which == 2 || e.which == 3) { (message); alert('Right Click is Disabled!'); return false; } 
//    } 
//}
//if (document.layers) 
//{document.captureEvents(Event.MOUSEDOWN);document.onmousedown=clickNS;}
//else { document.onmouseup = clickNS; document.oncontextmenu = clickIE; }
//document.oncontextmenu=new Function("return false")
// --> 
function DisplayModal(ID) {
    //    document.getElementById("overlay").style.height = document.body.clientHeight + 'px';
    //    document.getElementById("overlay").className = "OverlayEffect";
    document.getElementById(ID).className = "ShowModal";
}

function RemoveModal(ID) {
    document.getElementById(ID).className = "HideModal";
    document.getElementById("overlay").className = "";
    return false;
}
//        $(document).ready(function () {
function GridDisplay(ID, Title) {
    if (Title == null || Title == '')
        Title = 'File';

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var dateformatted = dd + '' + mm + '' + yyyy + '' + today.getTime();
    Title = Title + '_' + dateformatted;
    if ($("[id$=" + ID + "] tr").length > 1) {
        $('#' + ID).DataTable({
            pageLength: 10,
            responsive: true,
            dom: '<"col-md-6" B><"col-md-3" l><"col-md-3" f>t<"pull-right" ip>',
            buttons: [{
                    extend: 'csv'
                },
                {
                    extend: 'excel',
                    title: Title
                },
                {
                    extend: 'pdf',
                    title: Title
                },
                {
                    extend: 'print',
                    customize: function (win) {
                        $(win.document.body).addClass('white-bg');
                        $(win.document.body).css('font-size', '10px');

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
                    }
                }
            ]

        });
    }

}

function notification(message, type) {
    $.notify({
        // options
        message: message
    }, {
        // settings
        type: type,
        allow_dismiss: true,
        placement: {
            from: 'top',
            align: 'center'
        },
        z_index: 1061
    });
}


function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

function RoundingUpOrDown(decimalValue) {

    if (parseInt(decimalValue) <= 12 && parseInt(decimalValue) > 0)
        return 0;
    else if (parseInt(decimalValue) > 12 && parseInt(decimalValue) <= 37)
        return 25;
    else if (parseInt(decimalValue) > 37 && parseInt(decimalValue) <= 62)
        return 50;
    else if (parseInt(decimalValue) > 62 && parseInt(decimalValue) <= 87)
        return 75;
    else if (parseInt(decimalValue) > 87)
        return 1;
    else
        return 0;
};