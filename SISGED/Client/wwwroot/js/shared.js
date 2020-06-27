
async function SwalFireEliminacion(titlex,estado,nombreusuario) {
    var res = false;
    var texto = estado==="activo"?"Bloquear":"Desbloquear"
    await Swal.fire({
        title: titlex,
        type:"info",
        html: "Desea <b>" + texto + "</b> la cuenta del usuario <b>'" + nombreusuario+"'</b>",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, ' + texto+"!",
        cancelButtonText: "Volver"
    }).then((result) => {
        if (result.value) {
            res = true;
        } else {
            res = false;
        }
    });
    return res;
}
function gPDF() {
    var specialElementHandlers = {
        "#editor": function (element, redenderer) {
            return true;
        }
    };
    var doc = new jsPDF();
    doc.fromHTML($("#target").html(), 15, 15, {
        "width": 170,
        "elementHandlers": specialElementHandlers
    });
    $('#docpdf').attr('src', doc.output('datauristring'));
}