var identificadorVoz = 0;

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

async function SwalFireEstado(titlex) {
    var res = false;
    await Swal.fire({
        title: titlex,
        type: "info",
        html: "¿Desea cancelar la solicitud?",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Cancelar Solicitud',
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
function gPDF(image, nombre, documento) {
    var doc = new jsPDF();
    var pageSize = doc.internal.pageSize;
    var pageHeight = pageSize.height ? pageSize.height : pageSize.getHeight();
    var pageWidth = pageSize.width ? pageSize.width : pageSize.getWidth();

   
    doc.autoTable({
        html: '#convertirPDF',
        theme: 'plain',
        didParseCell: function (data) {
            if (data.row.index === 0) {
                data.cell.styles.fontSize = 20;
                data.cell.styles.halign = 'center';
            } else if (data.row.index === 2){
                data.cell.styles.fontSize = 13;
                data.cell.styles.fontStyle = 'bold';
            }else{
                data.cell.styles.fontSize = 12;
            }
        },
        didDrawPage: function (data) {
            // Header
            var logo = 'data:image/png;base64,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';
            doc.addImage(logo, 'JPEG', 10, 10, 55, 20);
            doc.setFontSize(15);
            doc.setFontType('italic')
            doc.text("Nihil Prius Fide (Nada antes que la fe)", pageWidth - 100, 20);
            doc.setFontType('normal')
            doc.setLineWidth(0.5);
            doc.setDrawColor(0, 0, 0) 
            doc.line(0, 32, pageWidth, 32);
            // Footer
            var str = "Página " + doc.internal.getNumberOfPages()
            doc.setFontSize(10);
            doc.text(str, pageWidth / 2, pageHeight - 10, 'center'); //23
        },
        margin: {
            top: 40,
            left: 20,
            right: 20,
        }
    });

    var imgData = 'data:image/jpeg;base64,' + image;

    if (doc.autoTableEndPosY() + 46 < pageHeight - 23) {
        doc.addImage(imgData, 'JPEG', (pageWidth / 2) - 27, doc.autoTableEndPosY() + 1, 55, 35);
        doc.setFontSize(10);
        doc.text(nombre, pageWidth / 2, doc.autoTableEndPosY() + 41, 'center');
        doc.text(documento, pageWidth / 2, doc.autoTableEndPosY() + 46, 'center');
    } else {
        doc.addPage();
        var logo = 'data:image/png;base64,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';
        doc.addImage(logo, 'JPEG', 10, 10, 55, 20);
        doc.setFontSize(15);
        doc.setFontType('italic')
        doc.text("Nihil Prius Fide (Nada antes que la fe)", pageWidth - 100, 20);
        doc.setFontType('normal')
        doc.setLineWidth(0.5);
        doc.setDrawColor(0, 0, 0)
        doc.line(0, 32, pageWidth, 32);
        // Footer
        var str = "Página " + doc.internal.getNumberOfPages()
        doc.setFontSize(10);
        doc.text(str, pageWidth / 2, pageHeight - 10, 'center');
        doc.addImage(imgData, 'JPEG', (pageWidth / 2) - 27, 37, 55, 35);
        doc.setFontSize(10);
        doc.text(nombre, pageWidth / 2, 77, 'center');
        doc.text(documento, pageWidth / 2, 82, 'center');

    }
    
    //document.getElementById('docpdf').setAttribute('src', doc.output('datauristring'));

    $('#docpdf').attr('src', doc.output('datauristring'));
    return doc.output('datauristring');
}

function textSpeak(message) {
    var text = message;
    responsiveVoice.setDefaultVoice("Spanish Female");
    if (identificadorVoz === 0) {
        responsiveVoice.speak(text);
    }
   
}

function pauseAsistent() {
    identificadorVoz = 1;
    responsiveVoice.cancel();
}

function continueAsistent(message) {
    identificadorVoz =  0;
    responsiveVoice.speak(message);
}