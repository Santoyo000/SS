
function validarSoloNumeros(event) {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        event.preventDefault();
    return false;
        }
    return true;
    }

    //function validarFormulario() {
    //    var telefono = document.getElementById("<%= txtTelefono.ClientID %>").value;
    //    var mensaje = document.getElementById("mensaje");

    //// Validar longitud del teléfono
    //if (telefono.length !== 7) {
    //  /*  msj.innerHTML = "Favor de ingresar el teléfono a 7 dígitos.";*/
    //    alert("Favor validar datos: El teléfono debe tener 7 dígitos.");
    //return false; // Evitar el envío del formulario
    //    }

    //// Otras validaciones (preguntar)
    //return true;
    //}
