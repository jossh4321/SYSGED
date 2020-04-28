
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
