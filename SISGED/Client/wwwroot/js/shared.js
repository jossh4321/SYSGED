
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
function gPDF(image) {
    const doc = new jsPDF();
    doc.setFontSize(15);
    doc.autoTable({
        html: '#convertirPDF',
        theme: 'plain',
        didParseCell: function (data) {
            if (data.row.index === 0) {
                data.cell.styles.fontSize = 20;
                data.cell.styles.halign = 'center';
            }else{
                data.cell.styles.fontSize = 12;
            }
        } 
    });
    $('#docpdf').attr('src', doc.output('datauristring'));
}